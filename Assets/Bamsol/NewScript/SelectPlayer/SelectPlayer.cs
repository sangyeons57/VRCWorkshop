
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SelectPlayer : UdonSharpBehaviour
{
    /*
    * 선택된 사용자를 특정위치로 이동시키는 기능
    * 선택 방식 - 사용자 이름
    * 
    */
    
    public TMP_InputField usernameText;
    protected VRCPlayerApi player;

    void Start()
    {
        usernameText.GetComponent<Image>().color = Color.white;
        Debug.Log("player name : " + Networking.LocalPlayer.displayName);
        usernameText.text = Networking.LocalPlayer.displayName;
    }
    

    public void SetPlayer()
    { 
        Debug.Log("searching name: " + usernameText.text);
        VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
        VRCPlayerApi.GetPlayers(players);
        
        foreach (VRCPlayerApi tempPlayer in players)
        {
            if(tempPlayer == null)
                continue;
            
            string name = tempPlayer.displayName;
            Debug.Log(" player name: " + name);
            if (name.Equals(usernameText.text))
            { 
                usernameText.GetComponent<Image>().color = Color.blue;
                player = tempPlayer;
                break;
            }
        }
    }
}
