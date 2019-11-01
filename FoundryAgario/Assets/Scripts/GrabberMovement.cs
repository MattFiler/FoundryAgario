using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberMovement : MonoBehaviour
{
    public GameObject hand1;
    public GameObject hand2;

    public float closingSpeed = 1.0f;
    public float maxClosedAngle = 35.0f;

    public bool hasStarted = false;
    public bool isClosed = true;

    public float help = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted)
        {
            if(isClosed)
            {
                help = hand1.transform.localRotation.z;

                Vector3 Hand1Rotation = new Vector3();// = hand1.transform.rotation.eulerAngles;
                if(hand1.transform.localRotation.z > -maxClosedAngle*0.01)
                {
                    Hand1Rotation.z -= closingSpeed * Time.deltaTime;
                }
                hand1.transform.Rotate(Hand1Rotation);


                Vector3 Hand2Rotation = new Vector3();// = hand2.transform.rotation.eulerAngles;
                if (hand2.transform.localRotation.z < maxClosedAngle * 0.01)
                {
                    Hand2Rotation.z += closingSpeed * Time.deltaTime;
                }
                hand2.transform.Rotate(Hand2Rotation);

                
                if ((hand1.transform.localRotation.z < -maxClosedAngle * 0.01) || (hand2.transform.localRotation.z > maxClosedAngle * 0.01))
                {
                    isClosed = !isClosed;
                    hasStarted = false;
                }
                
            }
            else
            {
                
                Vector3 Hand1Rotation = new Vector3();// = hand1.transform.rotation.eulerAngles;
                if (hand1.transform.localRotation.z < 0)
                {
                    Hand1Rotation.z += closingSpeed * Time.deltaTime;
                }
                hand1.transform.Rotate(Hand1Rotation);
                

                Vector3 Hand2Rotation = new Vector3();// = hand2.transform.rotation.eulerAngles;
                if (hand2.transform.localRotation.z > 0)
                {
                    Hand2Rotation.z -= closingSpeed * Time.deltaTime;
                }
                hand2.transform.Rotate(Hand2Rotation);

                
                if ((hand1.transform.localRotation.z > 0) || (hand2.transform.localRotation.z < 0))
                {
                    isClosed = !isClosed;
                    hasStarted = false;
                }
                

            }
        }
    }
}
