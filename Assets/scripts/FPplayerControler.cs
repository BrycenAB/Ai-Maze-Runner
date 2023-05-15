using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FPplayerControler : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSens = 3.5f;  //mouse sensativity direct
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float RunSpeed = 20.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = .03f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = .03f;
    

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMiltiplier;
    [SerializeField] private KeyCode jumpKey;

    [SerializeField] public bool LockCursor = true;
    public bool UpdateCamera = true;

    float cameraPitch = 0.0f;
    public float velocityY = 0.0f;
    CharacterController controller = null;




    private bool isJumping;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;

    Vector2 currMouseDelta = Vector2.zero;
    Vector2 currMouseDeltaVelocity = Vector2.zero;

    
    void Start()
    {

        controller = GetComponent<CharacterController>();
        /// <summary>
        /// confine cursor to center and makes invisable
        /// </summary>
        if (LockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {
        updateMouseLook();
        updateMovement();
        //Flying();
    }

    
    void updateMouseLook()
    {
        if(UpdateCamera == true)
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVelocity, mouseSmoothTime);


            //camera pitch movement
            cameraPitch -= currMouseDelta.y * mouseSens;
            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);  // limits up and down movement (inverted for camera x/y)

            playerCamera.localEulerAngles = Vector3.right * cameraPitch;

            transform.Rotate(Vector3.up * currMouseDelta.x * mouseSens);
        }

        if (LockCursor == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        if (LockCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void updateMovement()
    {

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // gets raw value of vert and horiz directions (-1 to 1)
        targetDir.Normalize(); //on xy axis normalizes the middle axis distance from 1.41 back to 1 for consistant speed along all vectors

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDir, ref currentDirectionVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkSpeed = RunSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkSpeed = 6;
        }
            



        //velocity y = negative so vector3.up not .down                                         
        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        JumpInput();
        

    }

    void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(jumpEvent());
        }
    }

    private IEnumerator jumpEvent()
    {
        controller.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMiltiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;

        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45.0f;
        isJumping = false;
    }

    public void teleport(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;
    }
}
