using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum DangerZoneTypes
    {
        Road,
        Water
    }

    int totalZoneAmount;

    float tileOffset = 0.5f;
    float initialZoneX;
    float initialZoneZ;

    public GameObject safeZonePrefab;
    public GameObject waterZoneTriggerPrefab;

    public List<DangerZoneSO> dangerZoneSOs;

    [Header("Level settings")]
    public int levelWidth;
    public int dangerZoneAmount;
    public int minSafeZoneLength;
    public int maxSafeZoneLength;
    public int minDangerZoneLength;
    public int maxDangerZoneLength;

    void Start()
    {
        initialZoneX = ((float)(levelWidth / 2) - tileOffset) * -1f;
        initialZoneZ = -tileOffset;

        if (dangerZoneAmount < 1)
            dangerZoneAmount = 1;
        totalZoneAmount = dangerZoneAmount * 2 + 1;

        ClampZoneValues(minSafeZoneLength, maxSafeZoneLength);
        ClampZoneValues(minDangerZoneLength, maxDangerZoneLength);

        Generate();
    }

    void ClampZoneValues(int minValue, int maxValue)
    {
        if (minValue < 1)
            minValue = 1;

        if (maxValue < minValue)
            maxValue = minValue;
    }

    void Generate()
    {
        int currentTotalLength = 0;

        for (int i = 0; i < totalZoneAmount; i++)
        {
            int zoneLength;

            if (i % 2 == 0)
            {
                zoneLength = Random.Range(minSafeZoneLength, maxSafeZoneLength + 1);

                SafeZone newZone = Instantiate(safeZonePrefab, transform).GetComponent<SafeZone>();
                if (newZone) newZone.Initialize(initialZoneX, initialZoneZ + currentTotalLength, levelWidth, zoneLength);
            }
            else
            {
                zoneLength = Random.Range(minDangerZoneLength, maxDangerZoneLength + 1);

                DangerZoneSO newZoneSO = dangerZoneSOs[Random.Range(0, dangerZoneSOs.Count)];
                DangerZone newZone = Instantiate(newZoneSO.prefab, transform).GetComponent<DangerZone>();
                if (newZone)
                {
                    newZone.Initialize(initialZoneX, initialZoneZ + currentTotalLength, levelWidth, zoneLength);
                    newZone.SetType(newZoneSO.type);
                    newZone.SetObstacleValues(newZoneSO.minObstacleGenerationTime, newZoneSO.maxObstacleGenerationTime, newZoneSO.minObstacleSpeed, newZoneSO.maxObstacleSpeed);
                    newZone.SetListElements(newZoneSO.tilePrefabs, newZoneSO.obstaclePrefabs);

                    if (newZoneSO.type == DangerZoneTypes.Water)
                        newZone.SetWaterZoneTriggerPrefab(waterZoneTriggerPrefab);
                }
            }

            currentTotalLength += zoneLength;
        }
    }
}