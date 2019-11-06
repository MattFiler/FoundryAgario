using System.Collections.Generic;
using UnityEngine;

public class ShipResourceManagement : MonoSingleton<ShipResourceManagement>
{
    [SerializeField] private BoxCollider2D RedZone;
    [SerializeField] private BoxCollider2D GreenZone;
    [SerializeField] private BoxCollider2D BlueZone;
    [SerializeField] private BoxCollider2D YellowZone;

    [SerializeField] private Transform ContractSpawnSpot;

    [SerializeField] private Sprite[] RainyDaySprites;
    [SerializeField] private GameObject RainyDayErrorCover;
    [SerializeField] private SpriteRenderer RainyDayTypeSprite;
    [SerializeField] private float TimeForRainyDay = 10.0f;
    private RainyDayType CurrentRainyDay = RainyDayType.MAX_TYPES; //MAX_TYPES acts as NONE because I'm lazy
    private float TimeSinceRainyDay = 0.0f;

    private List<GameObject> ContractsInside = new List<GameObject>();
    private int ActiveContractTouch = -1;
    private int PrevContractTouch = -1;

    private float[] ResourceDepletionRate = new float[(int)ContractAssignee.MAX_COUNT];

    [SerializeField] private GameObject friendlyObject;

    public AudioSource workingAudio;


    /* Tweak these values to change the resource depletion time! */
    private void Start()
    {
        ResourceDepletionRate[(int)ContractAssignee.BLUE] = 0.1f; //Grabby arm
        ResourceDepletionRate[(int)ContractAssignee.RED] = 0.06f; //Booster
        ResourceDepletionRate[(int)ContractAssignee.GREEN] = 0.4f; //Lazer
        ResourceDepletionRate[(int)ContractAssignee.YELLOW] = 0.4f; //Money blastaa
    }
    
    /* Bring the contract inside the ship */
    public void ImportContract(FriendlyAI contract)
    {
        //Update score
        int ThisScore = 0;
        switch (contract.GetContractWorth())
        {
            case ContractWorthAmount.TEN_K_WORTH:
                ThisScore = 10000;
                break;
            case ContractWorthAmount.TWENTY_K_WORTH:
                ThisScore = 20000;
                break;
            case ContractWorthAmount.FIFTY_K_WORTH:
                ThisScore = 50000;
                break;
            case ContractWorthAmount.SEVENTY_K_WORTH:
                ThisScore = 75000;
                break;
        }
        PlayerScore.Instance.Score += ThisScore;

        //Increase ship size
        WorldScaleManager.Instance.AddScale(ThisScore == 0 ? 0 : ThisScore / 10);

        //Work out how many contracts are in centre of ship, and don't spawn if we're at max
        int ContractsInCentre = 0;
        foreach (GameObject InShip in ContractsInside)
        {
            if (InShip.GetComponent<ContractInShip>().Assignee == ContractAssignee.NONE)
            {
                ContractsInCentre++;
            }
        }
        if (ContractsInCentre == 4)
        {
            Debug.LogWarning("Contract can't enter ship - no space!");
            return;
        }

        //We're not at max, spawn in centre
        GameObject OnBoardContract = Instantiate(friendlyObject, ContractSpawnSpot.Find(ContractsInCentre.ToString())) as GameObject;
        OnBoardContract.transform.localScale = new Vector3(1, 1, 1);
        OnBoardContract.GetComponent<ContractInShip>().ResourceRemaining = contract.GetContractValue();
        OnBoardContract.GetComponent<ContractInShip>().ResourceMax = contract.GetContractValue();
        OnBoardContract.GetComponent<ContractInShip>().SetContractWorth(contract.GetContractWorth());
        OnBoardContract.transform.parent = ContractSpawnSpot.transform;
        ContractsInside.Add(OnBoardContract);
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
        ThisZone.ResourceCount -= ResourceDepletionRate[(int)ResourceType];
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

    /* Set the current rainy day issue */
    public void SetRainyDay(RainyDayType issue)
    {
        CurrentRainyDay = issue;
        RainyDayTypeSprite.gameObject.SetActive(true);
        RainyDayTypeSprite.sprite = RainyDaySprites[(int)issue];
        TimeSinceRainyDay = 0.0f;
        RainyDayErrorCover.SetActive(true);
        RainyDayErrorCover.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    /* Update the rainy day progress bar, and disable rainy day status if zero */
    void FixedUpdate()
    {
        TimeSinceRainyDay += Time.deltaTime;
        if (TimeSinceRainyDay >= TimeForRainyDay)
        {
            CurrentRainyDay = RainyDayType.MAX_TYPES;
            RainyDayTypeSprite.gameObject.SetActive(false);
            RainyDayErrorCover.SetActive(false);
            return;
        }

        float RainyDayProgress = TimeSinceRainyDay / TimeForRainyDay;
        if (RainyDayProgress > 1) RainyDayProgress = 1;
        if (RainyDayProgress < 0) RainyDayProgress = 0;
        RainyDayProgress = (RainyDayProgress - 1) * -1;
        RainyDayErrorCover.transform.localScale = new Vector3(1.0f, RainyDayProgress, 1.0f);
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

    /* Distribute resources from contracts to zones if they're low */
    private void DistributeResources()
    {
        if (CurrentRainyDay != RainyDayType.MAX_TYPES) return; //Only allow contract consumption when no rainy day issues are active

        bool contractBeingWorked = false;
        foreach (GameObject Contract in ContractsInside)
        {
            ContractInShip ContractMeta = Contract.GetComponent<ContractInShip>();
            if (ContractMeta.State == ContractState.BEING_WORKED_ON)
            {
                contractBeingWorked = true;
                if(!workingAudio.isPlaying) workingAudio.Play();
                
                ZoneInShip ThisZone = null;
                switch (ContractMeta.Assignee)
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
                if (ThisZone == null) continue;
                if (ThisZone.ResourceCount >= ThisZone.ResourceMax) continue;
                ThisZone.ResourceCount += ResourceDepletionRate[(int)ContractMeta.Assignee];
                ContractMeta.ResourceRemaining -= ResourceDepletionRate[(int)ContractMeta.Assignee];
                if (ContractMeta.ResourceRemaining <= 0.0f)
                {
                    ContractMeta.State = ContractState.OUT_OF_JUICE;
                }
            }
        }

        if(!contractBeingWorked) workingAudio.Stop();
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

            ContractsInside[PrevContractTouch].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            ContractsInside[PrevContractTouch].transform.Find("LIGHT").gameObject.SetActive(false);

            return;
        }
        if (ActiveContractTouch == -1) return;

        //Still interacting with contract
        ContractsInside[ActiveContractTouch].transform.position = UserTouch.position;
        ContractsInside[ActiveContractTouch].GetComponent<ContractInShip>().Assignee = ContractAssignee.NONE;
        ContractsInside[ActiveContractTouch].GetComponent<ContractInShip>().State = ContractState.BEING_DRAGGED;
        ContractsInside[ActiveContractTouch].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        ContractsInside[ActiveContractTouch].transform.Find("LIGHT").gameObject.SetActive(true);
    }
}
