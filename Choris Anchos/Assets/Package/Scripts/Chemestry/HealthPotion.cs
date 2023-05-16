using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPotion : MonoBehaviour
{
    private bool canOil = true;
    private float Timer = 3;
    [SerializeField] private ParticleSystem drip;
    [SerializeField] private Color empty;
    [SerializeField] private GameObject liquid;
    [SerializeField] private string effect;
    public int prize;
    [SerializeField] private bool rightOne;
    public UnityEvent myEvent;
    public List<GameObject> potionParts;

    private void Start()
    {
        float random = Random.Range(0, 255);
        int randomFull = (int)Random.Range(-0.4f, 3);
        Color x = new Color(0,0,0);
        x[randomFull] = 255;
        
        foreach (GameObject obj in potionParts)
        {
            obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", x);
        }

    }


    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        bool isVisible = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.green);
        if (canOil && this.transform.eulerAngles.z >= 160f && this.transform.eulerAngles.z <= 200f)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, Mathf.Infinity) &&
                hit.transform.gameObject.CompareTag("MainCamera") && hit.transform.eulerAngles.x >= 250f && hit.transform.eulerAngles.x <= 310f && isVisible)
            {
                if (!drip.isPlaying)
                {
                    drip.Play();
                }
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    myEvent.Invoke();
                    canOil = false;
                    this.liquid.GetComponent<Renderer>().material.color = empty;
                }
            }
        }
        else
        {
            drip.Stop();
            Timer = 3;
        }

    }
    public void setPrize(int prize)
    {
        this.prize = prize;
    }
}
