using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotions : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator = null;
    public CharacterController characterController = null;

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float speedValue = 2f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.06f;
    private float gravity = 3f;
    private int animationMultiplyer;
    [SerializeField] private GameObject[] objectsNotRotating;
    [SerializeField] private float sprintSpeed = 4f;

    private Transform mainCameraTransform;

    private void Awake()
    {
        //Get the animator
        animator = this.gameObject.GetComponent<Animator>();
        characterController = this.gameObject.GetComponent<CharacterController>();

        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Sprint();
        Attack();
        VFXSettings();
    }

    private void Move()
    {
        
        //Movement Inputs for pivot

        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
       
        //Gravity settings

        Vector3 gravityVector = Vector3.zero;
        if (!characterController.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        characterController.Move(gravityVector * Time.deltaTime);

        //Basic Movement
        
        //Character rotation
        if(desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
        }



        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        characterController.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);

        //Animator set animation "idle" if walue = 0, "walk" if value = 1, "run" if value = 2
        animator.SetFloat("freeMotion", movementInput.magnitude * animationMultiplyer, speedSmoothTime, Time.deltaTime);
    }

    private void Sprint()
    {
        //On press LShift character is running

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = sprintSpeed;
                rotationSpeed = 0.06f;
                animationMultiplyer = 2;
            }
            else
            {
                animationMultiplyer = 1;
                rotationSpeed = 0.06f;
                movementSpeed = speedValue;
            } 
    }

    private void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {
            movementSpeed = 1f;
            rotationSpeed = 0.01f;
        }     
    }

    private void VFXSettings()
    {
        for (int i = 0; i < objectsNotRotating.Length; i++)
        {
            objectsNotRotating[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
