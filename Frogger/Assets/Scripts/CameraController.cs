using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 rotation;

    public Transform pivot;

    void Update()
    {
        if (!pivot) return;

        transform.rotation = Quaternion.Euler(rotation);

        Vector3 position = offset;
        position.z += pivot.position.z;
        transform.position = position;
    }
}