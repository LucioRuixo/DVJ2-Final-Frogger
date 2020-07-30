using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    protected int width;
    protected int length;

    protected float initialX;
    protected float initialZ;

    protected void Generate(bool dangerZone, List<GameObject> tilePrefabs)
    {
        Vector3 position = new Vector3(initialX, 0f, initialZ);
        GameObject prefab = null;

        for (int i = 0; i < length; i++)
        {
            position.z = initialZ + i;

            if (tilePrefabs.Count > 0)
            {
                if (dangerZone)
                {
                    if (i == 0)
                        prefab = tilePrefabs[0];
                    else if (i == length - 1)
                        prefab = tilePrefabs[tilePrefabs.Count - 1];
                    else if (tilePrefabs.Count > 3)
                        prefab = tilePrefabs[Random.Range(1, tilePrefabs.Count - 1)];
                    else
                        prefab = tilePrefabs[1];
                }
                else if (tilePrefabs.Count > 1)
                    prefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
                else
                    prefab = tilePrefabs[0];
            }

            for (int j = 0; j < width; j++)
            {
                position.x = initialX + j;
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }

    public void Initialize(float initialX, float initialZ, int width, int length)
    {
        this.width = width;
        this.length = length;
        this.initialX = initialX;
        this.initialZ = initialZ;
    }
}