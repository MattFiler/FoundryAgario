using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPointer : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPointer;
    [SerializeField] private GameObject FriendlyPointer;

    private List<GameObject> EnemyPointers = new List<GameObject>();
    private List<GameObject> FriendlyPointers = new List<GameObject>();

    [SerializeField] private float DistanceToTrack = 60.0f;

    /* Create all possible pointers when awoken */
    void Awake()
    {
        //Create all possible enemy pointer arrows
        for (int i = 0; i < ContractSpawneer.Instance.GetEnemyEntityMaxCount() * WorldScaleManager.Instance.maxDifficulty; i++)
        {
            EnemyPointers.Add(Instantiate(EnemyPointer, this.transform) as GameObject);
        }

        //Create all possible friendly pointer arrows
        for (int i = 0; i < ContractSpawneer.Instance.GetFriendlyEntityMaxCount(); i++)
        {
            FriendlyPointers.Add(Instantiate(FriendlyPointer, this.transform) as GameObject);
        }
    }

    /* Update the position of the pointers */
    void Update()
    {
        //Position the enemy entity pointers
        int EnemyIndex = 0;
        for (int i = 0; i < EnemyPointers.Count; i++)
        {
            EnemyPointers[i].SetActive(false);
        }
        foreach (GameObject enemyObject in ContractSpawneer.Instance.GetEnemyEntities())
        {
            if (enemyObject == null) continue;
            if (EnemyIndex > EnemyPointers.Count - 1) break; //FUCK OFF YOU FUCKING SHIT PIECE OF WANK
            if (Vector3.Distance(enemyObject.transform.position, ShipMovement.Instance.GetPosition()) < DistanceToTrack)
            {
                EnemyPointers[EnemyIndex].transform.position = ConvertPositionToClampedScreenspace(enemyObject.transform.position);
                EnemyPointers[EnemyIndex].transform.localScale = DistanceFromPointerToEntityAsVector(enemyObject.transform.position);
                EnemyPointers[EnemyIndex].SetActive(!IsPositionInScreenspace(EnemyPointers[EnemyIndex].transform.position));
            }
            EnemyIndex++;
        }

        //Position the friendly entity pointers
        int FriendlyIndex = 0;
        for (int i = 0; i < FriendlyPointers.Count; i++)
        {
            FriendlyPointers[i].SetActive(false);
        }
        foreach (GameObject friendlyObject in ContractSpawneer.Instance.GetFriendlyEntities())
        {
            if (friendlyObject == null) continue;
            if (Vector3.Distance(friendlyObject.transform.position, ShipMovement.Instance.GetPosition()) < DistanceToTrack)
            {
                FriendlyPointers[FriendlyIndex].transform.position = ConvertPositionToClampedScreenspace(friendlyObject.transform.position);
                FriendlyPointers[FriendlyIndex].transform.localScale = DistanceFromPointerToEntityAsVector(friendlyObject.transform.position);
                FriendlyPointers[FriendlyIndex].SetActive(!IsPositionInScreenspace(FriendlyPointers[FriendlyIndex].transform.position));
            }
            FriendlyIndex++;
        }
    }

    /* Convert a position to screen space and clamp it */
    private Vector3 ConvertPositionToClampedScreenspace(Vector3 position)
    {
        Vector3 ScreenspacePosition = Camera.main.WorldToScreenPoint(position);
        return new Vector3(Mathf.Clamp(ScreenspacePosition.x, 0, Screen.width), Mathf.Clamp(ScreenspacePosition.y, 0, Screen.height), 0.0f);
    }

    /* Get distance from pointer to entity */
    private float DistanceFromPointerToEntity(Vector3 entityPos)
    {
        return ((Vector3.Distance(entityPos, ShipMovement.Instance.GetPosition()) / DistanceToTrack) - 1) * -1;
    }
    private Vector3 DistanceFromPointerToEntityAsVector(Vector3 entityPos)
    {
        float dist = DistanceFromPointerToEntity(entityPos);
        return new Vector3(dist * 2, dist * 2, dist * 2);
    }

    /* Is the clamped screenspace within the visible screen? */
    private bool IsPositionInScreenspace(Vector3 position)
    {
        return ((position.x > 0.5f && position.x < Screen.width - 0.5f) && (position.y > 0.5f && position.y < Screen.height - 0.05f));
    }
}
