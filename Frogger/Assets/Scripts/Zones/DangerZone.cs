using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : Zone
{
    float obstacleOffset = 0.5f;
    float obstacleMaxXOffset = 2f;
    float minObstacleGenerationTime;
    float maxObstacleGenerationTime;
    float minObstacleSpeed;
    float maxObstacleSpeed;

    LevelManager.DangerZoneTypes type;

    GameObject waterZoneTriggerPrefab = null;

    List<GameObject> tilePrefabs;
    List<GameObject> obstaclePrefabs;

    void Start()
    {
        Generate(true, tilePrefabs);

        if (type == LevelManager.DangerZoneTypes.Water)
            GenerateWaterZoneTriggers();

        bool onLeft;
        for (int i = 0; i < length - 1; i++)
        {
            onLeft = Random.Range(0, 2) % 2 == 0 ? true : false;
            float speed = Random.Range(minObstacleSpeed, maxObstacleSpeed);
            StartCoroutine(GenerateObstacles(onLeft, initialZ + obstacleOffset + i, speed));
        }
    }

    void GenerateWaterZoneTriggers()
    {
        GameObject newTrigger = Instantiate(waterZoneTriggerPrefab, transform);
        Vector3 size = newTrigger.GetComponent<BoxCollider>().size;
        size.x = (float)width;
        newTrigger.GetComponent<BoxCollider>().size = size;
        Vector3 position = newTrigger.transform.position;
        position.z = initialZ;
        newTrigger.transform.position = position;

        newTrigger = Instantiate(waterZoneTriggerPrefab, transform);
        size = newTrigger.GetComponent<BoxCollider>().size;
        size.x = (float)width;
        newTrigger.GetComponent<BoxCollider>().size = size;
        position = newTrigger.transform.position;
        position.z = initialZ + length - 1f;
        newTrigger.transform.position = position;
    }

    public void SetType(LevelManager.DangerZoneTypes type)
    {
        this.type = type;
    }

    public void SetObstacleValues(float minObstacleGenerationTime, float maxObstacleGenerationTime, float minObstacleSpeed, float maxObstacleSpeed)
    {
        this.minObstacleGenerationTime = minObstacleGenerationTime;
        this.maxObstacleGenerationTime = maxObstacleGenerationTime;
        this.minObstacleSpeed = minObstacleSpeed;
        this.maxObstacleSpeed = maxObstacleSpeed;
    }

    public void SetListElements(List<GameObject> tilePrefabs, List<GameObject> obstaclePrefabs)
    {
        this.tilePrefabs = tilePrefabs;
        this.obstaclePrefabs = obstaclePrefabs;
    }

    public void SetWaterZoneTriggerPrefab(GameObject waterZoneTriggerPrefab)
    {
        this.waterZoneTriggerPrefab = waterZoneTriggerPrefab;
    }

    IEnumerator GenerateObstacles(bool onLeft, float initialZ, float speed)
    {
        float waitTime;

        Vector3 position = Vector3.zero;
        Quaternion addedRotation = Quaternion.identity;
        GameObject prefab;
        Obstacle newObstacle;

        Vector3 rotationLeftEuler = Vector3.zero;
        rotationLeftEuler.y = 90f;
        Quaternion rotationLeft = Quaternion.Euler(rotationLeftEuler);

        Vector3 rotationRightEuler = Vector3.zero;
        rotationRightEuler.y = -90f;
        Quaternion rotationRight = Quaternion.Euler(rotationRightEuler);

        while (this)
        {
            waitTime = Random.Range(minObstacleGenerationTime, maxObstacleGenerationTime);
            yield return new WaitForSeconds(waitTime);

            prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            position.z = initialZ;
            if (onLeft)
            {
                position.x = initialX;
                addedRotation = rotationLeft;
            }
            else
            {
                position.x = initialX + width;
                addedRotation = rotationRight;
            }

            bool movingRight = onLeft ? true : false;
            float maxX = onLeft ? initialX + width + obstacleMaxXOffset : initialX - obstacleMaxXOffset;

            newObstacle = Instantiate(prefab, position, prefab.transform.rotation, transform).GetComponent<Obstacle>();
            newObstacle.transform.rotation *= addedRotation;
            newObstacle.Initialize(movingRight, speed, maxX);
        }
    }
}