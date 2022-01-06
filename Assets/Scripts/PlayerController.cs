using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform diceModel;
    [SerializeField] private Transform leftLimit, rightLimit;
    [SerializeField] private float forwardMovementSpeed = 1f, sideMovementSensitivity = 1f, rotationSpeed = 1f, rightRotateSpeed = 10f;

    private Vector2 inputDrag;
    private Vector2 previousMousePosition;

    private float leftLimitX => leftLimit.localPosition.x;
    private float rightLimitX => rightLimit.localPosition.x;

    void Update()
    {
        HandleForwardMovement();
        HandleInput();
    }

    private void HandleForwardMovement()
    {
        //transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
        
        rb.velocity = new Vector3(rb.velocity.x + sideMovementSensitivity * inputDrag.x, 0, forwardMovementSpeed);
    }


    /*private void HandleSideMovement()
    {
        var localPos = sideMovementRoot.localPosition;
        localPos += Vector3.right * inputDrag.x * sideMovementSensitivity;

        localPos.x = Mathf.Clamp(localPos.x, leftLimitX, rightLimitX);

        sideMovementRoot.localPosition = localPos;

        var moveDirection = Vector3.forward * 0.5f;
        moveDirection += sideMovementRoot.right * inputDrag.x * sideMovementSensitivity;

        moveDirection.Normalize();

        var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        sideMovementRoot.rotation = Quaternion.Lerp(sideMovementRoot.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }*/

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var deltaMouse = (Vector2)Input.mousePosition - previousMousePosition;
            inputDrag = deltaMouse;
            previousMousePosition = Input.mousePosition;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    var rb = other.attachedRigidbody;
    //    rb.isKinematic = false;
    //    other.isTrigger = false;
    //    rb.velocity = transform.forward * 50f;
    //}
}
