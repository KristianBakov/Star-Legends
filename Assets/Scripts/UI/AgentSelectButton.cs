using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectButton : Button
{
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;

    public AgentSelectButton(AgentData agentData)
    {
        //add the stylesheet
       // styleSheets.Add(styleSheet);

        //add the button
        AddToClassList("agent-select-button");
        text = agentData.agentName;
        clickable.clicked += () => Debug.Log("Agent Select Button Clicked " + agentData.agentName);
    }
    void Start()
    {
        //add the stylesheet
        //styleSheets.Add(styleSheet);
    }
}

