using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //Instance Variables

    //Variables for Movement
    public float movementSpeed = 3.0f; // The initial speed
    private Vector2 movement = new Vector2(); // Vector: X will add movement. Y = for jump, but force should be used
    private float moveModifier = 1.0f; // Modifier for the speed
    public float tempVelocity = 0; //Velocity that allows other classes to gain access to it

    //Variables for jumping
    public float jumpForce = 5.0f;
    public int jumpUses = 0;
    public float jumpMax = 1;
    public float jumpRelease = 0.0f; // Variable that = jump pressed length when its released
    public float jumpPressedLength = 0.0f; // Variable that is how long the user presses it 
    public float gravity;

    //Variables for the airDash() mechanic.
    private float dashLength = .2f;
    public float startDashTime = .2f;
    private Vector2 direction = new Vector2();
    public float dashSpeed = 3.0f;
    public bool isDashing = false;
    public float yPriorVelocity = 0.0f;
    public float freezeLength = .09f;
    //Get Components
    public Rigidbody2D character;
    //Location Check
    public bool isGrounded;

    //Health


    //Death 

    //Collider


    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<Rigidbody2D>();
        gravity = character.gravityScale;
       
    }
   

    // Update is called once per frame
    public virtual void Update()
    {

       

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
     
        Jump();
        airDash();
        Debug.Log(isDashing);
        if(jumpUses >=4)
        Debug.Log(jumpUses);
        //   Debug.Log(isGrounded);

        
    }
    public virtual void FixedUpdate()
    {

        //Debug.Log("this runs");
        


            if (movement.magnitude > 1)
                movement.Normalize();

            Move(movement * movementSpeed * moveModifier);

            tempVelocity = movement.x;

        

    }

    #region Code For Moving
    //Code for moving
    protected void Move(Vector2 move)
    {
        //Debug.Log("this runs");
        //if (Mathf.Abs(character.velocity.x) > movementSpeed)
       // {
         //   character.velocity = new Vector2(Mathf.Sign(character.velocity.x) * movementSpeed, character.velocity.y);
        //}
       // else
            character.velocity = new Vector2(move.x * 1.0f, character.velocity.y);
    }
    //Code to jump
    protected void Jump()
    {
        //Debug.Log(jumpPressedLength);
        //This code sets jump release = JumpPressedlength. Done so that it =s when its released
        if (Input.GetButtonUp("Jump"))
        {
            jumpRelease = jumpPressedLength;
        }
      
        //This test how long the user presses jump
        if (Input.GetButton("Jump"))
        {
            jumpPressedLength += Time.deltaTime;
        }
        else
            jumpPressedLength = 0.0f;
       
        if(jumpPressedLength >= .07f)
        {
            jumpRelease = .08f;
        }
        //This code does the jump input
        if (jumpRelease >= .05f && jumpUses <= jumpMax )
        {
            //Jump Cancel For Dash
         
            isGrounded = false;
       
            character.velocity = new Vector2(movement.x, jumpForce);
            jumpUses += 1;
            character.gravityScale = gravity;
            jumpPressedLength = 0.0f;
            jumpRelease = 0.0f;
        }
        else if (jumpRelease > 0.0f && jumpRelease < .05f && jumpUses <= jumpMax )
        {
            //Jump Cancel For Dash
          
            isGrounded = false;

            character.velocity = new Vector2(movement.x, jumpForce / 1.25f);
            jumpUses += 1;
            character.gravityScale = gravity;
            jumpPressedLength = 0.0f;
            jumpRelease = 0.0f;
        }
        jumpRelease = 0.0f;
      /*
        if (Input.GetButtonDown("Jump") && jumpUses <= jumpMax)
        {
            Debug.Log("Works");
            character.velocity = new Vector2(character.velocity.x, 0);
            character.AddForce(new Vector2(0.0f, jumpForce));
            jumpUses += 1;
            character.gravityScale = gravity;
        }
       */
    }

    //AirDash will be a technique that allows the user to dash in the air. 
    // Step 1 --> Add a force based on the users input of direction. Calculation of it will be difficult. Step 2 --> Instantiate a prefab that would exist that showcases it. Step 3 --> Have an animation set for the characters when they airdash
    protected void airDash()
    {
      
        //Sets dashing = true
            if (Input.GetKeyUp("p"))
            {
            isDashing = true;
            direction.x = movement.x;
            direction.y = movement.y;
            }

        if (isDashing == true)
        {
            if (dashLength > 0)
            {
                int tempCount = 0;
                if(tempCount != 1)
                {

                    tempCount++;
                    yPriorVelocity = character.velocity.y;
                }
                character.velocity = new Vector2(direction.x * 30, direction.y * 30);
                Debug.Log("works");
                dashLength -= Time.deltaTime;
            }
        }
        if(dashLength <= 0)
        {
            isDashing = false;
            //character.velocity = new Vector2(0, 0);
            if(freezeLength >0)
            {
                freezeLength -=Time.deltaTime;
            }
            if(freezeLength<= 0)
            character.velocity = new Vector2(0, yPriorVelocity);
        }
        if (isGrounded == true)
        {
            isDashing = false;
            dashLength = startDashTime;
            movement.y = movement.y / 20.0f;
        }
        
    }
    #endregion
    #region Location
    public void checkifGrounded()
    {
        isGrounded = true;
        jumpUses = 0;
       
    }
    #endregion
}
