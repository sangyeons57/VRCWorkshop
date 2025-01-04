
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FullScreen : UdonSharpBehaviour
{
    public GameObject videoScreen;
    public GameObject background;
    public Vector2 quadRatio;
    public float multiply = 0.4f;
    public float distanceFromCamera = 0.30001f;
    public VRCUnityVideoPlayer player;
    
    public VRCUrl url;

    private bool isFullScreen = false;
    private bool acquireOrder = false;


    private void OnEnable()
    {
        player.LoadURL(url);
        
    }

    void Update()
    {
        if (!isFullScreen)
            return;
        
        if (Networking.LocalPlayer != null)
        {
            // 로컬 플레이어의 카메라 위치 가져오기
            Vector3 cameraPosition = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;

            // 로컬 플레이어의 카메라 회전 가져오기
            Quaternion cameraRotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            
            videoScreen.transform.position = cameraPosition + cameraRotation * Vector3.forward * distanceFromCamera;
            videoScreen.transform.rotation = cameraRotation;
            
            if(background != null)
            {
                background.transform.position = cameraPosition + cameraRotation * Vector3.forward * (distanceFromCamera + 0.01f);
                background.transform.rotation = cameraRotation;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stopVideo();
        }
        
        if (!acquireOrder && player.IsPlaying)
        {
            acquireOrder = true;
        }
        else if (acquireOrder && !player.IsPlaying )
        {
            stopVideo();
        }
    }

    public void stopVideo()
    {
         player.Stop();
         player.LoadURL(url);
         isFullScreen = false;
         videoScreen.SetActive(false); 
         if(background != null) 
             background.SetActive(false);   
    }

    public void ToggleFullScreen()
    {
        Debug.Log("toggle full screen");
        videoScreen.transform.localScale = new Vector3(quadRatio.x * multiply, quadRatio.y * multiply, 1);
        videoScreen.SetActive(true);
        if (background != null)
        { 
            background.SetActive(true); 
            background.transform.localScale = new Vector3(background.transform.localScale.x, background.transform.localScale.y, 1);
        }
        player.Play();
        isFullScreen = true;
        acquireOrder = false;
        Debug.Log(url);
        Debug.Log(videoScreen.ToString());
        Debug.Log(player);
    }

    public void Interact()
    {
        ToggleFullScreen();
    }
}
