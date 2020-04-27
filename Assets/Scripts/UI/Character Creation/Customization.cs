using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;

namespace RPG.UI 
{
public class Customization : MonoBehaviour
{

    private GameObject[] characterList;
    private int index = 0;

    private void Start() 
    {

        Debug.Log(Application.persistentDataPath);

        index = PlayerPrefs.GetInt("Character selected "); //saves the model to next scene

        characterList = new GameObject[transform.childCount]; //array size = number of objects 

        //Fill array with models
        for(int i = 0; i < transform.childCount; i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;    //get the gameobjct and not transform 
        }

        //Toggle of their renderer = invisible          **Still in memory
        foreach(GameObject go in characterList) 
            go.SetActive(false);    
        
        //Toggle on the selected character
        if(characterList[index])
            characterList[index].SetActive(true);
    }

    public void ButtonNext()
    {
        //Toggle off the current model
        characterList[index].SetActive(false);

        index++;
        if(index == characterList.Length)
            index = 0;    //start from the other end of the list if end of array is reached   
        
        //Toggle on the new model 
        characterList[index].SetActive(true);
    }

    /*      ------------------------------------------
    set the active avatar to the gameobject reached with buttons
          ------------------------------------------
    */

    public void ButtonPre()
    {
        //Toggle off the current model
        characterList[index].SetActive(false);

        index--;
        if(index < 0)
            index = characterList.Length - 1;    //start from the other end of the list if 0 is passed   
        
        //Toggle on the new model
        characterList[index].SetActive(true);
    }

    //Start the game on another scene
    public void StartButton()
    {

        PlayerPrefs.SetInt("Character selected ", index);
        SceneManager.LoadScene("StarterTown");
        

    }
    


}
}
