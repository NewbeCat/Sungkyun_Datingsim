using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TestCondition", menuName = "Condition/TestCondition")]
public class TestCondition : ConditionBase
{
    public bool testVar = false;
    private bool firstbtns = false;
    private bool secondbtns = false;

    [SerializeField] private bool first;
    [SerializeField] private bool second;

    public override void reset()
    {
        firstbtns = first;
        secondbtns = second;
    }

    public override int condition_to_occur()
    {
        if (testVar == true) { return 1; }
        return 0;
    }

    public override void action_by_choice(int index)
    {
        if (index == 0) { firstbtns = true; }
        else if (index == 1) { secondbtns = true; }

        if (firstbtns == true && secondbtns == true)
        {
            DialogueManager.Instance.changeBranch();
        }
        else
        {
            DialogueManager.Instance.TurnOnOffBtns(false);
        }
    }
}