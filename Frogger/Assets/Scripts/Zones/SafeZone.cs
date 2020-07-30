using UnityEngine;

public class SafeZone : MonoBehaviour
{
    int width;
    int length;

    float initialX;
    float initialZ;

    public GameObject tilePrefab;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Vector3 position = new Vector3(initialX, 0f, initialZ);

        for (int i = 0; i < length; i++)
        {
            position.z = initialZ + i;

            for (int j = 0; j < width; j++)
            {
                position.x = initialX + j;
                Instantiate(tilePrefab, position, Quaternion.identity, transform);
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