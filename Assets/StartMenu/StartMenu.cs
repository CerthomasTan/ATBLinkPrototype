using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// class that controlls the start scene behavoir
/// </summary>
public class StartMenu : MonoBehaviour
{
    /// <summary>
    /// Changes scene according parameter passed in. 
    /// </summary>
    /// <param name="scene"></param>
    public void changeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
