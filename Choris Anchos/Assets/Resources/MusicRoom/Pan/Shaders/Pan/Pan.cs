using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    [Header("Note")]
    [SerializeField] PanNotes note;
    // Start is called before the first frame update
    [SerializeField] private GameObject scannerPref;
    [SerializeField] private bool bong = false;
    [SerializeField] private float scannerLifeTime;


    [Header("pitch")]

    [SerializeField] private float min = 0.1f;
    [SerializeField] private float max = 2f;

    [Header("Ping")]

    [SerializeField] private float speed = 3;
    [SerializeField] private float Macht = 3;
    [SerializeField] private float devide = 10;
    void Update()
    {
        if (bong)
        {
            PlayBong();
            bong = false;

        }
    }

    private void PlayBong()
    {
        //play audio
        AudioManager.instance.PlayPanNote(note, this.transform.position);
        //instatiate pulse
        GameObject scannerPulse = Instantiate(scannerPref, this.transform.position, this.transform.rotation);
        ScannerSphere myScanner = scannerPulse.GetComponent<ScannerSphere>();
        myScanner.setSpeed(Mathf.Pow(speed, Macht) / devide);
        Destroy(scannerPulse, scannerLifeTime);
    }

    public void DoesBong()
    {
        bong = true;
    }
}
