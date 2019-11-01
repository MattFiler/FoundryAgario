using UnityEngine;

public class GrabberMovement : MonoBehaviour
{
    public GetMousePos mousePos;

    public GameObject grabberTransformPoint;
    public GameObject grabberExtension;
    public GameObject grabberPoint;
    public GameObject hand1;
    public GameObject hand2;

    public float closingSpeed = 1.0f;
    public float maxClosedAngle = 35.0f;
    public float lengthToExtend = 5.0f;
    public float extendingSpeed = 2.0f;

    public bool isExtending = false;
    public bool hasExtended = false;
    public bool hasStartedOpenClose = false;
    public bool isOpened = true;

    public float scaleAtStart;
    public float grabberPositionAtStart;

    public float help = 0.0f;
    public bool wasMouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        scaleAtStart = grabberExtension.transform.localScale.y;
        grabberPositionAtStart = this.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (mousePos.mouseDown)
        {
            wasMouseDown = true;
        }
        else
        {
            if(wasMouseDown)
            {
                isExtending = true;
                wasMouseDown = false;
            }
            else
            {
                wasMouseDown = false;
            }
        }

        if (isExtending)
        {
            Vector3 translationAddition = new Vector3(0.0f, ((0.25f * extendingSpeed) * Time.deltaTime), 0.0f);
            Vector3 scalingAddition = new Vector3(0.0f, ((5.0f * extendingSpeed) * Time.deltaTime), 0.0f);

            if (!hasExtended)
            {
                if (grabberExtension.transform.localScale.y < scaleAtStart + lengthToExtend)
                {
                    grabberTransformPoint.transform.Translate(translationAddition * 2);
                    grabberExtension.transform.Translate(translationAddition);
                    grabberExtension.transform.localScale = grabberExtension.transform.localScale + scalingAddition;
                    if (grabberExtension.transform.localScale.y >= scaleAtStart + lengthToExtend)
                    {
                        hasExtended = true;
                        isExtending = false;
                        hasStartedOpenClose = true;
                    }
                }
            }
            else
            {
                if (grabberExtension.transform.localScale.y > scaleAtStart )
                {
                    grabberTransformPoint.transform.Translate(-(translationAddition * 2));
                    grabberExtension.transform.Translate(-translationAddition);
                    grabberExtension.transform.localScale = grabberExtension.transform.localScale - scalingAddition;
                    if (grabberExtension.transform.localScale.y <= scaleAtStart )
                    {
                        hasExtended = false;
                        isExtending = false;
                        hasStartedOpenClose = true;
                    }
                }
            }

            //grabberTransformPoint.transform.position = grabberPoint.transform.position;
        }

        if(hasStartedOpenClose)
        {
            if(isOpened)
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
                    isOpened = !isOpened;
                    hasStartedOpenClose = false;
                    isExtending = true;
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
                    isOpened = !isOpened;
                    hasStartedOpenClose = false;
                }
                

            }
        }
    }
}
