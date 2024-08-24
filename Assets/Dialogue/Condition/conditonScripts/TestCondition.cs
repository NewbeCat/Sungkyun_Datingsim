using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TestCondition", menuName = "Condition/TestCondition")]
public class TestCondition : ConditionBase
{
    public bool testVar = false;

    [SerializeField] private bool first;
    [SerializeField] private bool second;

    public override void reset()
    {
    }

    public override int condition_to_occur()
    {
        if (testVar == true) { return 1; }
        return 0;
    }

    public override void action_by_choice(int index)
    {
        Debug.Log("You clicked Button " + index);
    }
}