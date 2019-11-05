using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipCollisionCheck : MonoBehaviour
{
    [SerializeField] private Text DamageCountText;
    [SerializeField] private int DamagePerBulletHit = 1;
    private int ShipHealth = 100;
    private int ShipHealthOrig = 100;

    /* When something collides with the ship, check what it is, and act appropriately. */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //We've been shot - lose health!
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("FUCK I'VE BEEN SHOT!");
            ShipHealth -= DamagePerBulletHit;
            Destroy(collision.gameObject);
        }

        //We've collided with an enemy - activate its rainy day issue and destroy it
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            Debug.Log("SOME CUNT INFECTED MEEEE!");
            ShipResourceManagement.Instance.SetRainyDay(collision.gameObject.GetComponent<EnemyAI>().GetEnemyType());
            Destroy(collision.gameObject);
        }
    }

    /* Update the health percent on update */
    private void Update()
    {
        DamageCountText.text = ((ShipHealth / ShipHealthOrig) * 100).ToString() + "%";
    }
}
