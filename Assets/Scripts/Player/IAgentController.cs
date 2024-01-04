using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentController
{
    public abstract void HandleAbilityQ();
    public abstract void HandleAbilityC();
    public abstract void HandleAbilityE();
    public abstract void HandleAbilityX();
}
