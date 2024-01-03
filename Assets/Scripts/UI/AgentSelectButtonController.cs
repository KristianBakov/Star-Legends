using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectButtonController
{
    private List<Button> agentSelectButtons;
    public ListView agentSelectListView;
    private Button currentlySelectedButton;

    public void InitializeCharacterList()
    {
        QueryListItems();
    }
    private void QueryListItems()
    {
        GenerateAgentButtons();
    }

    private void GenerateAgentButtons()
    {
        if (AgentDataTable.agentDataList.Count == 0)
        {
            Debug.LogError("No agents found in AgentDataTable");
            return;
        }
        agentSelectButtons = new List<Button>();

        foreach (var agentData in AgentDataTable.agentDataList)
        {
            agentSelectButtons.Add(new Button());
        }

        Func<VisualElement> makeItem = () => new Button();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Button).text = AgentDataTable.agentDataList[i].agentName;
            (e as Button).clicked += () => OnAgentSelectButtonClick(e as Button);
            (e as Button).AddToClassList("agent-select-button");
        };

        agentSelectListView = new ListView(agentSelectButtons, 16, makeItem, bindItem);
        agentSelectListView.AddToClassList("agent-select-list-view");
        agentSelectListView.selectionType = SelectionType.Single;
    }

    private void OnAgentSelectButtonClick(Button agentSelectButton)
    {
        Debug.Log("Agent Select Button Clicked " + agentSelectButton.text);
        UIController.Instance.HideUI();
    }

    public void SetButtonText()
    {
        agentSelectButtons.ForEach(button => button.text = AgentDataTable.agentDataList[agentSelectButtons.IndexOf(button)].agentName);
    }
}
