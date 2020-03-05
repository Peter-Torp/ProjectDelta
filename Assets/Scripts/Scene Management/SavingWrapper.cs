using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.SceneManagement
{
public class SavingWrapper : MonoBehaviour
{

    const string defaultSaveFile = "Save";
    [SerializeField] float fadeInTime = 0.2f;   //load fading time

    IEnumerator Start() 
    {   
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        //what was the last scene saved in the save file. Coroutine.
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);  
        yield return fader.FadeIn(fadeInTime);   
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            Save();  
        }
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load()
    {
        //call to saving system load    
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }




    }
}
