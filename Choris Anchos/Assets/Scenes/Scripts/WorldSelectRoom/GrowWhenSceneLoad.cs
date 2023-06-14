using System.Collections;
using UnityEngine;

public class GrowWhenSceneLoad : MonoBehaviour
{
    public float targetScale = 1f; // The target scale that can be set in the editor
    public float duration = 0.3f; // The duration of the scaling animation in seconds
    public float speed = 1f; // The speed of the scaling animation

    void Awake()
    {
        // Set the initial scale to 0
        transform.localScale = Vector3.zero;

        // Use a coroutine to gradually scale up the object over time
        StartCoroutine(ScaleCoroutine());
    }

    IEnumerator ScaleCoroutine()
    {
        yield return new WaitForSeconds(3f);
        float elapsedTime = 0f;

        // Gradually scale up the object over time
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(0f, targetScale, t);
            transform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        // Set the final scale to the target scale
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
