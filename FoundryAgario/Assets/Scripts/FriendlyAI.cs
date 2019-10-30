using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public float shrinkRate = 1.0f;
    public float shrinkThreshold = 0.1f;
    public bool shrink = false;

    void FixedUpdate()
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
        else
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, Vector3.zero, 0.025f);
        }

        //temp
        if (this.gameObject.transform.position == Vector3.zero) Destroy(this.gameObject);
    }
}
