using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPanOnMesh : MonoBehaviour
{
    [SerializeField] EventReference note;
    // Start is called before the first frame update
    [SerializeField] private GameObject scannerPref;
    [SerializeField] private bool play = false;
    [SerializeField] private float scannerLifeTime;

    [Header("Ping")]

    [SerializeField] private float Macht = 3;
    [SerializeField] private float devide = 10;
    void Update()
    {
        if (play)
        {
            PlayNotes();
            play = false;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayNotes();
    }

    private void PlayNotes()
    {
        //GameObject scannerPulse = Instantiate(scannerPref, this.transform.position, this.transform.rotation);
        //ScannerSphere myScanner = scannerPulse.GetComponent<ScannerSphere>();

        AudioManager.instance.PlayOneShot(note, this.transform.position);
        //Destroy(scannerPulse, scannerLifeTime);
    }

    public bool PlayPan()
    {
        return play = true;
    }
}
