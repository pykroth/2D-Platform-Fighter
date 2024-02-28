using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBagScript : MonoBehaviour
{
    //Fixing rotation
    public GameObject SandBag;
    public bool grounded = true;
    public float rotationZ;
    public float rotationX;
    public float rotationY;

    //Health
    public HealthBar_Player2 hpScript;
    public float healthLength = 100.0f;
    public float healthCurrent = 100.0f;

    //Values
    private Rigidbody2D character;

    //Getting Hit
    protected float hitFlashTicker = -0.1f;
    protected float hitFlashDuration = -0.1f;
    protected float isStunnedTicker = 0.0f;
    protected bool playerisStunned = false;
    private Animator anim;
    public bool wasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rotationZ = 0.0f;
        hpScript = GameObject.FindWithTag("Player2HP").GetComponent<HealthBar_Player2>();
        character = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rotationZ = SandBag.GetComponent<Transform>().rotation.z;
        if(rotationZ != 0)
        {
            rotationZ -= 1.0f;
        }
        anim.SetInteger("angleZ", (int)rotationZ);

        UpdateStunnedState();
        UpdateHitFlash();
        updateHealth();
        if(Input.GetKeyUp("q"))
        {
            healthCurrent -= 20.0f;
        }
        if(healthCurrent <= 0)
        {
            sandBagDestroy();
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("l");
            grounded = true;

       
        }
        //else
         //   grounded = false;
    } //end OnTriggerEnter2D
    public void fixAngle()
    {
        rotationZ = SandBag.GetComponent<Transform>().rotation.z;
        if (rotationZ > 15.0f || rotationZ <= 15.0f && grounded == true)
            SandBag.GetComponent<Transform>().rotation = Quaternion.Euler(180, 180, 180);
    }

    //Getting Hit
    public void Hit(float incomingDamage, Vector2 forceDirection)
    {
        //Subtract health (we'll check for death in update())
        healthCurrent -= incomingDamage;

        //Apply the knockback force
        character.AddForce(forceDirection, ForceMode2D.Impulse);

        //Make it stunned and flashy
        playerisStunned = true;
        isStunnedTicker = forceDirection.magnitude / 20.0f; //About 1/2 of the knockback value in seconds

        //Maybe we should calculate the duration instead of just 3.0 seconds of flashing?
        hitFlashDuration = 3.0f;


        //Allow Rebound
        wasHit = true;




    }  //end Hit()
    protected void UpdateHitFlash()
    {
        if (hitFlashDuration >= 0)
        {
            //Decrease time from the overall duration of flashing
            hitFlashDuration -= Time.deltaTime;
            //Decrease time from the interval until we change from red to white or vice versa
            hitFlashTicker -= Time.deltaTime;

            //Done Flashing
            if (hitFlashDuration < 0)
            {

                hitFlashTicker = -0.1f;
                return; //end the method immediately
            }

            //Change Color


        } //end if hitFlashDuration >= 0

    } //end UpdateHitFlash()
    protected void UpdateStunnedState()
    {
        //Countdown the stun timer
        if (isStunnedTicker >= 0)
            isStunnedTicker -= Time.deltaTime;

        //Unstun if either the velocity reaches 0 OR stunned timer is up
        if (playerisStunned && (character.velocity.magnitude < 0.1 || isStunnedTicker < 0))
        {
            //Disable stunned state and stop any movement from the knockback
            playerisStunned = false;
            character.velocity = Vector2.zero;

            //Stop the flashing
            hitFlashDuration = 0;


        }
    } //end UpdateStunnedState()
   
    public void updateHealth()
    {
       hpScript.ChangeFill(healthCurrent, healthLength);
    }//end updateHealth()

    public void sandBagDestroy()
    {
        Destroy(this.gameObject);
    }

  public void checkIfHit()
    {
        
    }//end checkIfHit()
}//end everything


