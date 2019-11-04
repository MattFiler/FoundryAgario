using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public float shrinkRate = 1.0f;
    public float shrinkThreshold = 0.1f;
    public bool shrink = false;

    /* Set/get the worth of this contract */
    private float ContractWorth = 100.0f;
    public float GetContractValue()
    {
        return ContractWorth;
    }
    public void SetContractValue(float worth)
    {
        ContractWorth = worth;
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
        transform.Find("outlline").gameObject.SetActive(shrink);
        if (shrink)
        {
            transform.localScale = new Vector3(transform.localScale.x - (shrinkRate * Time.fixedDeltaTime), transform.localScale.y - (shrinkRate * Time.fixedDeltaTime)
                , transform.localScale.z - (shrinkRate * Time.fixedDeltaTime));

            if (transform.localScale.x < shrinkThreshold || transform.localScale.y < shrinkThreshold || transform.localScale.z < shrinkThreshold)
            {
                GameObject.Destroy(this.gameObject);
            }
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
