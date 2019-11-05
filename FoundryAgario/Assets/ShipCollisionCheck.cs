using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionCheck : MonoBehaviour
{
    /* When something collides with the ship, check what it is, and act appropriately. */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //We've been shot - lose health!
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("FUCK I'VE BEEN SHOT!");
        }

        //We've collided with an enemy - activate its rainy day issue and destroy it
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            ShipResourceManagement.Instance.SetRainyDay(collision.gameObject.GetComponent<EnemyAI>().GetEnemyType());
            Destroy(collision.gameObject);
        }
    }
}
