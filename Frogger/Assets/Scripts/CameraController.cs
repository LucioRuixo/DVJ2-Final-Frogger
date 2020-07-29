using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 rotation;

    public Transform pivot;

    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    void Update()
    {
        transform.position = pivot.position + offset;
    }
}