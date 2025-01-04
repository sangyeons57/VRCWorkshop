
using System.Linq;
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ObjectArrangementManager : UdonSharpBehaviour
{
    public ObjectArrangement[] objectArrangements;
    public string[] idArray;
    
    public GameObject[] eventListeners;
    void Start()
    {
        foreach (ObjectArrangement element in objectArrangements)
        {
            element.manager = this;
        }
    }

    public bool checkObjectArrangement(ObjectArrangement arrangement)
    {
        int index = 0;
        foreach (ObjectArrangement element in objectArrangements)
        {
            if (element == arrangement)
            {
                return arrangement.checkId(idArray[index]);
            }

            index++;
        }

        return false;
    }

    public void checkAllObjectArrangements()
    {
        bool[] successArrangements = new bool[objectArrangements.Length];
        for (int i = 0; i < objectArrangements.Length; i++)
        {
            successArrangements[i] = objectArrangements[i].checkId(idArray[i]);
        }

        if (Util.All(successArrangements)) 
        {
            //전부다 맞는 경우
            onAllObjectArrangements();
        }
        else
        {
            onNotAllObjectArrangements();
        }
        
    }

    public void onAllObjectArrangements()
    {
        foreach (GameObject element in eventListeners)
        {
            foreach (UdonBehaviour udonBehaviour in element.GetComponents<UdonBehaviour>())
            {
                udonBehaviour.SendCustomNetworkEvent(NetworkEventTarget.All, "onAllObjectArranged");
            }
        }
        Debug.Log("다 맞는경우");
    }

    public void onNotAllObjectArrangements()
    {
        foreach (GameObject element in eventListeners)
        {
            foreach (UdonBehaviour udonBehaviour in element.GetComponents<UdonBehaviour>())
            {
                udonBehaviour.SendCustomNetworkEvent(NetworkEventTarget.All, "OnNotAllObjectArranged");
            }
        }
        Debug.Log("틀린게 있는경우");
    }
}
