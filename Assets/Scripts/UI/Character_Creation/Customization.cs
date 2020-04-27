using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI 
{
public class Customization : MonoBehaviour
{

    //add more if needed
    enum AppearanceDetail
    {
        CharacterAvatar,

    }

    [SerializeField] private GameObject[] avatars = null;
    //[SerializeField] private Transform anchor = null;
    GameObject activeAvatar;
    private Transform playerTransform = null;
    private GameObject player = null;

    private void Start() 
    {
        this.playerTransform = GameObject.FindWithTag("Player").transform;
        Debug.Log("Player transform is: " + playerTransform);

        this.player = GameObject.FindWithTag("Player");
        Debug.Log("Player object is: " + playerTransform);

        var avatarComponent = player.GetComponent<Animator>().avatar; 
        Debug.Log("Got the animator component!");
    }


    //button left and right for ++ and -- appearance
    void ButtonRight()
    {
        Debug.Log("Button ´Right´ is clicked ");
    }

    /*      ------------------------------------------
    set the active avatar to the gameobject reached with buttons
          ------------------------------------------
    */
    
    void ButtonLeft()
    {
        Debug.Log("Button ´Left´ is clicked ");
    }


    void ApplyModification(AppearanceDetail detail, int id)
    {
        switch(detail)
        {
            case AppearanceDetail.CharacterAvatar: 
            if(activeAvatar != null) GameObject.Destroy(activeAvatar);

                break;
        }

    }


}
}
