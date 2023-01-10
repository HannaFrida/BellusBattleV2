using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
* Author Khaled Alraas
*/
public class OpenPlatformer : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Platformer");
            GameManager.Instance.SetIsinMiniGame(true);
        }
    }
}
