using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    public abstract void reset();
    public abstract int condition_to_occur();
    public abstract void action_by_choice(int index);
}
