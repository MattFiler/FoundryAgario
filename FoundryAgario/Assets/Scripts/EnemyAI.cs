using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int health = 100;

    /* When we're close to the player, start moving towards them */
    void FixedUpdate()
    {
        if (ContractSpawneer.Instance.PointIsWithinCameraView(this.gameObject.transform.position))
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, ShipMovement.Instance.GetPosition(), 0.025f);
            if (this.gameObject.transform.position == ShipMovement.Instance.GetPosition()) Destroy(this.gameObject);
        }
    }

    /* When hit with a lazer, reduce our health */
    public void reduceHealth(int healthLost)
    {
        health -= healthLost;
        Debug.Log("OW");
        if(health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
