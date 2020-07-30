using UnityEngine;

public class DangerZone : MonoBehaviour
{
    int width;
    int length;

    float initialX;
    float initialZ;

    public GameObject startTilePrefab;
    public GameObject tilePrefab;
    public GameObject endTilePrefab;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Vector3 position = new Vector3(initialX, 0f, initialZ);
        GameObject prefab;

        for (int i = 0; i < length; i++)
        {
            position.z = initialZ + i;

            if (i == 0)
                prefab = startTilePrefab;
            else if (i == length - 1)
                prefab = endTilePrefab;
            else
                prefab = tilePrefab;

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