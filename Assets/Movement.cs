using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [Header("Movement")]
    private float movx = 0f;
    [SerializeField] private float movspeed;
    [SerializeField] private float movsmooth;
    private Vector3 speed = Vector3.zero;
    private bool lookingRight = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask isFloor;
    [SerializeField] private Transform FloorController;
    [SerializeField] private Vector3 boxDimension;
    [SerializeField] private bool onFloor;
    private bool jumping = false;
    [SerializeField] private int extraJumps;
    [SerializeField] private int totalExtraJumps;
    [SerializeField] private int dashes;
    private bool dashing = false;
    [SerializeField] float dashForce;
    private float dashCooldown = 2f; // 2 seconds cooldown
    private float dashTime;

    //[Header("Animation")]
    //private Animator animator;

    [Header("TimeTravel")]
    [SerializeField] private bool terrenoCounter = true;
    [SerializeField] private int teleportDistance; // Teleportation distance (3 units on the y-axis)

     // **Key changed to Fire2**



    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
       //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movx = Input.GetAxisRaw("Horizontal") * movspeed;

        //animator.SetFloat("Horizontal", MathF.Abs(movx));

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true; 
        }

        if (onFloor)
        {
            extraJumps = totalExtraJumps;

        }
        if (Input.GetButtonDown("Fire2") && dashes>0)
        {
            dashing = true;
        }
        if (terrenoCounter == true && Input.GetButtonDown("Fire1"))
        {
            // Teleport to Terrain 2
            terrenoCounter = false;
            transform.position = transform.position + new Vector3(0,-34f);
        }
        else if (terrenoCounter == false && Input.GetButtonDown("Fire1"))
        {
            // Teleport to Terrain 1
            terrenoCounter = true;
            transform.position = transform.position + new Vector3(0,34f);
        }
    }

    private void FixedUpdate()
    {
        onFloor = Physics2D.OverlapBox(FloorController.position, boxDimension, 0f, isFloor);
        Move(movx * Time.fixedDeltaTime);
        jumping = false;
        //animator.SetBool("OnFloor", onFloor);
        //animator.SetFloat("ExtraJumps", extraJumps);
    }

    private void Move(float move)
    {
        Vector3 objspeed = new Vector2(move, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, objspeed, ref speed, movsmooth);
        
        if(move>0 && !lookingRight) 
        {
            Turn();
        }
        else if (move<0 && lookingRight)
        {
            Turn();
        }

        if (jumping)
        {
            if (onFloor)
            {
                Jump();
            }
            else if (jumping && extraJumps > 0)
            {
                
                Jump();
                extraJumps = extraJumps-1;
            }
        }
        if (dashing && dashes>0)
        {
            Dash();
            dashing = false;
        }
    }

    private void Turn()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(FloorController.position, boxDimension);
    }

    private void Jump()
    {
        
        rb2D.velocity = new Vector2((rb2D.velocity.x), jumpForce);
        jumping = false;
    }
    private async void Dash()
    {
        if (dashing && dashes > 0)
        {
            if (lookingRight)
            {
                rb2D.velocity = new Vector2(dashForce, 0f);
            }
            else
            {
                rb2D.velocity = new Vector2(-dashForce, 0f);
            }

            dashes--;
            dashing = false;
            dashTime = Time.time; // Inicia el cooldown timer

            // Espera la recarga del dash de forma asincrónica
            await Task.Delay((int)(dashCooldown * 1000)); // Milisegundos

            // Repone un dash después del cooldown
            dashes++;
        }
    }
    
}
