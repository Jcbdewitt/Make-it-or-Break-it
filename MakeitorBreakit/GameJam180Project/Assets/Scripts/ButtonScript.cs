using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public bool exitButton;

    public int sceneToGoTo = 0;

    public void ButtonPressed()
    {
        if (!exitButton)
        {
            SceneManager.LoadScene(sceneToGoTo);
        } else
        {
            Application.Quit();
        }
    }
}
