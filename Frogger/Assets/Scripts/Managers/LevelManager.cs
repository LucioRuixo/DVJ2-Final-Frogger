using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum DangerZoneTypes
    {
        Road,
        Water
    }

    int currentLevel = 0;
    int levelWidth = 32;
    int lastZoneExtraLength = 10;
    int totalZoneAmount;

    float tileOffset = 0.5f;
    float firstZoneZ = -1.5f;
    float initialZoneX;
    float initialZoneZ;

    public GameObject safeZonePrefab;
    public GameObject waterZoneTriggerPrefab;
    public GameObject levelEndTriggerPrefab;

    public List<DangerZoneSO> dangerZoneSOs;
    public List<GameObject> zones;

    [Header("Level settings")]
    public int baseDangerZoneAmount;
    public int minSafeZoneLength;
    public int maxSafeZoneLength;
    public int minBaseDangerZoneLength;
    public int maxBaseDangerZoneLength;

    public static event Action onNewLevelGeneration;
    public static event Action<int> onCurrentLevelUpdate;

    void OnEnable()
    {
        UIManager_Gameplay.onNextLevelButtonPressed += GenerateNewLevel;
    }

    void Start()
    {
        initialZoneX = (levelWidth / 2 - tileOffset) * -1f;
        initialZoneZ = -tileOffset;

        if (baseDangerZoneAmount < 1)
            baseDangerZoneAmount = 1;
        totalZoneAmount = baseDangerZoneAmount * 2 + 1;

        ClampZoneValues(minSafeZoneLength, maxSafeZoneLength);
        ClampZoneValues(minBaseDangerZoneLength, maxBaseDangerZoneLength);

        Generate();

        onCurrentLevelUpdate(currentLevel + 1);
    }

    void OnDisable()
    {
        UIManager_Gameplay.onNextLevelButtonPressed -= GenerateNewLevel;
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
        int zoneLength = 0;

        for (int i = 0; i < totalZoneAmount; i++)
        {
            if (i % 2 == 0)
            {
                zoneLength = UnityEngine.Random.Range(minSafeZoneLength, maxSafeZoneLength + 1);

                float initialZ;
                if (i == 0)
                {
                    initialZ = firstZoneZ;
                    zoneLength++;
                    currentTotalLength--;
                }
                else
                {
                    initialZ = initialZoneZ + currentTotalLength;

                    if (i == totalZoneAmount - 1)
                        zoneLength += lastZoneExtraLength;
                }

                zones.Add(Instantiate(safeZonePrefab, transform));
                SafeZone newZone = zones[zones.Count - 1].GetComponent<SafeZone>();
                if (newZone) newZone.Initialize(initialZoneX, initialZ, levelWidth, zoneLength);
            }
            else
            {
                zoneLength = UnityEngine.Random.Range(minBaseDangerZoneLength, maxBaseDangerZoneLength + 1) + currentLevel;

                DangerZoneSO newZoneSO = dangerZoneSOs[UnityEngine.Random.Range(0, dangerZoneSOs.Count)];
                zones.Add(Instantiate(newZoneSO.prefab, transform));
                DangerZone newZone = zones[zones.Count - 1].GetComponent<DangerZone>();
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

        GameObject levelEndTrigger = Instantiate(levelEndTriggerPrefab, transform);
        Vector3 size = levelEndTrigger.GetComponent<BoxCollider>().size;
        size.x = levelWidth;
        levelEndTrigger.GetComponent<BoxCollider>().size = size;
        Vector3 position = levelEndTrigger.transform.position;
        position.z = currentTotalLength - zoneLength - 1f - tileOffset;
        levelEndTrigger.transform.position = position;
    }

    void GenerateNewLevel()
    {
        foreach (GameObject zone in zones)
        {
            Destroy(zone);
        }

        IncreaseCurrentLevel();
        Generate();

        if (onNewLevelGeneration != null) onNewLevelGeneration();
    }

    void IncreaseCurrentLevel()
    {
        currentLevel++;

        if (onCurrentLevelUpdate != null) onCurrentLevelUpdate(currentLevel + 1);
    }
}