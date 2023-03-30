using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideInObject : MonoBehaviour
{
    [SerializeField] Transform startPosition; // starting position
    [SerializeField] Transform endPosition; // ending position
    [SerializeField] float timeToMove = 1f; // time it takes to move
    private float timer = 0f; // timer to keep track of time
    [Tooltip("Slide Object in when scene gets loaded in")]
    [SerializeField] bool slideOnLoad;
    [SerializeField] AnimationCurve animCurve;
    private float value;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //mat.GetComponent<Material>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // start the movement coroutine if slideOnLoad is true
        if (slideOnLoad)
        {
            StartCoroutine(SlideInEffect());
        }
    }

    void Update()
    {
        // manually trigger the coroutine if slideOnLoad is false
        if (!slideOnLoad)
        {
            StartCoroutine(SlideInEffect());
        }
    }

    IEnumerator SlideInEffect()
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
