using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public CursorLockMode currentCursorLockMode;
    public PlayerStats playerStats;


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
}
