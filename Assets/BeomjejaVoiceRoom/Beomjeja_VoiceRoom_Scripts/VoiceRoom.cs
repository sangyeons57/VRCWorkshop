
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VoiceRoom : UdonSharpBehaviour
{
    [SerializeField, Range(0, 24)] private int VoiceSize;
    [SerializeField] private int MaximumVoice_Distance;
    [SerializeField] private int Voice_Distance;

    [Header("VRCWorld가 VoiceRoom 안에 있는지 여부")]
    [SerializeField] bool HasVRCWorld = false;

    [Header("VoiceCollider 내 포함가능한 최대 인원수(1~80)")]
    [Tooltip("불필요하게 과도한 인원수는 성능저하를 일으킬 수 있습니다.")]
    [SerializeField, Range(1, 80)] int MaximumInVoiceCollier = 1;
    private int[] inplayerint;

    void Start()
    {
        inplayerint = new int[MaximumInVoiceCollier];
        for (int i = 0; i < inplayerint.Length; i++)
        {
            inplayerint[i] = -1;
        }
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private bool Existplayer(VRCPlayerApi player)
    {
        for (int i = 0; i < inplayerint.Length; i++)
        {
            if (inplayerint[i] == player.playerId)
            {
                return true;
            }
        }
        return false;
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!Existplayer(player)) Addplayer(player);
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (Existplayer(player)) Subtract(player);
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (HasVRCWorld)
        {
            if (!Existplayer(player)) Addplayer(player);
        }
    }
    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        if (HasVRCWorld)
        {
            if (!Existplayer(player)) Addplayer(player);
        }
        else
        {
            if (Existplayer(player)) Subtract(player);
        }
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Existplayer(player)) Subtract(player);
    }
    private void Addplayer(VRCPlayerApi player)
    {
        for (int i = 0; i < inplayerint.Length; i++)
        {
            if (inplayerint[i] == -1)
            {
                inplayerint[i] = player.playerId;
                //Debug.Log("In "+inplayerint[i]);
                break;
            }
        }
        voicesetting();
    }
    private void Subtract(VRCPlayerApi player)
    {
        for (int i = 0; i < inplayerint.Length; i++)
        {
            if (inplayerint[i] == player.playerId)
            {
                //Debug.Log("Out "+inplayerint[i]);
                inplayerint[i] = -1;
            }
        }
        voicesetting();
        voicedefault(player);
    }
    public void voicesetting()
    {
        for (int i = 0; i < inplayerint.Length; i++)
        {
            if (inplayerint[i] != -1)
            {
                if (Existplayer(Networking.LocalPlayer))
                {
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceGain(15);
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceDistanceNear(0);
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceDistanceFar(25);
                }
                else
                {
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceGain(VoiceSize);
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceDistanceNear(MaximumVoice_Distance);
                    VRCPlayerApi.GetPlayerById(inplayerint[i]).SetVoiceDistanceFar(Voice_Distance);
                }
            }
        }
    }
    public void voicedefault(VRCPlayerApi player)
    {
        player.SetVoiceGain(15);
        player.SetVoiceDistanceNear(0);
        player.SetVoiceDistanceFar(25);
    }
}

