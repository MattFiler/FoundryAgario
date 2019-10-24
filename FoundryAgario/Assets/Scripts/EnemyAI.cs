using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    void FixedUpdate()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, Vector3.zero, 0.025f);
    }
}
