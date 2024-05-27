using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Player Vars
    private Rigidbody2D player_rb;

    [Header("Movement")]
    //Player Movement Vars
    public float movementSpeed = 10.0f;
    private float movementInputDirection;
    private int facingDirection = 1;
    private float turnTimer;
    public float turnTimerSet = 0.1f;

    [Header("Jump")]
    //Player Jump Vars
    public float jumpForce = 16.0f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool canNormalJump;
    private bool canWallJump;
    public int amountOfJump = 1;//This is teh var that indicate how many jumps the player can di, if you wants more, just up the value
    private int amountOfJumpsLeft;
    public float variableJumpHeightMultiplier = 0.5f;
    private float jumpTimer;
    public float jumpTimerSet = 0.15f;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;

    [Header("WallSlide")]
    //Player Walls Slide Vars
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isTouchingWall;
    private bool isWallSliding;
    public float wallSlideSpeed;
    
    [Header("WallJump")]
    //Player Wall Jump
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    public float wallHopForce;
    public float wallJumpForce;
    private float wallJumpTimer;
    public float wallJumpTimerSet = 0.5f;
    private bool hasWallJump;
    private int lastWallJumpDirection;

    [Header("Air Movement")]
    //Player Air Movement
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    
    //Player Animations
    private Animator player_anim;
    private bool isWalking; 
    private bool isGrounded;

    //Player Facing direction
    private bool isFacingRight = true;//The charcater initialize look to right direction

    //Start
    private void Start()
    {
        //Initialice Vars
        //player_rb = GetComponent<Rigidbody2D>();
        //player_anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJump;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    //Awake
    private void Awake()//Ask Evan
    {
        //Initialice Vars
        player_rb = GetComponent<Rigidbody2D>();
        player_anim = GetComponent<Animator>();
    }

    //Update
    private void Update()
    {
        //Check for de used Inputs
        CheckInput();
        //Check the direction the player is Facing
        CheckMovementDirection();
        //Update the animations depending the player actions
        UpdateAnimations();
        //Check if player con jump
        CheckIfCanJump();
        //Check if player can slide in wall
        CheckIfWallSliding();
        //Check if Player Jump
        CheckJump();
    }

    //FixedUpdate
    private void FixedUpdate()
    {
        //Applaymovement in the FixedUpdate to avoid errors
        ApplyMovement();
        //CheckSurroundings in the FixedUpdate to avoid erros in the frames
        CheckSurroundings();
    }

    //Function CheckIfWallSliding
    private void CheckIfWallSliding()
    {
        //Condition that if the raycast activate the bool of a wall, the player is not on the ground and the velocity in axe "y" is negative, the we can said that is wall sliding
        if(isTouchingWall && movementInputDirection == facingDirection && player_rb.velocity.y < 0)
        {
            isWallSliding = true;
        }   
        else
        {
            isWallSliding = false;
        }
    }

    //Function CheckSurroundings
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);//This is gona check if the player is in the ground and determinate what is ground, using the postion of the CheckGround gameObject in the player, a radius and the objects that select like a proper ground

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);//Use a RayCast to detect if a wall is enough close to slide
    }

    //Function CheckIfCanJump
    private void CheckIfCanJump()
    {
        //Condition that only player can jump if is touching ground and the velocity in axe Y is 0
        if(isGrounded && player_rb.velocity.y <= 0.01f)//Before was 0
        {
            amountOfJumpsLeft = amountOfJump;
        }

        //Condition to allow the wallJump
        if(isTouchingWall)
        {
            canWallJump = true;
        }
        
        //Can only jump depending on the amount of Jumps
        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    //Check MovementDirection
    private void CheckMovementDirection()
    {
        //Condition to call de function "Flip" depending in the direction the player is going
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        //Condition that know if the player is walking or not
        if(player_rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    //Function UpdateAnimations
    private void UpdateAnimations()
    {
        player_anim.SetBool("isWalking", player_rb.velocity.x > 0.01f || player_rb.velocity.x < -0.01f);//Use the bool that is in the Animator and the bool here in the code to activate the animation
        // player_anim.SetBool("isWalking", isWalking);
        player_anim.SetBool("isGrounded", isGrounded);//Use the bool that is in the Animator and the bool here in the code to activate the animation
        player_anim.SetFloat("yVelocity", player_rb.velocity.y);//Use the float in the Animator and the player_rb.velocity.y to activate the animation
        player_anim.SetBool("isWallSliding", isWallSliding);//Use the bool in the script to activate the one in the Animator
    }

    //Function CheckInput
    private void CheckInput()
    {
        //Horizontal Movement
        movementInputDirection = Input.GetAxisRaw("Horizontal"); //Input for "A" and "D" Keys, GetAxisRaw is because y want round numbers

        //Jump Action-Condition
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || amountOfJumpsLeft > 0 && isTouchingWall)
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        //Condition that is ging to make a really little amoount of time to the player to do the wall jump, this we be allow by freezing the movement in a short time, and if detects a inputs it will do teh wall jump
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        //Condition thats take track of the timer thats allow move
        if(!canMove)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        //Condition taht determinate the moment we stop pressing the jump boton(in this case spacebar), with this change how hiegh player jump
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            player_rb.velocity = new Vector2(player_rb.velocity.x, player_rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    //Function Jump
    private void CheckJump()
    {
        //Condition that determinate what kind of jump the player is going to do
        if (jumpTimer > 0)
        {
            if(!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)//WallJump
            {
                WallJump();
            }
            else if(isGrounded)//Normal Jump
            {
                NormalJump();
            }
        }

        //Condition that determinate the time is jumping
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        //Cindition that it is still time left to do wall jump and the player select a direction diferent than up, can do the wall jump
        if(wallJumpTimer > 0)
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x, 0.0f);
            hasWallJump = false;
        }
        else if (wallJumpTimer <= 0)//Stop the oportunity to walljump
        {
            hasWallJump = false;
        }
        else//Deacrese the timer
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        //Condition to only jump once util you touch the ground
        if (canNormalJump)//Jump
        {
            //Jump Force
            player_rb.velocity = new Vector2(player_rb.velocity.y, jumpForce);
            amountOfJumpsLeft--;//This rest the amount of jump evrey time player jump
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        //Condition that allow player to do a wall jump depending in the situation
        if (canWallJump)//WallJump
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJump;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            player_rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJump = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }

    //Function ApplyMovement
    private void ApplyMovement()
    {
        //Condition
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x * airDragMultiplier, player_rb.velocity.y);
        }
        //Condition that only applay the movement to walk on the ground  --Maybe i change this--
        else if (canMove)
        {
            //Applay Velocity to the movements
            player_rb.velocity = new Vector2(movementSpeed * movementInputDirection, player_rb.velocity.y);
        }       
        
        
        //Condition that if is walSsliding the velocity of going down deacrease but not the same when is falling
        if(isWallSliding)
        {
            if (player_rb.velocity.y < -wallSlideSpeed)
            {
                player_rb.velocity = new Vector2(player_rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    //Function Flip
    private void Flip()
    {
        //Condition to avoid change the character face direction in the wall sliding
        if(!isWallSliding && canFlip)
        {
            facingDirection *= -1;//This will change 1 and -1 to flip the character
            isFacingRight = !isFacingRight;//Only changing if is diferent from his own
            transform.Rotate(0.0f, 180.0f, 0.0f);//This rotate the sprite, with this we dont need to create a diferent sprite looking to the other side
        }      
    }

    //OnDrawGizmos
    private void OnDrawGizmos()
    {
        //Sphere for detect Ground
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);//This wild draw a circle in the player that we help to calculate the ground

        //Line to detect walls
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));//This will Draw a line thats help to used like a RayCast, with this will detect a wall to know if can slide from a wall
    }
}
