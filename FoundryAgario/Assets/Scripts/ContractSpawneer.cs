﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayspaceZone
{
    public Bounds bounds;
    public int count = 0;
    public bool locked = false;
}

public class ContractSpawneer : MonoSingleton<ContractSpawneer>
{
    [SerializeField] private GameObject environmentObject;
    [SerializeField] private GameObject friendlyObject;
    [SerializeField] private GameObject enemyObject;

    [SerializeField] private int numOfFriendlyContracts = 10; 
    [SerializeField] private int numOfEnemyContracts = 10; 

    private List<GameObject> friendlyObjects = new List<GameObject>();
    private List<GameObject> enemyObjects = new List<GameObject>();

    [SerializeField] private int zonesX = 10;
    [SerializeField] private int zonesY = 10;
    private Vector2 zoneSize;
    private List<PlayspaceZone> spawnZones = new List<PlayspaceZone>();

    [SerializeField] private float spawnInterval = 5.0f;
    private float spawnTimer = 0.0f;

    private Bounds envBounds;
    private Bounds lastBounds; //For in-editor only

    /* Create spawn zones and spawn entities within them */
    void Start()
    {
        CreateZones();
        SpawnEntities();
    }

    /* On set intervals, keep the entity count up to the max */
    void FixedUpdate()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            ValidateSpawnedEntities();
            SpawnEntities();
            spawnTimer = 0;
        }
    }

    /* Draw zone debugs in editor */
    void OnDrawGizmosSelected()
    {
        CreateZones(true);
    }

    /* Create all possible spawn zones */
    void CreateZones(bool drawDebug = false)
    {
        spawnZones.Clear();
        envBounds = environmentObject.GetComponent<BoxCollider2D>().bounds;
        if (drawDebug && envBounds == lastBounds) return; //Optimise for in-editor
        if (drawDebug) lastBounds = envBounds;
        zoneSize = new Vector2(envBounds.size.x / zonesX, envBounds.size.y / zonesY);
        for (int x = 0; x < zonesX; x++)
        {
            for (int y = 0; y < zonesY; y++)
            {
                PlayspaceZone zone = new PlayspaceZone();
                zone.bounds = new Bounds(new Vector3((zoneSize.x * x) + (zoneSize.x / 2), (zoneSize.y * y) + (zoneSize.y / 2)) + environmentObject.transform.position - (envBounds.size / 2), new Vector3(zoneSize.x, zoneSize.y));
                if (drawDebug) Gizmos.DrawWireCube(zone.bounds.center, zone.bounds.size);
                zone.locked = PointIsWithinCameraView(zone.bounds.center, drawDebug);
                spawnZones.Add(zone);
            }
        }
    }

    /* Spawn entites in the zones */
    void SpawnEntities()
    {
        for (int i = friendlyObjects.Count; i < numOfFriendlyContracts; i++)
        {
            friendlyObjects.Add(Instantiate(friendlyObject, GeneratePosition(), Quaternion.identity) as GameObject);
        }
        for (int i = enemyObjects.Count; i < numOfEnemyContracts; i++)
        {
            enemyObjects.Add(Instantiate(enemyObject, GeneratePosition(), Quaternion.identity) as GameObject);
        }
    }

    /* Remove any despawned entities from the entity lists */
    void ValidateSpawnedEntities()
    {
        List<GameObject> friendlyObjectsNew = new List<GameObject>();
        List<GameObject> enemyObjectsNew = new List<GameObject>();
        for (int i = 0; i < friendlyObjects.Count; i++)
        {
            if (friendlyObjects[i] != null)
            {
                friendlyObjectsNew.Add(friendlyObjects[i]);
            }
        }
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (enemyObjects[i] != null)
            {
                enemyObjectsNew.Add(enemyObjects[i]);
            }
        }
        friendlyObjects = friendlyObjectsNew;
        enemyObjects = enemyObjectsNew;
    }

    /* Evenly and randomly spawn entities in the zones */
    Vector3 GeneratePosition()
    {
        int zoneToSpawn = GetSpawnZone();
        Bounds zoneBounds = spawnZones[zoneToSpawn].bounds;
        return new Vector3(Random.Range(zoneBounds.min.x, zoneBounds.max.x), Random.Range(zoneBounds.min.y, zoneBounds.max.y), Random.Range(zoneBounds.min.z, zoneBounds.max.z));
    }
    int GetSpawnZone()
    {
        int zoneToSpawn = Random.Range(0, spawnZones.Count - 1);
        if (spawnZones[zoneToSpawn].count > GetAverageSpawnCount() || spawnZones[zoneToSpawn].locked)
        {
            return GetSpawnZone();
        }
        return zoneToSpawn;
    }
    float GetAverageSpawnCount()
    {
        float avg = 0.0f;
        foreach (PlayspaceZone zone in spawnZones)
        {
            avg += zone.count;
        }
        avg /= spawnZones.Count;
        return avg;
    }

    /* Check if a point is visible within the view space */
    public bool PointIsWithinCameraView(Vector3 point, bool drawDebug = false)
    {
        Bounds camBounds = new Bounds(ShipMovement.Instance.GetPosition(), new Vector3((Camera.main.aspect * (Camera.main.orthographicSize * 2)) + zoneSize.x, (Camera.main.orthographicSize * 2) + zoneSize.y));
        if (drawDebug) Gizmos.DrawWireCube(camBounds.center, camBounds.size);
        return camBounds.Contains(point);
    }
}
