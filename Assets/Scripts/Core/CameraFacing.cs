using UnityEngine;

namespace RPG.Core 
{
    
public class CameraFacing : MonoBehaviour 
{

        //called after every update call
        void LateUpdate() 
        {
            //point the canvas/text toward the maincamera 
            transform.forward = Camera.main.transform.forward;
        }



}



}