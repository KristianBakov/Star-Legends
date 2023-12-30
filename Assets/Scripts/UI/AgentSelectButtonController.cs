using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectButtonController : MonoBehaviour
{
    private List<Button> agentSelectButtons;
    private List<AgentData> agentDataList;
    public ListView agentSelectListView;
    private Button currentlySelectedButton;

    public void InitializeCharacterList()
    {
        EnumerateAllAgents();
        QueryListItems();
    }
    private void EnumerateAllAgents()
    {
        agentDataList = new List<AgentData>();
        agentDataList.AddRange(Resources.LoadAll<AgentData>("Agents"));
    }
    private void QueryListItems()
    {
        GenerateAgentButtons();
    }

    private void GenerateAgentButtons()
    {
        agentSelectButtons = new List<Button>();

        foreach (var agentData in agentDataList)
        {
            agentSelectButtons.Add(new Button());
        }

        Func<VisualElement> makeItem = () => new Button();
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Button).text = agentDataList[i].agentName;
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
    }

    public void SetButtonText()
    {
        agentSelectButtons.ForEach(button => button.text = agentDataList[agentSelectButtons.IndexOf(button)].agentName);
    }
}
