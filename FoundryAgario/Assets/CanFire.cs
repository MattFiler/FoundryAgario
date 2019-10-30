using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFire : MonoBehaviour
{
    public bool canFire = true;

    public enum TurretType { COIN, LASER, CLAW};
    public TurretType turretType;

    private void Update()
    {
        GameObject gunPivot = null;

        foreach(Transform child in transform)
        {
            if(child.CompareTag("Turret Pivot"))
            {
                gunPivot = child.gameObject;
            }
            else if(child.CompareTag("Turret Outline"))
            {
                child.GetComponent<SpriteRenderer>().enabled = canFire;
            }
        }

        if (gunPivot != null)
        {
            gunPivot.GetComponent<RotateGun>().enabled = canFire;

            switch (turretType)
            {
                case TurretType.COIN:
                    gunPivot.GetComponentInChildren<FireCoins>().enabled = canFire;
                    break;
                case TurretType.LASER:
                    gunPivot.GetComponentInChildren<FireLazer>().enabled = canFire;
                    break;
                case TurretType.CLAW:
                    //gunPivot.GetComponentInChildren<FireLazer>().enabled = canFire;
                    break;
                default:
                    break;
            }
        }
    }
}
