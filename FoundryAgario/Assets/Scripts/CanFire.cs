using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFire : MonoBehaviour
{
    public bool canFire = true;

    public enum TurretType { COIN, LASER, CLAW, THRUSTER};
    public TurretType turretType;

    private void Update()
    {
        GameObject turretPivot = null;

        foreach(Transform child in transform)
        {
            if(child.CompareTag("Turret Pivot"))
            {
                turretPivot = child.gameObject;
            }
            else if(child.CompareTag("Turret Outline"))
            {
                child.GetComponent<SpriteRenderer>().enabled = canFire;
            }
        }

        if (turretPivot != null)
        {
            turretPivot.GetComponent<RotateGun>().canFire = canFire;

            switch (turretType)
            {
                case TurretType.COIN:
                    turretPivot.GetComponentInChildren<FireCoins>().enabled = canFire;
                    break;
                case TurretType.LASER:
                    turretPivot.GetComponentInChildren<FireLazer>().enabled = canFire;
                    break;
                case TurretType.CLAW:
                    //gunPivot.GetComponentInChildren<FireLazer>().enabled = canFire;
                    break;
                //case TurretType.THRUSTER:
                //    turretPivot.GetComponentInChildren<FireCoins>().enabled = canFire;
                //    break;
                default:
                    break;
            }
        }
    }
}
