using FMODUnity;
using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayPanOnMesh : MonoBehaviour
{
    const float PITCH_C4 = 261.63f;

    [InspectorButton("DebugModeAudio")]
    public int TestPitchButton;
    [SerializeField] SphereScanInstantiator sphereScanInstantiatorScript;
    [SerializeField] AsyncGPUReadbackMeshMusic raiseMeshManager;
    [Space(10)]
    [SerializeField] EventReference note;  // Reference to the audio event to be played
    [SerializeField] Transform handpanRotation;  // Reference to the handpan's rotation transform
    [SerializeField] Transform[] notePositions;  // Array of sound positions on the handpan
    [SerializeField] Transform[] notePositionsProjected;  // Array of sound positions on a plane
    [Space(10)]
    public float minPitch = 0f;  // Minimum pitch value for the audio event
    public float maxPitch = 10;  // Maximum pitch value for the audio event
    [Range(-10, 10)]
    public float _pitchOffset = 0;  // Maximum pitch value for the audio event

    [Space(10)]
    [Tooltip("Delay between collisiondetections")]
    public float delay = 0.5f;
    public float minHitVelocity = 0.1f;
    public float maxHitVelocity = 5f;

    [SerializeField] private bool _debugMode = false;  // Flag for enabling or disabling debug mode

    float timeElapsed;
    float lerpDurationBegin = 0.01f;
    float lerpDurationEnd = 3f;

    private bool canPlay = true;
    private bool upDownSwitch;

    // Called when the script instance is being loaded
    private void Awake()
    {
        if (!sphereScanInstantiatorScript)
        {
            Debug.LogWarning("No SphereScanInstantiator Script, attach the SphereScanInstantiator Script to this object");
        }

        notePositionsProjected = new Transform[notePositions.Length];
        // Loop through each sound position on the handpan
        for (int i = 0; i < notePositions.Length; i++)
        {
            // Create a plane with dimensions width=1 and height=1
            Plane plane = new Plane(handpanRotation.up, handpanRotation.position);

            // Get the collision point
            Vector3 point = notePositions[i].localPosition;

            // Project the collision point onto the plane
            Vector3 projectedPoint = PlaneMath.ProjectPointOnPlane(point, plane);

            // Set the sound position to the projected point on the plane
            notePositions[i].localPosition = point = PlaneMath.PointCirleToPointPlane(handpanRotation.localPosition, projectedPoint);
            notePositionsProjected[i] = notePositions[i];
            // Draw a debug marker at the projected point if debug mode is enabled
            DebugModeVisual(point, Color.red);
        }
    }

    // Called when a collision occurs
    private IEnumerator OnCollisionEnter(Collision collision)
    {
        // Check if collision object is null
        if (collision == null)
        {
            yield break;
        }
        //check if the last hit has been longer than the set dalay
        if (!canPlay)
        {
            yield break;
        }
        //check if the velocity treshhold has been crossed
        if(collision.relativeVelocity.magnitude > maxHitVelocity)
        {
            AudioManager.instance.PlayOneShot3D(note, transform.position, 100f);
            yield break;
        }

        if (collision.relativeVelocity.magnitude < minHitVelocity)
        {
            //AudioManager.instance.PlayOneShot3D(sound, transform.position, 100f);
            yield break;
        }

        canPlay = false;

        // Create a plane with dimensions width=1 and height=1
        Plane plane = new Plane(handpanRotation.up, handpanRotation.localPosition);

        // Get the collision point
        Vector3 point = collision.GetContact(0).point;

        // Project the collision point onto the plane
        Vector3 projectedPoint = PlaneMath.ProjectPointOnPlane(point, plane);

        // Calculate the pitch based on the projected point
        float pitch = CalculatePitchFromCollision(projectedPoint);

        // Play the sound with the calculated pitch
        Debug.Log("Playing Handpan on frequency " + pitch);
        AudioManager.instance.PlayOneShot3D(note, transform.position, pitch);

        if (sphereScanInstantiatorScript)
        {
            sphereScanInstantiatorScript.SetPitchAndPlay(pitch);
        }

        if (raiseMeshManager)
        {
            upDownSwitch = !upDownSwitch;

            int _switch = upDownSwitch ? Random.Range(1, 5) : 1;
            float targetHeight = Mathf.Abs(pitch) * _switch;
            raiseMeshManager.raiseAmount = targetHeight;
            //StopCoroutine(LerpHeightBegin(targetHeight));
            //timeElapsed = 0;
            //StartCoroutine(LerpHeightBegin(targetHeight));
        }

        StartCoroutine(WaitBeforeNextHit());
    }

    IEnumerator WaitBeforeNextHit()
    {
        yield return new WaitForSeconds(0.5f);
        canPlay = true;
    }

    IEnumerator LerpHeightBegin(float targetHeight)
    {
        while (raiseMeshManager.raiseAmount != targetHeight)
        {
            raiseMeshManager.raiseAmount = Mathf.Lerp(raiseMeshManager.raiseAmount, targetHeight, timeElapsed / lerpDurationBegin);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        raiseMeshManager.raiseAmount = targetHeight;
        timeElapsed = 0;
        StopCoroutine(LerpHeightEnd());
        StartCoroutine(LerpHeightEnd());
    }

    IEnumerator LerpHeightEnd()
    {
        while (raiseMeshManager.raiseAmount != 1)
        {
            raiseMeshManager.raiseAmount = Mathf.Lerp(raiseMeshManager.raiseAmount, 1, timeElapsed / lerpDurationEnd);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        raiseMeshManager.raiseAmount = 1;
    }
    private float CalculatePitchFromCollision(Vector3 point)
    {
        point = PlaneMath.PointCirleToPointPlane(handpanRotation.position, point);
        Debug.Log(point);
        DebugModeVisual(point, Color.gray);

        float pitch = 0f;
        int noteCount = 0;

        // Find the 3 closest sound positions to the given point
        Transform[] closestNotes = notePositionsProjected.OrderBy(n => Vector3.Distance(n.position, point)).Take(3).ToArray();

        //if(closestNotes)

        // Calculate the weights for each sound based on their distance to the point
        float totalDistance = 0f;
        float[] weights = new float[3];
        for (int i = 0; i < closestNotes.Length; i++)
        {
            float distance = Vector3.Distance(closestNotes[i].position, point);
            weights[i] = 1f / Mathf.Max(distance, 0.0001f); // Avoid division by zero
            totalDistance += weights[i];

            //Note sound = closestNotes[i].GetComponent<Note>();
            //if (sound != null)
            if (closestNotes[i].TryGetComponent<Note>(out var note))
            {
                pitch += note.pitch;
                noteCount++;
            }
        }

        // Calculate the sum of pitch values from the closest notes
        float pitchSum = 0f;
        for (int i = 0; i < closestNotes.Length; i++)
        {
            float distance = Vector3.Distance(closestNotes[i].position, point);
            float normalizedDistance = Mathf.Clamp01(distance / maxPitch); // Normalize the distance between 0 and 1
            float notePitch = Mathf.Lerp(minPitch, maxPitch, normalizedDistance); // Calculate the pitch for the sound
            pitchSum += notePitch; // Add the pitch value to the sum
        }

        // Calculate the average pitch
        float combinedPitch = (pitch + pitchSum / closestNotes.Length) / 2f;

        //The Note C4 is used as base so we need to offset the pitch based on the pitch of C4 and then normelize it
        float pitchOffset = Mathf.Log(combinedPitch / PITCH_C4, 2f) + _pitchOffset;
        return pitchOffset;
    }


    void DebugModeVisual(Vector3 point, UnityEngine.Color color)
    {
        if (!_debugMode) { return; }
        GameObject prim;
        if (color == UnityEngine.Color.red)
        {
            prim = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        else
        {
            prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        prim.transform.position = point;
        prim.transform.localScale = Vector3.one / 50;
        Destroy(prim.GetComponent<Collider>());
    }

    void DebugModeAudio()
    {
        float pitch = Mathf.Log(1 / PITCH_C4, 2f) + _pitchOffset;
        // Play the sound with the calculated pitch
        AudioManager.instance.PlayOneShot3D(note, transform.position, pitch);
        Debug.LogAssertion("Playing Handpan on frequency " + pitch);
        if (sphereScanInstantiatorScript)
        {
            sphereScanInstantiatorScript.SetPitchAndPlay(pitch);
        }

        if (raiseMeshManager)
        {
            upDownSwitch = !upDownSwitch;

            int _switch = upDownSwitch ? Random.Range(1,5) : 1;
            float targetHeight = Mathf.Abs(pitch) * _switch;
            raiseMeshManager.raiseAmount = targetHeight;
            //StopCoroutine(LerpHeightBegin(targetHeight));
            //timeElapsed = 0;
            //StartCoroutine(LerpHeightBegin(targetHeight));
        }
    }
}
public static class PlaneMath
{
    public static UnityEngine.Vector3 ProjectPointOnPlane(UnityEngine.Vector3 point, UnityEngine.Plane plane)
    {
        // Get the distance between the plane and the point
        float distance = plane.GetDistanceToPoint(point);

        // Get the projection of the point onto the plane
        UnityEngine.Vector3 projection = point - distance * plane.normal;

        return projection;
    }

    public static UnityEngine.Vector2 PointCirleToPointPlane(UnityEngine.Vector3 centerCircle, UnityEngine.Vector3 pointOnCirclePlane, float radiusCircle = 1f)
    {
        UnityEngine.Vector3 circlePointRelativeToCenter = pointOnCirclePlane - centerCircle;
        float r = circlePointRelativeToCenter.magnitude;
        float theta = Mathf.Atan2(circlePointRelativeToCenter.y, circlePointRelativeToCenter.x);
        System.Numerics.Complex z = new System.Numerics.Complex(r * Mathf.Cos(theta), r * Mathf.Sin(theta));

        float magnitudeSquared = (float)(z.Real * z.Real + z.Imaginary * z.Imaginary);
        float x = (float)(2 * z.Real) / (1 + magnitudeSquared);
        float y = (float)(2 * z.Imaginary) / (1 + magnitudeSquared);
        Vector2 squarePoint = new Vector2(x, y);
        return squarePoint;

    }
}
