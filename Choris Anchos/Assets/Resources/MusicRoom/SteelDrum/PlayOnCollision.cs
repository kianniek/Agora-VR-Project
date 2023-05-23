/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayOnCollision : MonoBehaviour
{
    [SerializeField] UnityEvent events;
    public float cooldownTime = 30.0f;
    public bool canInvoke = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (canInvoke)
        {
            canInvoke = false;
            StartCoroutine(InvokeEventWithCooldown());
        }
    }

    IEnumerator InvokeEventWithCooldown()
    {
        events.Invoke();
        StartCoroutine(ResetInvokeCooldown());
        yield return null;
    }

    IEnumerator ResetInvokeCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canInvoke = true;
    }
}
*/