using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RainyDayType
{
    CODING_ERROR,
    ILLNESS,
    POWER_OUTTAGE,

    MAX_TYPES //Must be last, don't use obvs
}

public class EnemyAI : MonoBehaviour
{
    public int health = 100; //temp

    [SerializeField] private GameObject PoofFX;

    [SerializeField] private Sprite[] RainyDaySprites;
    //[SerializeField] private Sprite[] BulletSprites;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private SpriteRenderer ThisSprite;

    [SerializeField] private float shootRange = 2;
    [SerializeField] private float shootCooldown = 1;
    [SerializeField] private float bulletSpeed = 1;
    private float timeSinceShoot = 100;

    //public AudioSource deathClip;

    /* Get/set the rainy day (enemy) type */
    private RainyDayType thisType;
    public RainyDayType GetEnemyType()
    {
        return thisType;
    }
    public void SetEnemyType(RainyDayType type)
    {
        thisType = type;
        ThisSprite.sprite = RainyDaySprites[(int)type];
    }

    /* When we're close to the player, start moving towards them and shoot when in range */
    void FixedUpdate()
    {
        if (ContractSpawneer.Instance.PointIsWithinCameraView(this.gameObject.transform.position, 20 * (WorldScaleManager.Instance.difficultyScale)))
        {
            if(Vector2.Distance(gameObject.transform.position, ShipMovement.Instance.GetPosition()) <= shootRange * WorldScaleManager.Instance.difficultyScale) Shoot();
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, ShipMovement.Instance.GetPosition(), 0.025f * WorldScaleManager.Instance.difficultyScale);
            //if (this.gameObject.transform.position == ShipMovement.Instance.GetPosition()) Destroy(this.gameObject);
        }
    }

    /* Shoot a bullet at the player */
    private void Shoot()
    {
        if(timeSinceShoot >= shootCooldown)
        {
            GameObject newBullet = Instantiate(Bullet);
            newBullet.transform.position = transform.position;
            //Bullet.GetComponent<SpriteRenderer>().sprite = BulletSprites[(int)thisType];
            Vector3 moveVector = ShipMovement.Instance.GetPosition() - newBullet.transform.position;
            moveVector.Normalize();
            moveVector *= bulletSpeed * (WorldScaleManager.Instance.difficultyScale*2);
            var angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;

            newBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            newBullet.GetComponent<Rigidbody2D>().velocity = moveVector;
            timeSinceShoot = 0;
        }
        else
        {
            timeSinceShoot += Time.deltaTime;
        }
    }

    /* When hit with a lazer, reduce our health */
    public void reduceHealth(int healthLost)
    {
        GetComponent<Animator>().SetTrigger("damaged");

        health -= healthLost;
        Debug.Log("THANKS FOR THAT. RUINED MY HOLIDAY.");
        if(health <= 0)
        {
            Debug.Log("OH NO HE'S DEED");

            //if(!deathClip.isPlaying)
            //{
            //   deathClip.PlayOneShot(deathClip.clip);
            //}

            Instantiate(PoofFX, this.gameObject.transform.position, Quaternion.identity);
            GameObject.Destroy(this.gameObject);
        }
    }
}
