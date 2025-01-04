
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class ShowVideoToPlayer : SelectPlayer
{
    
    public FullScreen fullScreen;

    public void ShowVideo()
    {
        if (player != null && player.IsValid())
        {
            Networking.SetOwner(player, gameObject);
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "ShowVideoAndPlay");
        }
    }

    public void ShowVideoAndPlay()
    {
        fullScreen.ToggleFullScreen();
    }
}
