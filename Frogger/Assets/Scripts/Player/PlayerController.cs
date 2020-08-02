using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;
    public PlayerView view;

    bool active = true;
    bool moving = false;
    bool onWaterZone = false;

    float maxXValue = 5f;

    Vector3 passiveMovement;

    public static event Action onLevelEndReached;
    public static event Action onDeath;

    void OnEnable()
    {
        LevelManager.onNewLevelGeneration += ResetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water Zone Trigger")
        {
            onWaterZone = !onWaterZone;

            if (onWaterZone)
                view.Jump();
            else
                view.Run();
        }
        else if (other.tag == "Water Zone Obstacle")
            passiveMovement = other.GetComponent<Obstacle>().GetMovement();
        else if (other.tag == "Road Zone Obstacle")
            Die();
        else if (other.tag == "Level End Trigger")
        {
            active = false;

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
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {
                string axis;
                Vector3 newDirection;

                if (Input.GetButton("Vertical"))
                {
                    axis = "Vertical";
                    newDirection = Vector3.forward * Input.GetAxisRaw(axis);
                }
                else
                {
                    axis = "Horizontal";
                    newDirection = Vector3.right * Input.GetAxisRaw(axis);
                }

                if (onWaterZone)
                    view.Jump();
                else
                    view.Run();

                StartCoroutine(Move(transform.position, newDirection));

                float angle = Vector3.Angle(transform.forward, newDirection);
                if (angle != 0f)
                    StartCoroutine(TurnAround(newDirection));
            }

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
        if (onDeath != null) onDeath();

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

        view.Idle();
    }

    IEnumerator TurnAround(Vector3 finalDirection)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.LookRotation(finalDirection);
        float angle = Quaternion.Angle(initialRotation, finalRotation);

        while (transform.forward != finalDirection)
        {
            Quaternion rotation = transform.rotation;

            float fractionMoved = Quaternion.Angle(initialRotation, rotation) / angle;
            float fractionToMove = Time.deltaTime * model.rotationSpeed;

            rotation = Quaternion.Lerp(initialRotation, finalRotation, fractionMoved + fractionToMove);
            transform.rotation = rotation;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}