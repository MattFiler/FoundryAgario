using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScaleManager : MonoSingleton<WorldScaleManager>
{
    [SerializeField] private ScaleLevelContainer[] backgroundObjects;
    [SerializeField] private GameObject ship;
    [SerializeField] private float scaleAnimDuration = 2.0f;
    private Vector3 minShipScale = Vector3.one;
    [SerializeField] private Vector3 maxShipScale = Vector3.one*2;
    [SerializeField] private ParticleSystem backGround;

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

        Vector3 startShapeScale = backGround.shape.scale;
        Vector3 endShapeScale = backGround.shape.scale * 2.0f;

        Vector3 startPSScale = backGround.gameObject.transform.localScale;
        Vector3 endPSScale = backGround.gameObject.transform.localScale * 0.5f;


        while (percentDone <= 100)
        {
            if (percentDone <= 10)
            {
                ship.transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.1f, percentDone / 10.0f);
            }
            else if (percentDone <= 20)
            {
                ship.transform.localScale = Vector3.Lerp(originalScale * 1.1f, originalScale, (percentDone - 10) / 10.0f);
            }
            else if (percentDone <= 75)
            {
                ship.transform.localScale = Vector3.Lerp(originalScale, minShipScale, (percentDone - 20) / 55.0f);
            }
            else if (percentDone <= 85)
            {
                ship.transform.localScale = Vector3.Lerp(minShipScale, minShipScale * 0.9f, (percentDone - 75) / 10.0f);
            }
            else if(percentDone <= 95)
            {
                ship.transform.localScale = Vector3.Lerp(minShipScale * 0.9f, minShipScale*1.05f, (percentDone - 85) / 10.0f);
            }
            else
            {
                ship.transform.localScale = Vector3.Lerp(minShipScale * 1.05f, minShipScale, (percentDone - 95) / 5.0f);
            }

            var whaat = backGround.shape;
            whaat.scale = Vector3.Lerp(startShapeScale, endShapeScale, percentDone / 100.0f);

            backGround.gameObject.transform.localScale = Vector3.Lerp(startPSScale, endPSScale, percentDone / 100.0f);

            percentDone += percentStep;
            yield return new WaitForSeconds(0.02f);
        }

        scaleLevel++;
        scale = 0;
        isAnimating = false;
    }

    private IEnumerator ScaleObject(GameObject obj, bool scaleIn, bool fade = false)
    {
        float percentDone = 0;
        float percentStep = 2.0f / scaleAnimDuration;

        Vector3 originalScale = obj.transform.localScale;
        float originalAlpha = obj.GetComponent<SpriteRenderer>().color.a;
        Color col = obj.GetComponent<SpriteRenderer>().color;

        // If scaling up then set the object as active and scale to 0
        if (scaleIn)
        {
            obj.transform.localScale = originalScale * 1.5f;
            obj.SetActive(true);
        }

        while (percentDone <= 100)
        {
            if (scaleIn)
            {
                col.a = Mathf.Lerp(0, originalAlpha, percentDone / 100.0f);
                obj.transform.localScale = Vector3.Lerp(originalScale*1.5f, originalScale, percentDone / 100.0f);
                obj.GetComponent<SpriteRenderer>().color = col;

            }
            else
            {
                obj.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, percentDone / 100.0f);
            }

            percentDone += percentStep;
            yield return new WaitForSeconds(0.02f);
        }

        // Disable the scaled down object and return it to the original scale
        if(!scaleIn)
        {
            obj.SetActive(false);
            obj.transform.localScale = originalScale;
        }
    }


    private void Start()
    {
        minShipScale = ship.transform.localScale;
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