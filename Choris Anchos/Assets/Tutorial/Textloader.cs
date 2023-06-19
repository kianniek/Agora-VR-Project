using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Textloader : MonoBehaviour
{
    public List<Sprite> Images;
    public Image mainImage;
    public int index;
    public bool change;
    public float time = 1;
    public float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime >= time)
        {
            tempTime = 0;
            wait();
        }        
    }

    public void wait()
    {
        if (index >= Images.Count)
        {
            index = 0;
        }
        else
        {
            mainImage.sprite = Images[index];
            index++;
        }
    }
}
