using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideInObject : MonoBehaviour
{
    [SerializeField] private Transform startPosition; // starting position
    [SerializeField] private Transform endPosition; // ending position
    [SerializeField] private float timeToMove = 1f; // time it takes to move
    private readonly float timer = 0f; // timer to keep track of time
    [Tooltip("Slide Object in when scene gets loaded in")]
    [SerializeField] private bool slideOnLoad;
    [SerializeField] private AnimationCurve animCurve;
    private float value;

    private void OnEnable()
    {
        endPosition.transform.position = gameObject.transform.position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //mat.GetComponent<Material>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // start the movement coroutine if slideOnLoad is true
        if (slideOnLoad)
        {
            StartCoroutine(SlideInEffect());
        }
    }

    private void Update()
    {
        // manually trigger the coroutine if slideOnLoad is false
        if (!slideOnLoad)
        {
            StartCoroutine(SlideInEffect());
        }
    }

    private IEnumerator SlideInEffect()
    {
        // move the object to the starting position
        transform.position = startPosition.position;

        // initialize timer
        float timer = 0f;

        while (timer < timeToMove)
        {
            // increase timer
            timer += Time.deltaTime;

            // calculate the percentage of time elapsed
            float percentageComplete = timer / timeToMove;
            value = animCurve.Evaluate(percentageComplete);
            //animCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
            // move the object based on percentage complete
            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, value);

            // wait for the next frame
            yield return null;
        }

        // move the object to the ending position
        transform.position = endPosition.position;
        Destroy(this);
    }
}
