using UnityEngine;


public enum AgentType
{
    Duelist,
    Initiator,
    Sentinel,
    Controller
}

[CreateAssetMenu(fileName = "AgentData", menuName = "ScriptableObjects/AgentData", order = 1)]
public class AgentData : ScriptableObject
{
    public string agentName;
    public AgentType agentType;
    public Sprite agentImage;
    public string agentDescription;
}
