using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    public AudioSource blockerClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(collision.gameObject);
            ps.Play();

            if(!blockerClip.isPlaying)
            {
                blockerClip.PlayOneShot(blockerClip.clip);
            }
        }
    }
}
