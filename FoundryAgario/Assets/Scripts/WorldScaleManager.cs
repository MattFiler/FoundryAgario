using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScaleManager : MonoBehaviour
{
    [SerializeField] private ScaleLevelContainer[] backgroundObjects;
    [SerializeField] private GameObject ship;
    [SerializeField] private float scaleAnimDuration = 2.0f;
    [SerializeField] private Vector3 minShipScale = Vector3.one;
    [SerializeField] private Vector3 maxShipScale = Vector3.one*2;

    // This is the current index in backgroundObjects to render
    private int scaleLevel = 0;
    private int scale = 0;

    private Vector3 targetShipScale = Vector3.one;
    private bool isAnimating = false;

    // Every scale added will increase the ships scale where 0 is minShipScale and 
    // 100 is maxShipScale and every 100 scale added will increase the scale level.
    // If already at max scale level, adding scale will have no effect
    public void AddScale(int scaleToAdd)
    {
        if (isAnimating)
            return;
        scale += scaleToAdd;
        if(scale >= 100)
        {
            if (scaleLevel == backgroundObjects.Length -1)
            {
                scale = 100;
            }
            else
            {
                StartCoroutine(ScaleWorldUp());
                return;
            }
        }
        targetShipScale = Vector3.Lerp(minShipScale, maxShipScale, ((float)scale / 100.0f));

    }

    private IEnumerator ScaleWorldUp()
    {
        isAnimating = true;
        targetShipScale = minShipScale;

        foreach (GameObject o in backgroundObjects[scaleLevel].objects)
        {
            StartCoroutine(ScaleObject(o, false));
        }
        foreach (GameObject o in backgroundObjects[scaleLevel + 1].objects)
        {
            StartCoroutine(ScaleObject(o, true));
        }

        float percentDone = 0;
        float percentStep = 2.0f / scaleAnimDuration;

        Vector3 originalScale = ship.transform.localScale;

        while (percentDone <= 100)
        {
            ship.transform.localScale = Vector3.Lerp(originalScale, minShipScale, percentDone / 100.0f);

            percentDone += percentStep;
            yield return new WaitForSeconds(0.02f);
        }

        scaleLevel++;
        scale = 0;
        isAnimating = false;
    }

    private IEnumerator ScaleObject(GameObject obj, bool scaleUp)
    {
        float percentDone = 0;
        float percentStep = 2.0f / scaleAnimDuration;

        Vector3 originalScale = obj.transform.localScale;

        // If scaling up then set the object as active and scale to 0
        if(scaleUp)
        {
            obj.transform.localScale = Vector3.zero;
            obj.SetActive(true);
        }

        while (percentDone <= 100)
        {
            if (scaleUp)
            {
                obj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, percentDone / 100.0f);
            }
            else
            {
                obj.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, percentDone / 100.0f);
            }

            percentDone += percentStep;
            yield return new WaitForSeconds(0.02f);
        }

        // Disable the scaled down object and return it to the original scale
        if(!scaleUp)
        {
            obj.SetActive(false);
            obj.transform.localScale = originalScale;
        }
    }


    private void Start()
    {
        targetShipScale = minShipScale;
        for (int i = 0; i < backgroundObjects.Length; i++)
        {
            if (i == scaleLevel)
                continue;
            foreach(GameObject g in backgroundObjects[i].objects)
            {
                g.SetActive(false);
            }
        }
    }

    void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.O))
        {
            AddScale(5);
        }
        #endif
        if(!isAnimating)
            ship.transform.localScale = Vector3.Lerp(ship.transform.localScale, targetShipScale, 0.2f);
    }
}

[System.Serializable]
class ScaleLevelContainer
{
    public GameObject[] objects;
}