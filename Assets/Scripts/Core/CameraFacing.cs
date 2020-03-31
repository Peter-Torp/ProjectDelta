using UnityEngine;

namespace RPG.Core 
{
    
public class CameraFacing : MonoBehaviour 
{

        void Update() 
        {
            //point the canvas/text toward the maincamera 
            transform.forward = Camera.main.transform.forward;
        }



}



}