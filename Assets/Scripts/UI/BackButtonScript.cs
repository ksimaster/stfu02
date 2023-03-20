using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{

    public void BackButtonGo()
    {

        Application.LoadLevel("Menu");
    }

    public void BackButtonGame()
    {

        Application.LoadLevel("Seiryu Park");
    }

}
