using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForContracts : MonoBehaviour
{
    public List<GameObject> currentContracts;
    public GetMousePos mousePos;

    private void Update()
    {
        foreach(GameObject contract in currentContracts)
        {
            FriendlyAI thisAi = contract.GetComponent<FriendlyAI>();
            if (thisAi != null)
            {
                thisAi.SetShouldMove(mousePos.mouseDown);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Friendly") && collision.gameObject.GetComponent<FriendlyAI>().GetWidth() > LazyGlobalStuff.Instance.ThisWidth)
        {
            collision.GetComponent<FriendlyAI>().SetShouldMove(false);
            currentContracts.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Friendly"))
        {
           if(currentContracts.Contains(collision.gameObject))
           {
                collision.GetComponent<FriendlyAI>().SetShouldMove(true);
                currentContracts.Remove(collision.gameObject);
           }
        }
    }
}
