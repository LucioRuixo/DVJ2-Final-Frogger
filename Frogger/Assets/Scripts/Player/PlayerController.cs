using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool moving = false;
    bool onWaterZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water Zone Trigger")
            onWaterZone = !onWaterZone;
    }

    void Update()
    {
        if (!moving)
        {
            if (Input.GetButtonDown("Vertical"))
                StartCoroutine(Move(transform.position, transform.forward * Input.GetAxisRaw("Vertical")));
            else if (Input.GetButtonDown("Horizontal"))
                StartCoroutine(Move(transform.position, transform.right * Input.GetAxisRaw("Horizontal")));
        }
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
    }
}