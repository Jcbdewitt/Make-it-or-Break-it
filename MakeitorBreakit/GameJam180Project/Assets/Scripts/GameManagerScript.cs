using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    public MouseLookScript mouseLook;

    public PlayerMovementScript playerMovementScript;

    public TextMeshProUGUI scoreToUpdate;

    public int finalScore;

    public void GameLost()
    {
        foreach(GameObject g in objectsToDisable)
        {
            g.SetActive(false);
        }

        foreach(GameObject g in objectsToEnable)
        {
            g.SetActive(true);
        }

        playerMovementScript.gameLost = true;
        mouseLook.cameraDisabled = true;
        Cursor.lockState = CursorLockMode.None;

        scoreToUpdate.text = finalScore.ToString();
    }
}
