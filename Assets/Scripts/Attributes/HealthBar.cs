using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
{
    [SerializeField] Health healthComponent = null; 
    [SerializeField] RectTransform foreground = null; 
    [SerializeField] Canvas rootCanvas = null;

  
    void Update()
    {   
        //floating points may vary from different CPU. We then use Mathf approximatly method.
        if(Mathf.Approximately(healthComponent.GetFraction(), 0) || Mathf.Approximately(healthComponent.GetFraction(), 1))  //disable at full and no health
        {
            rootCanvas.enabled = false; 
            return; 
        }
        rootCanvas.enabled = true;

        //update the healthbar by checking the health of enemy. 
        foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1); // x,y,z of health bar
    }
}


}
