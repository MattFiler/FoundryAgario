using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    public int damage = 1;
    ParticleSystem coins;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    void Start()
    {
        coins = GetComponent<ParticleSystem>(); 
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            int numCollisionEvents = coins.GetCollisionEvents(other, collisionEvents);

            for(int i = 0; i < numCollisionEvents; ++i)
            {
                other.GetComponent<EnemyAI>().reduceHealth(damage);
            }

        }
    }
}
