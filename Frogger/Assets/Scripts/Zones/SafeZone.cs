using System.Collections.Generic;
using UnityEngine;

public class SafeZone : Zone
{
    public List<GameObject> tilePrefabs;

    void Start()
    {
        Generate(false, tilePrefabs);
    }
}