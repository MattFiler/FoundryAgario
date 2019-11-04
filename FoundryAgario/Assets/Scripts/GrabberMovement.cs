using UnityEngine;

public class GrabberMovement : MonoBehaviour
{
    public GetMousePos mousePos;

    public GameObject lineRender;

    public GameObject gunpivot;
    public GameObject grabberWallUp;

    public GameObject grabberTransformPoint;
    public GameObject grabberExtension;
    public GameObject grabberPoint;
    public GameObject hand1;
    public GameObject hand2;

    public float closingSpeed = 1.0f;
    public float maxClosedAngle = 35.0f;

    public float lengthToExtend = 5.0f;
    public float extendingSpeed = 2.0f;

    [HideInInspector]
    public bool isExtending = false;
    [HideInInspector]
    public bool hasExtended = false;
    [HideInInspector]
    public bool hasStartedOpenClose = false;
    [HideInInspector]
    public bool isOpened = true;

    [HideInInspector]
    public bool canMove = true;

    [HideInInspector]
    private float scaleAtStart;
    [HideInInspector]
    private float grabberPositionAtStart;

    [HideInInspector]
    private bool wasMouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        scaleAtStart = grabberExtension.transform.localScale.y;
        grabberPositionAtStart = this.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShipResourceManagement.Instance.ResourceIsEmpty(ContractAssignee.BLUE)) return;

        LineRenderer mainLine = lineRender.GetComponent<LineRenderer>();
        //public GameObject gunpivot;
        //public GameObject grabberWallUp;
        mainLine.useWorldSpace = true;

        mainLine.SetWidth(0.2f, 0.2f);
        mainLine.SetPosition(0, gunpivot.transform.position);
        mainLine.SetPosition(1, grabberWallUp.transform.position);

        if (mousePos.mouseDown)
        {
            wasMouseDown = true;
            ShipResourceManagement.Instance.UseResource(ContractAssignee.BLUE); //todo: decrease properly (e.g. X on activation)
        }
        else
        {
            if(wasMouseDown)
            {
                if(canMove)
                {
                    isExtending = true;
                    wasMouseDown = false;
                    canMove = false;
                }
                else
                {
                    wasMouseDown = false;
                }
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
                    canMove = true;
                }


            }
        }
    }
}
