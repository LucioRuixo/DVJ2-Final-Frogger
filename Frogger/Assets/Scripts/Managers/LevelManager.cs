using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int totalZoneAmount;

    float tileOffset = 0.5f;
    float initialZoneX;
    float initialZoneZ;

    public GameObject safeZonePrefab;

    public List<GameObject> dangerZonePrefabs;

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
                GameObject prefab = dangerZonePrefabs[Random.Range(0, dangerZonePrefabs.Count)];

                zoneLength = Random.Range(minDangerZoneLength, maxDangerZoneLength + 1);

                DangerZone newZone = Instantiate(prefab, transform).GetComponent<DangerZone>();
                if (newZone) newZone.Initialize(initialZoneX, initialZoneZ + currentTotalLength, levelWidth, zoneLength);
            }

            currentTotalLength += zoneLength;
        }
    }
}