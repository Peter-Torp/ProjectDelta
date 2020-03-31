using UnityEngine; 

namespace RPG.Core
{

    public class CameraFacing : Monobehavior 
    {

        private void Update() 
        {
            //paint the canvas toward the maincamera 
            transfrom.forward = Camera.main.transfrom.forward;
        }



    }


}