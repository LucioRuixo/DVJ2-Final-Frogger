using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool moving = false;
    bool onWaterZone = false;

    Vector3 passiveMovement;

    public static event Action<int> onDeath;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water Zone Trigger")
            onWaterZone = !onWaterZone;
        else if (other.tag == "Water Zone Obstacle")
            passiveMovement = other.GetComponent<Obstacle>().GetMovement();
        else if (other.tag == "Road Zone Obstacle")
            Die();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water Zone Obstacle")
            passiveMovement = Vector3.zero;
    }

    void Update()
    {
        if (!moving)
        {
            if (Input.GetButton("Vertical"))
                StartCoroutine(Move(transform.position, transform.forward * Input.GetAxisRaw("Vertical")));
            else if (Input.GetButton("Horizontal"))
                StartCoroutine(Move(transform.position, transform.right * Input.GetAxisRaw("Horizontal")));

            if (passiveMovement != Vector3.zero)
                transform.position += passiveMovement * Time.deltaTime;
        }
    }

    void Die()
    {
        if (onDeath != null)
            onDeath(model.score);
    }

    IEnumerator Move(Vector3 initialPosition, Vector3 direction)
    {
        moving = true;

        Vector3 finalPosition = initialPosition + direction;

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