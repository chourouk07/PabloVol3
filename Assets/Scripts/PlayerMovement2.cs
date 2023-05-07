using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    CharacterStats stats; 
    //Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
   

    //CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 turnvelocity;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;
    //References
    private CharacterController controller;
    private Animator anim;
    public bool playerIsAttacking = false; 








    private Rigidbody rb;

    /**/

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();


        /**/
        rb = GetComponent<Rigidbody>();
        /**/
        stats = GetComponent <CharacterStats>();
    }
    private void Update()
    {
        
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack2();
            playerIsAttacking= true;
            StartCoroutine(ResetAttack(1.0f));
        }
    }

    private void Move()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
           
        }
        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);

       

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        transform.LookAt(transform.position +moveDirection) ;

        // Smoothly rotate the character towards the move direction
        if (moveDirection != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }

            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }

            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }


         moveDirection *= moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }



    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }
    private void Jump()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // Jump if the character is on the ground and the jump button is pressed
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);

        }

        // Apply gravity to the character controller
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
    }
    private void Attack2()
    {
        anim.SetTrigger("Attack2");
    }


    IEnumerator ResetAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        playerIsAttacking = false;
    }


    /**/
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, groundCheckDistance);
    }
    /**/
}
