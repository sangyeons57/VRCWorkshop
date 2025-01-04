
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WakeUp : UdonSharpBehaviour
{
    private Material material;

    public bool changeScale = false;
    public bool changeAlpha = false;
    public float initialScale = 2.0f;
    public float speed = 1f;
    public float finalScale = 200f;
    public float progress;

    public readonly float power = 3; 
    //2.30258509299f;

    private bool isExpanding = false;
    private bool isTracking = false;
    private VRCPlayerApi target;

    private float realScale;

    void Start()
    {
        target = Networking.LocalPlayer;

        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (isTracking)
            transform.position = target.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        
        if(isExpanding) {

            if(changeScale) {
                float scale = Mathf.Pow(progress, power) * realScale;
                transform.localScale += Vector3.one * scale * Time.deltaTime;
            }
            if(changeAlpha) {
                float alpha = 1 - Mathf.Pow(progress, power);
                material.SetFloat("_Alpha", alpha);
            }

            if(progress >= 1f) {
                isExpanding = false;
                gameObject.SetActive(false);
            }

            
            progress += speed * Time.deltaTime / (realScale > 1 ? realScale : -realScale);
        }
    }

    public void wakeup()
    {
        transform.localScale = Vector3.one * initialScale; 
        realScale = (finalScale - initialScale);

        isExpanding = true;
        isTracking = true;
        progress = 0f;
        gameObject.SetActive(true);
    }

    public WakeUp Init(bool scale, bool alpha, float initScale,  float finalScale, float speed)
    {
        this.initialScale = initScale;
        this.finalScale = finalScale;
        this.changeAlpha = alpha;
        this.changeScale = scale;
        this.speed = speed;
        
        return this;
    }

    public void setAlpha(float alpha)
    {
        Debug.Log(material + " is set to " + alpha);
        if (material != null) 
        {
            material.SetFloat("_Alpha", alpha);
            
        }
    }

    public void setScale(float scale)
    {
        transform.localScale = scale * Vector3.one;
    }

    public void setProgress(float progress)
    {
        this.progress = progress;
    }

    public void setState(bool isExpanding, bool isTracking)
    {
        this.isExpanding = isExpanding;
        this.isTracking = isTracking;
    }
}

