using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneInShip : MonoBehaviour
{
    [SerializeField] GameObject ProgressBar;
    public ContractAssignee Type; //Assigned in editor
    public float ResourceCount = 0.0f;
    public float ResourceMax = 150.0f; //Should be const

    void FixedUpdate()
    {
        float ResourceProgress = ResourceCount / ResourceMax;
        if (ResourceProgress > 1) ResourceProgress = 1;
        ProgressBar.transform.localScale = new Vector3(ResourceProgress, 1.1f, 1.0f);
    }
}
