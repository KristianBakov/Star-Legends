using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public bool inMenu = true;
    public UIDocument agentSelectScreen;

    private void Start()
    {
        agentSelectScreen = GetComponent<AgentSelectScreen>().document;
        GoToAgentSelectScreen();
    }

    void SetPanelEnableState(UIDocument panel, bool finalState)
    {
        if(finalState)
        {
            panel.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            panel.rootVisualElement.style.display = DisplayStyle.None;
        }

        panel.enabled = finalState;
    }
    public void GoToAgentSelectScreen()
    {
        SetPanelEnableState(agentSelectScreen, true);
    }

    public void HideUI()
    {

    }


}
