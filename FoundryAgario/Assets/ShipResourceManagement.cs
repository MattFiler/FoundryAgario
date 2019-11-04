using System.Collections.Generic;
using UnityEngine;

public class ShipResourceManagement : MonoSingleton<ShipResourceManagement>
{
    [SerializeField] private BoxCollider2D RedZone;
    [SerializeField] private BoxCollider2D GreenZone;
    [SerializeField] private BoxCollider2D BlueZone;
    [SerializeField] private BoxCollider2D YellowZone;

    [SerializeField] private Transform ContractSpawnSpot;

    private List<GameObject> ContractsInside = new List<GameObject>();
    private int ActiveContractTouch = -1;
    private int PrevContractTouch = -1;
    private int ContractsInCentre = 0;

    public float ResourceDepletionRate = 0.5f; //Should be const

    [SerializeField] private GameObject friendlyObject;

    /* DEBUG ONLY UNTIL GRABBER FUCKING WORKS */
    void Awake()
    {
        FriendlyAI demo = new FriendlyAI();
        demo.SetContractValue(100.0f);
        ImportContract(demo);
    }
    
    /* Bring the contract inside the ship */
    public void ImportContract(FriendlyAI contract)
    {
        if (ContractsInCentre == 4)
        {
            Debug.LogWarning("Contract can't enter ship - no space!");
            return;
        }
        GameObject OnBoardContract = Instantiate(friendlyObject, ContractSpawnSpot.Find(ContractsInCentre.ToString())) as GameObject;
        OnBoardContract.transform.localScale = new Vector3(1, 1, 1);
        OnBoardContract.GetComponent<ContractInShip>().ResourceRemaining = contract.GetContractValue();
        ContractsInside.Add(OnBoardContract);
        ContractsInCentre++;
    }

    /* Use a specified resource (returns false if used up) */
    public bool UseResource(ContractAssignee ResourceType)
    {
        ZoneInShip ThisZone = null;
        switch (ResourceType)
        {
            case ContractAssignee.BLUE:
                ThisZone = BlueZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.GREEN:
                ThisZone = GreenZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.RED:
                ThisZone = RedZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.YELLOW:
                ThisZone = YellowZone.gameObject.GetComponent<ZoneInShip>();
                break;
        }
        if (ThisZone == null) return false;
        ThisZone.ResourceCount -= ResourceDepletionRate;
        return (ThisZone.ResourceCount > 0.0f);
    }

    /* Check to see if a resource is empty */
    public bool ResourceIsEmpty(ContractAssignee ResourceType)
    {
        ZoneInShip ThisZone = null;
        switch (ResourceType)
        {
            case ContractAssignee.BLUE:
                ThisZone = BlueZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.GREEN:
                ThisZone = GreenZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.RED:
                ThisZone = RedZone.gameObject.GetComponent<ZoneInShip>();
                break;
            case ContractAssignee.YELLOW:
                ThisZone = YellowZone.gameObject.GetComponent<ZoneInShip>();
                break;
        }
        return (ThisZone == null || ThisZone.ResourceCount <= 0.0f);
    }
     
    /* Handle update logic */
    void Update()
    {
        ContractInteraction();
        DistributeResources();
        ClearDeJuicedContracts();
    }

    /* Check all contracts and remove ones that are out of juice */
    private void ClearDeJuicedContracts()
    {
        List<GameObject> ContractsWithJuice = new List<GameObject>();
        foreach (GameObject Contract in ContractsInside)
        {
            if (Contract.GetComponent<ContractInShip>().State != ContractState.OUT_OF_JUICE)
                ContractsWithJuice.Add(Contract);
            else
                Destroy(Contract);
        }
        ContractsInside = ContractsWithJuice;
    }

    /* Distribute resources from contracts to zones */
    private void DistributeResources()
    {
        foreach (GameObject Contract in ContractsInside)
        {
            ContractInShip ContractMeta = Contract.GetComponent<ContractInShip>();
            if (ContractMeta.State == ContractState.BEING_WORKED_ON)
            {
                ContractMeta.ResourceRemaining -= ResourceDepletionRate;
                switch (ContractMeta.Assignee)
                {
                    case ContractAssignee.BLUE:
                        BlueZone.gameObject.GetComponent<ZoneInShip>().ResourceCount += ResourceDepletionRate;
                        break;
                    case ContractAssignee.GREEN:
                        GreenZone.gameObject.GetComponent<ZoneInShip>().ResourceCount += ResourceDepletionRate;
                        break;
                    case ContractAssignee.RED:
                        RedZone.gameObject.GetComponent<ZoneInShip>().ResourceCount += ResourceDepletionRate;
                        break;
                    case ContractAssignee.YELLOW:
                        YellowZone.gameObject.GetComponent<ZoneInShip>().ResourceCount += ResourceDepletionRate;
                        break;
                }
                if (ContractMeta.ResourceRemaining <= 0.0f)
                {
                    ContractMeta.State = ContractState.OUT_OF_JUICE;
                }
            }
        }
    }

    /* Handle dragging of contracts */
    private void ContractInteraction()
    {
        PrevContractTouch = ActiveContractTouch;

        //Work out what contract we're interacting with
        Touch UserTouch = new Touch();
        int ContractIndex = 0;
        ActiveContractTouch = -1;
        foreach (GameObject Contract in ContractsInside)
        {
            if (TouchManager.instance.GetTouch(Contract, ref UserTouch))
            {
                ActiveContractTouch = ContractIndex;
                break;
            }
            ContractIndex++;
        }

        //Just released contract
        if (ActiveContractTouch == -1 && PrevContractTouch != -1)
        {
            Vector3 ContractPosition = ContractsInside[PrevContractTouch].transform.position;
            ContractInShip ContractMeta = ContractsInside[PrevContractTouch].GetComponent<ContractInShip>();
            if (RedZone.bounds.Contains(ContractPosition))
                ContractMeta.Assignee = ContractAssignee.RED;
            else if (GreenZone.bounds.Contains(ContractPosition))
                ContractMeta.Assignee = ContractAssignee.GREEN;
            else if (BlueZone.bounds.Contains(ContractPosition))
                ContractMeta.Assignee = ContractAssignee.BLUE;
            else if (YellowZone.bounds.Contains(ContractPosition))
                ContractMeta.Assignee = ContractAssignee.YELLOW;
            else
                ContractMeta.Assignee = ContractAssignee.NONE;

            if (ContractMeta.Assignee == ContractAssignee.NONE)
                ContractMeta.State = ContractState.IN_IDLE_SPOT;
            else
                ContractMeta.State = ContractState.BEING_WORKED_ON;

            return;
        }
        if (ActiveContractTouch == -1) return;

        //Still interacting with contract
        ContractsInside[ActiveContractTouch].transform.position = UserTouch.position;
        ContractsInside[ActiveContractTouch].GetComponent<ContractInShip>().Assignee = ContractAssignee.NONE;
        ContractsInside[ActiveContractTouch].GetComponent<ContractInShip>().State = ContractState.BEING_DRAGGED;
    }
}
