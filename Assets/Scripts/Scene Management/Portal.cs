using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{

    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            print("portal entered");

            if (other.tag == "Player")
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
