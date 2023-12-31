using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDataTable : MonoSingleton<AgentDataTable>
{ 
    public static List<AgentData> agentDataList = new List<AgentData>();

    private void OnValidate()
    {
        if(agentDataList.Count == 0)
        EnumerateAllAgents();
    }

    public static void EnumerateAllAgents()
    {
        agentDataList.AddRange(Resources.LoadAll<AgentData>("Agents"));
    }
}
