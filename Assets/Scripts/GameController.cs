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

    public void SetCurrentAgent(Agents _agentIn)
    {
        if (playerController == null) return;
        playerController.currentAgent = _agentIn;
        AddAgentControllerScript(_agentIn);

    }

    private void AddAgentControllerScript(Agents _agentIn)
    {
        switch(_agentIn)
        {
            case Agents.Windweaver:
                playerController.gameObject.AddComponent<WindweaverController>();
                break;
            case Agents.Firebird:
                playerController.gameObject.AddComponent<FirebirdController>();
                break;
            case Agents.Earthshaper:
                playerController.gameObject.AddComponent<EarthshaperController>();
                break;
            case Agents.Orbcaller:
                playerController.gameObject.AddComponent<OrbcallerController>();
                break;
            case Agents.Charmer:
                playerController.gameObject.AddComponent<CharmerController>();
                break;
        }
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
