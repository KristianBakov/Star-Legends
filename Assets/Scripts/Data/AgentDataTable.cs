using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Agents
{
    Windweaver,
    Firebird,
    Earthshaper,
    Orbcaller,
    Charmer,
}

public class AgentDataTable : MonoSingleton<AgentDataTable>
{ 
    public static List<AgentData> agentDataList = new List<AgentData>();
    public Dictionary<Agents, AgentData> agentDataDictionary = new Dictionary<Agents, AgentData>();
    public static Dictionary<Agents, string> agentNameDictionary = new Dictionary<Agents, string>()
    {
        {Agents.Windweaver, "Windweaver"},
        {Agents.Firebird, "Firebird"},
        {Agents.Earthshaper, "Earthshaper"},
        {Agents.Orbcaller, "Orbcaller"},
        {Agents.Charmer, "Charmer"},
    };
    [SerializeField] public List<IAgentController> agentControllers;
    private void OnValidate()
    {
        if(agentDataList.Count == 0)
        EnumerateAllAgents();
    }

    public static void EnumerateAllAgents()
    {
        agentDataList.AddRange(Resources.LoadAll<AgentData>("Agents"));
        Instance.agentDataDictionary.Clear();
        foreach (AgentData agentData in agentDataList)
        {
            Instance.agentDataDictionary.Add(agentNameDictionary.FirstOrDefault(x => x.Value == agentData.name).Key, agentData);
        }


    }
}
