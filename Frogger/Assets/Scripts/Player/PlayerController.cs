using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool active = true;
    bool moving = false;
    bool onWaterZone = false;

    float maxXValue = 5f;

    Vector3 passiveMovement;

    public static event Action<int> onDeath;
    public static event Action onLevelEndReached;

    void OnEnable()
    {
        LevelManager.onNewLevelGeneration += ResetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water Zone Trigger")
            onWaterZone = !onWaterZone;
        else if (other.tag == "Water Zone Obstacle")
            passiveMovement = other.GetComponent<Obstacle>().GetMovement();
        else if (other.tag == "Road Zone Obstacle")
            Die();
        else if (other.tag == "Level End Trigger")
        {
            active = false;
            model.IncreaseScore();

            if (onLevelEndReached != null) onLevelEndReached();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water Zone Obstacle")
        {
            if (moving)
                passiveMovement = Vector3.zero;
            else
                Die();
        }
    }

    void Update()
    {
        if (!active) return;

        if (!moving)
        {
            if (Input.GetButton("Vertical"))
                StartCoroutine(Move(transform.position, transform.forward * Input.GetAxisRaw("Vertical")));
            else if (Input.GetButton("Horizontal"))
                StartCoroutine(Move(transform.position, transform.right * Input.GetAxisRaw("Horizontal")));

            if (passiveMovement != Vector3.zero)
                transform.position += passiveMovement * Time.deltaTime;
        }

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXValue, maxXValue);
        if (clampedPosition.z < 0f)
            clampedPosition.z = 0f;
        transform.position = clampedPosition;
    }

    void OnDisable()
    {
        LevelManager.onNewLevelGeneration -= ResetPosition;
    }

    void Die()
    {
        if (onDeath != null) onDeath(model.score);

        Destroy(gameObject);
    }

    void ResetPosition()
    {
        transform.position = Vector3.zero;
        active = true;
    }

    IEnumerator Move(Vector3 initialPosition, Vector3 direction)
    {
        moving = true;

        Vector3 finalPosition = initialPosition + direction;
        finalPosition.x = Mathf.Clamp(finalPosition.x, -maxXValue, maxXValue);
        if (finalPosition.z < 0f)
            finalPosition.z = 0f;

        while (transform.position != finalPosition)
        {
            Vector3 position = transform.position;

            float fractionMoved = Vector3.Distance(initialPosition, position);
            float fractionToMove = Time.deltaTime * model.movementSpeed;

            position = Vector3.Lerp(initialPosition, finalPosition, fractionMoved + fractionToMove);
            transform.position = position;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        moving = false;

        if (onWaterZone && passiveMovement == Vector3.zero)
            Die();
    }
}