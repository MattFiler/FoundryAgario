using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int health = 100;

    void FixedUpdate()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, Vector3.zero, 0.025f);
    }

    public void reduceHealth(int healthLost)
    {
        health -= healthLost;

        if(health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
