using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractSpawneer : MonoBehaviour
{
    [SerializeField] private GameObject environmentObject;
    [SerializeField] private GameObject friendlyObject;
    [SerializeField] private GameObject enemyObject;

    [SerializeField] private int numOfFriendlyContracts = 10;
    [SerializeField] private int numOfEnemyContracts = 10;

    private List<GameObject> friendlyObjects = new List<GameObject>();
    private List<GameObject> enemyObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < numOfFriendlyContracts; i++)
        {
            friendlyObjects.Add(Instantiate(friendlyObject, GeneratePosition(), Quaternion.identity) as GameObject);
        }
        for (int i = 0; i < numOfEnemyContracts; i++)
        {
            enemyObjects.Add(Instantiate(enemyObject, GeneratePosition(), Quaternion.identity) as GameObject);
        }
    }

    Vector3 GeneratePosition()
    {
        Bounds envBounds = environmentObject.GetComponent<BoxCollider2D>().bounds;
        return new Vector3(Random.Range(envBounds.min.x, envBounds.max.x), Random.Range(envBounds.min.y, envBounds.max.y), Random.Range(envBounds.min.z, envBounds.max.z));
    }
}
