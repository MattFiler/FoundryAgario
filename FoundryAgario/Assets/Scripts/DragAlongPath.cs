using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAlongPath : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] points;
    public List<Line> lines = new List<Line>();
    [SerializeField] private GameObject[] targets;
    public bool pathIsLoop = true;

    private Dictionary<GameObject, Line> currentLines = new Dictionary<GameObject, Line>();

    private Dictionary<GameObject, Quaternion> targetAngles = new Dictionary<GameObject, Quaternion>();
    private Dictionary<GameObject, Vector3> targetPos = new Dictionary<GameObject, Vector3>();

    public bool forceBoxColliderEnable = false;
    private void Start()
    {
        GenerateLinesFromPath();
        foreach(GameObject target in targets)
        {
            target.transform.localPosition = GetClosestPointOnPath(target.transform.localPosition, target);
            targetPos[target] = target.transform.localPosition;
            Vector3 vec = currentLines[target].b - currentLines[target].a;
            vec = new Vector3(-vec.x, -vec.y);
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            targetAngles[target] = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject target in targets)
        {
            Touch touch = new Touch();
            if (TouchManager.instance.GetTouch(target, ref touch))
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    target.GetComponent<BoxCollider2D>().enabled = true;

                    targetPos[target] = GetClosestPointOnPath(transform.InverseTransformPoint(touch.position), target);
                    Vector3 vec = currentLines[target].b - currentLines[target].a;
                    vec = new Vector3(-vec.x, -vec.y);
                    float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                    targetAngles[target] = Quaternion.AngleAxis(angle, Vector3.forward);

                    if (target.GetComponent<CanFire>() != null)
                    {
                        target.GetComponent<CanFire>().canFire = false;
                    }
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    if (target.GetComponent<CanFire>() != null)
                    {
                        target.GetComponent<CanFire>().canFire = true;
                    }
                }
            }
            else
            {
                target.GetComponent<BoxCollider2D>().enabled = false;


                if ( target.GetComponent<CanFire>() != null)
                {
                    target.GetComponent<CanFire>().canFire = true;
                }
            }

            target.transform.rotation = Quaternion.Lerp(target.transform.rotation, targetAngles[target], 0.1f);
            target.transform.localPosition = Vector3.Lerp(target.transform.localPosition, targetPos[target], 0.6f);
        }
    }

    private void GenerateLinesFromPath()
    {
        for(int i = 0; i < points.Length; i++)
        {
            Line line = new Line();

            Vector3 a = transform.InverseTransformPoint(points[i]);
            Vector3 b;
            if (i == points.Length - 1)
                b = transform.InverseTransformPoint(points[0]);
            else
                b = transform.InverseTransformPoint(points[i + 1]);

            line.a = a;
            line.b = b;

            float denom = b.x - a.x;
            if (denom == 0)
            {
                line.vertLine = true;
                line.c = a.x;
            }
            else
            {
                line.m = (b.y - a.y) / denom;

                line.c = a.y - (line.m * a.x);
            }
            lines.Add(line);
        }
    }

    private Vector3 GetClosestPointOnPath(Vector3 point, GameObject objRef)
    {
        float closestDist = 10000000;
        Vector3 returnVec = new Vector3();
        foreach(Line line in lines)
        {
            float dist = 0;
            Vector2 vec = new Vector2();
            // If the line is horizontal, then just take the distance based on y diff
            if(line.m == 0)
            {
                dist = Mathf.Abs(line.c - point.x);
                vec.x = point.x;
                vec.y = line.c;
            }
            // If the line is vertical then just take hte distance based on x diff
            else if(line.vertLine)
            {
                dist = Mathf.Abs(line.c - point.y);
                vec.y = point.y;
                vec.x = line.c;
            }
            else
            {
                // y = mx + c
                // x = (y - c) / m

                // Get the intersect of the horitonzal line from this point
                float x = (point.y - line.c) / line.m;

                float hyp = Mathf.Abs(x - point.x);

                // tan(Θ) = opp/adj
                // Θ = atan(opp/adj)
                // opp/adj is the same as the magnitude of line, as this is an axis-alighned right anagled triangle
                float angle = Mathf.Atan(Mathf.Abs(line.m));

                // sin(Θ) = opp/hyp
                // opp = sin(Θ) * hyp
                dist = Mathf.Sin(angle) * hyp;

                // Lastly find the co-ordinates of the point on the line
                // Start by getting the length of the remaining side
                // cos(Θ) = adj/hyp
                // adj = cos(Θ) * hyp
                float adj = Mathf.Cos(angle) * hyp;

                // Then using a right-angled trianle with the line as its hypotenuse, calulate the width/height
                // opp = sin(Θ) * hyp
                float height = Mathf.Sin(angle) * adj; // the old adj is the new hyp
                // adj = cos(Θ) * hyp
                float width = Mathf.Cos(angle) * adj;

                if (x < point.x)
                {
                    vec.x = x + width;
                    if (line.m < 0)
                        vec.y = point.y - height;
                    else
                        vec.y = point.y + height;
                }
                else
                {
                    vec.x = x - width;
                    if (line.m < 0)
                        vec.y = point.y + height;
                    else
                        vec.y = point.y - height;
                }


            }

            if (dist < closestDist)
            {
                if (line.IsPointInLimits(vec))
                {
                    closestDist = dist;
                    returnVec = vec;
                    currentLines[objRef] = line;
                }
            }
        }

        return returnVec;
    }
}

public class Line
{
    public float m = 0;
    public float c = 0;

    public bool vertLine = false;

    public Vector3 a;
    public Vector3 b;

    public bool IsPointInLimits(Vector3 point)
    {
        float t = 0.2f;

        bool xIn = false;
        if (a.x > b.x)
            xIn = (point.x < a.x + t && point.x > b.x - t);
        else
            xIn = (point.x < b.x + t && point.x > a.x - t);

        if (xIn)
        {
            if (a.y > b.y)
                return (point.y < a.y + t && point.y > b.y - t);
            else
                return (point.y < b.y + t && point.y > a.y - t);
        }
        return false;
    }
}