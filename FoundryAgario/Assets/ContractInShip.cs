using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContractState
{
    IN_IDLE_SPOT, //In the centre of the ship
    BEING_DRAGGED, //Being dragged around
    BEING_WORKED_ON, //Being worked on by red/yellow/green/blue
    OUT_OF_JUICE //Was being worked on, now run out of resources
}

public enum ContractAssignee
{
    NONE,
    RED,
    YELLOW,
    GREEN,
    BLUE
}

public class ContractInShip : MonoBehaviour
{
    public ContractState State = ContractState.IN_IDLE_SPOT;
    public ContractAssignee Assignee = ContractAssignee.NONE;
    public float ResourceRemaining = 100.0f;

    /* When being worked on, update remaining resources every frame */
    void FixedUpdate()
    {
        if (State == ContractState.BEING_WORKED_ON)
        {
            ResourceRemaining -= 0.5f;
            if (ResourceRemaining <= 0.0f)
            {
                State = ContractState.OUT_OF_JUICE;
            }
        }
    }
}