
using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class StartProduction : UdonSharpBehaviour
{
    public Canvas canvas;
    public float changeInterval ;
    public PostProcessVolume postProcessVolume;
    
    public float wakeUpSpeed;
    
    public bool playOnStart;
    
    private float progress;
    private bool isWakeUp;
    
    private int productionIndex = 0;
    private float timer;
    private bool isStart  = false;

    public void Interact() 
    {
        PlayProduction();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        if (playOnStart)
        {
             PlayProduction();
        }
    }

    public void PlayProduction()
    {
        isStart = true;
        timer = 0;
        
        Debug.Log("Play Production" + canvas.gameObject.name);
        postProcessVolume.enabled = true; 
        postProcessVolume.weight = 1;
        canvas.gameObject.SetActive(true);
        ResetProduction();
        NextProduction();
    }

    
    public void Update()
    {
        if (isWakeUp)
        {
            progress += Time.deltaTime * wakeUpSpeed; 
            
            postProcessVolume.weight = 1 - progress;
            
            if (progress >= 1)
                EndWakeup();
        }
        
        if (!isStart)
        {
            return;
        }

        if (timer >= changeInterval)
        {
            timer -= changeInterval;
            NextProduction();
        }
        
        timer += Time.deltaTime;
    }
    
    public void StartWakeup()
    {
        if (postProcessVolume == null)
            return;

        isWakeUp = true;
        progress = 0;
        postProcessVolume.enabled = true;
    }
    public void EndWakeup() 
    {
        isWakeUp = false;
        postProcessVolume.enabled = false;
    }

    private void NextProduction()
    {
        if (productionIndex > 0)
        {
            Debug.Log("Next production active false " + canvas.transform.GetChild(productionIndex - 1).gameObject.name);
            canvas.transform.GetChild(productionIndex - 1).gameObject.SetActive(false);
        } 
        
        if (productionIndex >= canvas.transform.childCount) 
        {
            FinishProduction();
            return;
        }
            
        Debug.Log("Next production active true " + canvas.transform.GetChild(productionIndex).gameObject.name);
        canvas.transform.GetChild(productionIndex).gameObject.SetActive(true);
        productionIndex++;
    }

    private void ResetProduction()
    {
        foreach (RectTransform production in canvas.transform)
        {
            production.gameObject.SetActive(false);
        }
        productionIndex = 0;
    }

    private void FinishProduction()
    { 
        isStart = false;
        StartWakeup();
        canvas.gameObject.SetActive(false);
    }
}
