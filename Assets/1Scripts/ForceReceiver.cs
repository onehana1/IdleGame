using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    private float verticalVelocity;
    public Vector3 Movement => Vector3.up * verticalVelocity;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
