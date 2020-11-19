using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{

    private Camera playerCamera;
    public float walkingSpeed = 7.5f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public float Strength;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private bool isCrouching;
    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
    
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        isCrouching = Input.GetKey(KeyCode.LeftControl);
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
 
        float curSpeedX = canMove ?  walkingSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ?  walkingSpeed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        if (!isCrouching)
        {
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            transform.localScale = new Vector3(1, 1f, 1);
            
        }
        else
        {
            moveDirection = (forward * (curSpeedX / 2) ) + (right * (curSpeedY / 2));

            transform.localScale = new Vector3(1,0.5f,1);
            
        }
       

       

        // Apply gravity
        
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        
            characterController.Move(moveDirection * Time.deltaTime);
        
      
      

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
       
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

       

        // Apply the push
        body.velocity = pushDir * ( Strength / hit.gameObject.GetComponent<Rigidbody>().mass);

        print(body.velocity.magnitude);
    }
}
