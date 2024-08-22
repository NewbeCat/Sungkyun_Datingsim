using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TestCondition", menuName = "Condition/TestCondition")]
public class TestCondition : ConditionBase
{
    public bool testVar = false;
    public override int condition_to_occur()
    {
        if (testVar == true) { return 1; }
        return 0;
    }

    public override void action_by_choice(int index)
    {
        if (index == 0)
        {
            Debug.Log("react correctly for 0");
        }

        if (index == 1)
        {
            Debug.Log("react correctly for 1");
        }


    }
}
