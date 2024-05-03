using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;
    public bool isPlayerGrounded = true;
    public Transform cam;
    public float speed = 8f;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private Vector3 moveStep;
    //private float jumpHeight = 1.0f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float gravityMultiplier = 4;
    [SerializeField] float groundedGravity = -0.5f;
    [SerializeField] float jumpHeight = 2f;
    float velocityY = 0f;
    [SerializeField] Animator animator;
    
    public float rollFactor = 3f;
    public float rollDuration = 1.0f;
    public KeyCode rollKey = KeyCode.LeftCommand;
 
    private bool isRolling = false;
    private float rollTime = 0.0f;
    private Vector3 rollDirection;
    
    private float gatherDuration = 1.5f;
    private float gatherTime = 0.0f;
    private bool isGathering = false;
    private KeyCode gatherKey = KeyCode.F;
    
    private GameObject[] wells;
    
    private AudioSource stepSound;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        wells = GameObject.FindGameObjectsWithTag("well");
        stepSound = GetComponent<AudioSource>();
        GameState.CharacterWater = 1.0f;
    }


    void Update()
    {
        GameState.CharacterWater -= 0.1f * Time.deltaTime;
        // if well near character
        if (isGathering)
        {
            gatherTime += Time.deltaTime;
            if (gatherTime >= gatherDuration)
            {
                isGathering = false;
            }
            else
            {
                return;
            }
        }
        int animationState = animator.GetInteger("state");
        float horizontal = Input.GetAxisRaw( "Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (isPlayerGrounded && velocityY < 0f && !isRolling)
        {
            velocityY = groundedGravity;
            animationState = 0;
        }
        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        
        if (!isRolling && Input.GetKeyDown(rollKey))
        {
            isRolling = true;
            rollTime = 0.0f;
            rollDirection = transform.forward;
            animationState = 4;
            controller.height /= 2;
            controller.center = new Vector3(controller.center.x, -(controller.height / 2), controller.center.z);
            //transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        if (isRolling)
        {
            rollTime += Time.deltaTime;
            float rollProgress = rollTime / rollDuration;
            float rollAngle = rollProgress * 360.0f;
            //moveStep = rollDirection * speed * rollFactor;
 
            if (rollTime >= rollDuration)
            {
                isRolling = false;
                controller.height *= 2;
                controller.center = new Vector3(controller.center.x, 0, controller.center.z);
                //transform.position. = new Vector3(1f, 1f, 1f);
                //transform.rotation = Quaternion.identity;
            }
        }
        if ((direction.magnitude >= 0.1f || isRolling) && !isGathering){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (!isRolling)
            {
                animationState = 1;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            moveStep = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveStep.x *= 3.0f;
                moveStep.z *= 3.0f;
                if(!isRolling)
                    animationState = 2;
            }
            //moveDir.y += gravityValue * Time.deltaTime;
        }
        else
        {
            moveStep = Vector3.zero;
        }
        if (isPlayerGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetInteger("state", 3);
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
            isPlayerGrounded = false;
            
        }
        moveStep.y = velocityY;
       
        if (isRolling)
        {
            if(rollTime / rollDuration > 0.25f)
            {
                moveStep.x = rollDirection.x * speed * rollFactor;
                moveStep.z = rollDirection.z * speed * rollFactor;
            }
        }

        if (moveStep.x == 0 && moveStep.z == 0 && !isRolling)
        { 
            if (Input.GetKeyDown(KeyCode.F))
            {
                animationState = 5;
                gatherTime = 0.0f;
                isGathering = true;
            }
        }
        if (isPlayerGrounded)
        {
            animator.SetInteger("state", animationState);
        }
        controller.Move(moveStep.normalized * (speed * Time.deltaTime));
        
        if (Time.timeScale > 0 && (animationState is 1 or 2 or 4))
        {
            if (!stepSound.isPlaying)
            {
                stepSound.Play();
            }
        }
        else
        {
            stepSound.Stop();
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enter: " + other.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter: " + other.name);
        if (other.name == "Terrain")
        {
            isPlayerGrounded = true;
            animator.SetInteger("state", 0);
            //Debug.Log(groundedPlayer);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Terrain")
        {
            isPlayerGrounded = false;
        }
    }
}
