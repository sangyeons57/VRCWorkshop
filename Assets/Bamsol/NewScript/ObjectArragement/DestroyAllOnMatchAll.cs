
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class DestroyAllOnMatchAll : UdonSharpBehaviour
{
    public GameObject[] destroyObjects;
    public void onAllObjectArranged()
    {
        for (int i = destroyObjects.Length; i > 0; i--)
        {
            // destroyObjects[i - 1].SetActive(false);
            Destroy(destroyObjects[i - 1]);
        }
    }
}
