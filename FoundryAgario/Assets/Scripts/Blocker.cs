using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    public AudioSource blockerClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Shield shot - destroy bullet
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            ps.Play();

            if(!blockerClip.isPlaying)
            {
                blockerClip.PlayOneShot(blockerClip.clip);
            }
        }

        //Collided with enemy - fuck em up!
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            Debug.Log("LARGE OOF");
            collision.gameObject.GetComponent<EnemyAI>().reduceHealth(1000);
        }
    }
}
