using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContractWorthAmount
{
    TEN_K_WORTH,
    TWENTY_K_WORTH,
    FIFTY_K_WORTH,
    SEVENTY_K_WORTH,

    MAX_COUNT //Must always be last, obvs don't use it
}

public class FriendlyAI : MonoBehaviour
{
    [SerializeField] private Sprite[] WorthSprites;
    [SerializeField] private SpriteRenderer ThisWorthSprite;
    
    public float shrinkRate = 1.0f;
    public float shrinkThreshold = 0.5f;
    public bool shrink = false;

    /* Get the value of this contract (the value that is given out in resources) */
    private float ContractValue = 100.0f;
    public float GetContractValue()
    {
        return ContractValue;
    }

    /* Set/get the worth of this contract (the visual sprite, and subsequently the value) */
    private ContractWorthAmount ContractWorth;
    public ContractWorthAmount GetContractWorth()
    {
        return ContractWorth;
    }
    public void SetContractWorth(ContractWorthAmount worth)
    {
        ContractWorth = worth;
        ContractValue = ((int)worth + 1) * 70;
        ThisWorthSprite.sprite = WorthSprites[(int)worth];

        int thisScaleValue = Random.Range(1, ((int)worth+1) * 2);
        transform.localScale = new Vector3(thisScaleValue, thisScaleValue, thisScaleValue);
    }

    /* Get the width of this contract */
    public float GetWidth()
    {
        return gameObject.GetComponent<BoxCollider2D>().bounds.size.x * gameObject.transform.localScale.x;
    }

    /* Our movement starts at our current position */
    private Vector3 nextMoveTo;
    void Start()
    {
        nextMoveTo = this.transform.position;
    }

    /* Handle shrinking and dumb movement on update */
    void FixedUpdate()
    {
        HandleShrink();
        DumbMove();
    }

    /* If the consultation beam hits us, shrink and show outline */
    private void HandleShrink()
    {
        GetComponentInChildren<SpriteRenderer>().color = shrink ? Color.green : Color.white;
        if (shrink)
        {
            Debug.Log("ICH BIN SCHRUMPFEN");
            float scaleNew = transform.localScale.x - (shrinkRate * Time.fixedDeltaTime);
            transform.localScale = new Vector3(scaleNew, scaleNew, scaleNew);
        }
    }

    /* Float around in the environment in a dumb way */
    private void DumbMove()
    {
        if (ContractSpawneer.Instance.PointIsWithinCameraView(this.gameObject.transform.position))
        {
            if (nextMoveTo == null || this.gameObject.transform.position == nextMoveTo)
            {
                nextMoveTo += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, nextMoveTo, 0.008f);
        }
    }
}
