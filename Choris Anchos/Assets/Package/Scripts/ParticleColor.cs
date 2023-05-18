using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColor : MonoBehaviour
{
    TrailRenderer myTrail;
    Color cleur = new Color();
    private void Awake()
    {

    }
    void Start()
    {

        myTrail = this.GetComponent<TrailRenderer>();
        for (int i = 0; i < 3; i++)
        {
            int radom = (int)Random.Range(0, 2);
            Debug.Log(radom);
            if (radom >= 1)
            {
                cleur[i] = 255;
                cleur.a = Random.Range(0f,0.1f);
            }
        }
        myTrail.material.color = cleur;

    }
}
