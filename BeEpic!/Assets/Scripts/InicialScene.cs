using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InicialScene : MonoBehaviour {

    private void Start()
    {
        SceneManager.LoadScene("Main_Scene", LoadSceneMode.Single);
    }

}
