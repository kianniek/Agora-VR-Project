using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    [SerializeField] EventReference note;
    // Start is called before the first frame update
    [SerializeField] private GameObject scannerPref;
    [SerializeField] private bool bong = false;
    [SerializeField] private float scannerLifeTime;

    [Header("pitch")]

    [SerializeField] private float min = 0.1f;
    [SerializeField] private float max = 2f;

    [Header("Ping")]

    [SerializeField] private float Macht = 3;
    [SerializeField] private float devide = 10;
    void Update()
    {
        if (bong)
        {
            playBong();
            bong = false;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        playBong();
    }

    private void playBong()
    {
        float speedPitch = Random.Range(min, max);
        GameObject scannerPulse = Instantiate(scannerPref, this.transform.position, this.transform.rotation);
        ScannerSphere myScanner = scannerPulse.GetComponent<ScannerSphere>();
        //scannerPulse.GetComponent<AudioSource>().pitch = speedPitch;

        AudioManager.instance.PlayOneShot(note, this.transform.position);

        myScanner.setSpeed(Mathf.Pow(speedPitch, Macht) / devide);
        Destroy(scannerPulse, scannerLifeTime);
    }

    public bool DoesBong()
    {
        return bong = true;
    }
}
