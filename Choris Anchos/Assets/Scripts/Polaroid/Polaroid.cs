using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using HurricaneVR.Framework.ControllerInput;
using FMODUnity;

public class Polaroid : MonoBehaviour
{
    public HVRPlayerInputs playerInputs;

    public GameObject photoPrefab = null;
    public MeshRenderer screenRenderer = null;
    public Transform spawnLocation = null;
    public GameObject photos;
    public GameObject photoLocation;
    public int counter = 10;
    
    private bool filmCartridge = true;

    private Camera renderCamera = null;
    private bool isGrabbed = false;
    private bool photoTaken = false;
    private bool rightTrigger = false;
    private bool leftTrigger = false;
    private StudioEventEmitter photoSound;

    public TMP_Text frameCount;

    private void Awake()
    { 
        renderCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        photoSound = GetComponent<StudioEventEmitter>();
        CreateRenderTexture();
    }

    void Update() {
        rightTrigger = playerInputs.RightController.TriggerButtonState.Active;
        leftTrigger = playerInputs.LeftController.TriggerButtonState.Active;

        frameCount.text = counter.ToString();

        if (!rightTrigger && !leftTrigger)
            photoTaken = false;

        if ((rightTrigger || leftTrigger) && isGrabbed && !photoTaken)
        {
            TakePhoto();
            photoTaken = true;
        } 
    }

    public void IsGrabbed(bool grabbed)
    {
        this.isGrabbed = grabbed;
    }

    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    public void TakePhoto()
    {
        photoCounter();
        if(filmCartridge){
            Photo newPhoto = CreatePhoto();
            SetPhotoImage(newPhoto);
            photoSound.Play();
        }
    }

    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        photoObject.SetActive(true);
        photoObject.GetComponent<Collider>().enabled = true;
        return photoObject.GetComponent<Photo>();
    }

    private void SetPhotoImage(Photo photo)
    {
        Texture2D newTexture = RenderCameraToTexture(renderCamera);
        photo.SetImage(newTexture);
    }

    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(256, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        photo.Apply();

        return photo;
    }

    public void photoCounter(){
        counter--;
        Debug.Log(counter);

        if(counter < 0){
            filmCartridge = false;
            counter = 0;
        }
    }
}
