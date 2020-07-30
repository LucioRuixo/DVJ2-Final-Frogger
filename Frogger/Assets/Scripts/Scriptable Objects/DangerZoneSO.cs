using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Danger Zone", menuName = "Danger Zone")]
public class DangerZoneSO : ScriptableObject
{
    public float minObstacleGenerationTime;
    public float maxObstacleGenerationTime;
    public float minObstacleSpeed;
    public float maxObstacleSpeed;

    public LevelManager.DangerZoneTypes type;

    public GameObject prefab;

    public List<GameObject> tilePrefabs;
    public List<GameObject> obstaclePrefabs;
}