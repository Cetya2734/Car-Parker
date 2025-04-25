using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float reverseSpeed = 3f;
    public float turnSpeed = 150f;

    private float moveInput;
    private float turnInput;

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        MoveCar();
        TurnCar();
    }

    void MoveCar()
    {
        float speed = moveInput > 0 ? moveSpeed : reverseSpeed;
        transform.Translate(Vector3.up * moveInput * speed * Time.deltaTime);
    }

    void TurnCar()
    {
        if (moveInput != 0)
        {
            float direction = moveInput > 0 ? -1 : 1; // flip turning when reversing
            transform.Rotate(Vector3.forward * turnInput * turnSpeed * Time.deltaTime * direction);
        }
    }
}
