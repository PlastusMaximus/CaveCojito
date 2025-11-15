using System.Collections;

using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Light flashlight;
    private bool flashlightActive;
    
    [SerializeField]
    private FlashCam flashCam;
    
    [SerializeField]
    private Camera playerCam;
    private CharacterController characterController;
    private bool canMove = true;
    
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    [SerializeField]
    private float walkSpeed = 6f;
    [SerializeField]
    private float runSpeed = 12f;
    [SerializeField]
    private float jumpPower = 7f;
    [SerializeField]
    private float gravity = 10f;
    [SerializeField]
    private float lookSpeed = 2f;
    [SerializeField]
    private float lookXLimit = 45f;
    [SerializeField]
    private float defaultHeight = 2f;
    [SerializeField]
    private float crouchHeight = 1f;
    [SerializeField]
    private float crouchSpeed = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deactivateLights();
        
        characterController = GetComponent<CharacterController>();
        lockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        flashlightControls();
        flashCamControls();
        
        float movementDirectionY = moveDirection.y;
        
        setMovementDirection(movementDirectionY);
        handleJumping(movementDirectionY);
        handleGravity();
        handleCrouching();
        handleMovement();
        
    }

    private void deactivateLights()
    {
        flashlight.gameObject.SetActive(false);  
        flashCam.gameObject.SetActive(false);
    }

    private void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void flashlightControls()
    {
        if (Input.GetKeyDown(KeyCode.F))
                {
                    if (flashlightActive == false)
                    {
                        flashlight.gameObject.SetActive(true);
                        flashlightActive = true;
                    }
                    else
                    {
                        flashlight.gameObject.SetActive(false);
                        flashlightActive = false;
                    }
                }
    }

    private void flashCamControls()
    {
        
        if (!flashCam.on)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                flashCam.gameObject.SetActive(true);
                flashCam.on = true;
                flashCam.flashBanged();        
            }
        }
    }

    private void setMovementDirection(float movementDirectionY)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    }

    private void handleJumping(float movementDirectionY)
    {
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        } 
        else
        {
            moveDirection.y = movementDirectionY;
        }
    }
    
    private void handleGravity()
    {
        if (!characterController.isGrounded)

        {

            moveDirection.y -= gravity * Time.deltaTime;

        }
    }

    private void handleCrouching()
    {
        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {

            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;

        }
    }

    private void handleMovement()
    {
        characterController.Move(moveDirection * Time.deltaTime);

        
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

}
