using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoSingleton<ShipMovement>
{
    public GetMousePos mousePos;
    public float thrust = 100;
    public ParticleSystem mainParticle;
    public GameObject thruster;
    public bool isPlaying = false;

    // Update is called once per frame
    void Update()
    {
        if (ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.RED)) return;

        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        if (mousePos.mouseDown)
        {
            rb.AddForce(-thruster.transform.up * thrust);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * thrust);

            //rb.velocity = new Vector2(rb.velocity.x + 0.0f, rb.velocity.y + 5.0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce((-transform.up) * thrust);

            //rb.velocity = new Vector2(rb.velocity.x + 0.0f, rb.velocity.y - 5.0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce((-transform.right) * thrust);

            //rb.velocity = new Vector2(rb.velocity.x -5.0f, rb.velocity.y + 0.0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(transform.right * thrust);

            //rb.velocity = new Vector2(rb.velocity.x + 5.0f, rb.velocity.y + 0.0f);
        }

        if(mainParticle != null)
        {
            if (rb.velocity.x > 0.05 || rb.velocity.x < -0.05 || rb.velocity.y < -0.05 || rb.velocity.y > 0.05)
            {
                mainParticle.transform.position = this.transform.position;
                if (!mainParticle.isPlaying)
                {
                    mainParticle.Play();
                    isPlaying = true;
                }
                //mainParticle.transform.rotation.eulerAngles = mainParticle.transform.rotation.eulerAngles + new Vector3(0, 0, -15);
            }
            else
            {
                if (mainParticle.isPlaying)
                {
                    mainParticle.Stop();
                    isPlaying = false;
                }
            }
        }
    }

    /* Get the position of the ship in world space */
    public Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }
}
