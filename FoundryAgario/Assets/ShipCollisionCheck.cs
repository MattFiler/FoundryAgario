using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipCollisionCheck : MonoBehaviour
{
    [SerializeField] private GameObject PoofFX;
    [SerializeField] private Text DamageCountText;
    [SerializeField] private int DamagePerBulletHit = 2;
    [SerializeField] private int DamagePerEnemyImpact = 6;
    private int ShipHealth = 1;
    private int ShipHealthOrig = 100;
    private bool GameEnded = false;

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
            ShipHealth -= DamagePerEnemyImpact;
            collision.gameObject.GetComponent<EnemyAI>().reduceHealth(1000);
        }
    }

    /* Update the health percent on update, and check for loss states */
    private void Update()
    {
        DamageCountText.text = (((float)ShipHealth / (float)ShipHealthOrig) * 100).ToString() + "%";

        //If out of health or resources, we lost
        bool allResourcesEmpty = ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.BLUE) &&
                                 ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.YELLOW) &&
                                 ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.RED) &&
                                 ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.GREEN);
        if ((ShipHealth <= 0 || allResourcesEmpty) && !GameEnded)
        {
            ShowLoss.Instance.ShouldShowLoss = true;
            GameEnded = true;
        }
    }
}
