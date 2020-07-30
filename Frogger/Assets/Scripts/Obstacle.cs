using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool movingRight;

    float speed;
    float maxX;

    Vector3 movement;

    void Start()
    {
        movement = transform.forward * speed;
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        if (movingRight)
        {
            if (transform.position.x > maxX)
                Destroy(gameObject);
        }
        else
        {
            if (transform.position.x < maxX)
                Destroy(gameObject);
        }
    }

    public void Initialize(bool movingRight, float speed, float maxX)
    {
        this.movingRight = movingRight;
        this.speed = speed;
        this.maxX = maxX;
    }
}