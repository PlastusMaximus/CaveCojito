using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerControls : MonoBehaviour
{
    
    private enum States : int
    {
        WALKING = 0,
        CRAWLING = 1
    }
    
    [SerializeField] private States state = States.WALKING;
    
    [SerializeField]
    private Light flashlight;
    private bool flashlightActive;
    
    [SerializeField]
    private FlashCam flashCam;
    
    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private CharacterController characterController;
    private bool canMove = true;
    
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0;

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
    private float lookYLimit = 90f;
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
        handlePlayerStates();
        
        characterController = GetComponent<CharacterController>();
        lockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        flashlightControls();
        flashCamControls();
        
        float movementDirectionY = moveDirection.y;
        
        setMovementDirection();
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

    private void handlePlayerStates()
    {
        switch (state)
        {
            case States.WALKING:
                characterController.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                playerCam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case States.CRAWLING:
                characterController.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                playerCam.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                break;
        }
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

    private void setMovementDirection()
    {
        Vector3 forward = Vector3.zero;
        Vector3 right = Vector3.zero;
                                
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = 0;
        float curSpeedY = 0;
        
        
        switch (state)
        {
            case  States.WALKING:
                
                forward = transform.TransformDirection(Vector3.forward);
                right = transform.TransformDirection(Vector3.right);
                
                curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
                curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
                
                break;
                
            case States.CRAWLING:
                forward = transform.TransformDirection(Vector3.up);
                right = transform.TransformDirection(Vector3.right);
                
                curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
                curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
                break;
        }
        
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
            switch (state)
            {
                case States.WALKING:
                    rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                    playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                    break;
                case States.CRAWLING:
                    rotationY += -Input.GetAxis("Mouse Y") * lookSpeed;
                    rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
                    playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                    break;
            }
            
        }
    }

}
