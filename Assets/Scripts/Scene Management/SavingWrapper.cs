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

    private void Awake() 
    {
        StartCoroutine(LoadLastScene()); //Calling start after restore to load exp properly
    }
    
    private IEnumerator LoadLastScene() //This was Start()
    {   
        //what was the last scene saved in the save file. Coroutine.
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
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
        //delete key shortcut 
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
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


    //delete savefile 
    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }


    }
}
