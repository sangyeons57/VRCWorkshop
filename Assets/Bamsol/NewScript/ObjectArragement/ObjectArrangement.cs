
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ObjectArrangement : UdonSharpBehaviour
{
    public GameObject fixPosition;
    
    [HideInInspector]
    public bool hasFixedObject;
    [HideInInspector]
    public ObjectArrangementManager manager;
    
    private ObjectIndicator fixedObject;
    void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        
        boxCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasFixedObject) 
        {
            ObjectIndicator indicator = other.GetComponent<ObjectIndicator>();
            if(indicator == null)
                return;
            
            //오브젝트 fix(고정 배치됨)
            if (!indicator.isFixed && !indicator.pickup.IsHeld)
            {
                setFixedObject(indicator);
                manager.checkAllObjectArrangements();
            }
        }
    }

    private void setFixedObject(ObjectIndicator indicator) 
    { 
        clearFixedObject();
            
        hasFixedObject = true;
        this.fixedObject = indicator; 
        
        indicator.fixObject(this); 
        
        indicator.gameObject.transform.position = fixPosition.transform.position;
        // indicator.gameObject.transform.rotation = fixPosition.transform.rotation;
    }

    public void clearFixedObject()
    {
        hasFixedObject = false;
        this.fixedObject = null;
    }

    public bool checkId(String id)
    {
        if (fixedObject == null)
        {
            return id.Equals("");
        }
        else
        { 
            return id.Equals(fixedObject.id);
        }
    }

    public bool checkId()
    {
        return manager.checkObjectArrangement(this);
    }
}
