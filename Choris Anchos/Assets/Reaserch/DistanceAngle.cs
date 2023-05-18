using UnityEngine;
using UnityEngine.InputSystem;

public class DistanceAngle : MonoBehaviour
{
    public Transform Target;
    public Transform Self;
    public float lineDuration = 10f;
    public TransforData Scriptable;

    private bool _showingLine;
    private float _lineStartTime;
    private bool setPress;

    void Update()
    {
        // Check if space bar was pressed
        if (setPress)
        {
            setPress = false;
            Scriptable.positions.items.Add(Target.position);
            Scriptable.rotations.items.Add(Target.transform.eulerAngles);
            // Show line if it's not already being shown
            if (!_showingLine)
            {
                _showingLine = true;
                _lineStartTime = Time.time;

                // Get the vector between the two objects
                Vector3 direction = Self.position - Target.position;

                // Get the distance between the two objects
                float distance = direction.magnitude;

                // Get the angle between the two objects
                float angle = Vector3.Angle(direction, Target.forward);

                // Print the distance and angle to the console
                Debug.Log("Distance: " + distance.ToString("F2") + " units");
                Debug.Log("Angle: " + angle.ToString("F2") + " degrees");

                // Set the positions for the line renderer
            }
        }
    }

    public void LineTrigger()
    {
        setPress = true;
  
    }

}