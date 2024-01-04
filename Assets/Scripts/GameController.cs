using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public CursorLockMode currentCursorLockMode;
    public PlayerStats playerStats;
    public PlayerController playerController;

    private void Start()
    {
        //get the player controller
        playerController = FindObjectOfType<PlayerController>();
    }

    public void SetCurrentAgent()
    {

    }

    public void HideCursor()
    {
        currentCursorLockMode = CursorLockMode.Locked;
        Cursor.lockState = currentCursorLockMode;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        currentCursorLockMode = CursorLockMode.None;
        Cursor.lockState = currentCursorLockMode;
        Cursor.visible = true;
    }

    public void ToggleCursor()
    {
        if (currentCursorLockMode == CursorLockMode.None)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }
    }
}
