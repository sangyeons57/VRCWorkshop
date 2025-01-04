
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class TeleportUserTo : SelectPlayer
{
    public GameObject teleportPointer; 
    /*
    * 선택된 사용자를 특정위치로 이동시키는 기능
    * 선택 방식 - 사용자 이름
    * 
    */
    
    public void TeleportToTeleportPoint()
    {
        Debug.Log(player);
        if (player != null && player.IsValid())
        {
            Networking.SetOwner(player, gameObject);
            SendCustomNetworkEvent(NetworkEventTarget.Owner, "TeleportUser");
            usernameText.GetComponent<Image>().color = Color.white;
            usernameText.text = "";
            Debug.Log("teleporting player");
        }
    }

    public void TeleportUser()
    {
        Networking.LocalPlayer.TeleportTo(this.teleportPointer.transform.position, this.teleportPointer.transform.rotation);
    }
}
