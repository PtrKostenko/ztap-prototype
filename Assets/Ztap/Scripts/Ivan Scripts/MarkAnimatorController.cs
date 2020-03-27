using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAnimatorController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator = null;
    public CharacterController characterController = null;
    private bool block = false;
    private bool swordSheath = false;
    private bool crouching = false;
    private bool moveWithSword = false;

    private float speedSmoothTime = 0.1f;
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        characterController = this.gameObject.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        SwordSheath();
        if (swordSheath)
        {
            Block();
            Crouching();
            Attack();
        }
        Jump();
    }

    void Block()
    {
        if (Input.GetKey(KeyCode.Mouse1))
            block = true;
        else
            block = false;

        animator.SetBool("block", block);
    }

    void SwordSheath()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            swordSheath = !swordSheath;


            animator.SetBool("sheathSword", swordSheath);

            if (swordSheath)
            {
                animator.SetLayerWeight(2, 0f);
                animator.SetLayerWeight(1, 1f);
                crouching = false;

            }
            else
            {
                animator.SetLayerWeight(2, 1f);
                animator.SetLayerWeight(1, 0f);
                crouching = false;

            }
        }
    }

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
            crouching = !crouching;
        animator.SetBool("crouching", crouching);
    }

    void Attack()
    {
        if (Input.GetKey(KeyCode.F))
        {
            animator.SetFloat("attackPower", 2f, speedSmoothTime, Time.deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetFloat("variants", Random.Range(0, 5));
                animator.SetTrigger("attackTrigger");
            }

        }
        else
        {
            animator.SetFloat("attackPower", 1f, speedSmoothTime, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    animator.SetFloat("variants", Random.Range(0, 2));
                else
                    animator.SetFloat("variants", Random.Range(2, 5));

                animator.SetTrigger("attackTrigger");
            }
        }


        
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("Jump");
    }
}
