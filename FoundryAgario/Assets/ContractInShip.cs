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
    [SerializeField] private Sprite[] WorthSprites;
    [SerializeField] private SpriteRenderer ThisWorthSprite;
    [SerializeField] private SpriteRenderer ThisProgressBar;

    public ContractState State = ContractState.IN_IDLE_SPOT;
    public ContractAssignee Assignee = ContractAssignee.NONE;
    public float ResourceRemaining = 100.0f;
    public float ResourceMax = 100.0f;

    /* Keep the progress bar updated */
    private void FixedUpdate()
    {
        float ResourceProgress = ResourceRemaining / ResourceMax;
        if (ResourceProgress > 1) ResourceProgress = 1;
        ThisProgressBar.transform.localScale = new Vector3(ResourceProgress, 1.0f, 1.0f);
    }
    
    /* Set/get the worth of this contract (the visual sprite) */
    private ContractWorthAmount ContractWorth;
    public ContractWorthAmount GetContractWorth()
    {
        return ContractWorth;
    }
    public void SetContractWorth(ContractWorthAmount worth)
    {
        ContractWorth = worth;
        ThisWorthSprite.sprite = WorthSprites[(int)worth];
    }
}