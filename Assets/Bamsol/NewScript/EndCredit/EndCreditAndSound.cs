
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EndCreditAndSound : UdonSharpBehaviour
{
    
    public GameObject[] endContent;
    public SoundPlay soundPlay;
    
    private void OnEnable()
    {
        foreach (GameObject element in endContent)
        {
            element.SetActive(false);
        }
    }

    public void Play()
    {
        foreach (GameObject element in endContent)
        {
            element.SetActive(true);
        }
        soundPlay.Play();
    }

    public void Interact()
    {
        Play();
    }
}
