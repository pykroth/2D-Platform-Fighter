using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //Instance variables for movement
    public float movespeed = 3f;
    public float jumpForce = 750f;
    public int jumpMax = 2;
    public int jumpCurrent = 2;
    private Vector2 movement = new Vector2();
    public bool teleport = false;
    public int loop1 = 0;
    public float gravity;

    private float moveModifier = 1.0f;
    private float teleportTimer = 0.0f;
    public GameObject teleportPrefab;
    //Checking location
    private bool facingRight = true;
    public bool grounded = false;
    public bool groundCheck = false;

    //Health
    public HealthBar_Player1 hpScript;
    public float healthMax = 100.0f;
    public float healthCurrent = 100.0f;
    public float damage = 0;
    public float healthObject = 0.0f;

    //Death
    public GameObject GameOver;
    public Text gameOverText;

    //Colliders and stuff
    private Rigidbody2D character;
    private Animator anim;
    private Transform trans;
    public GameObject Player1;
    public GameObject Shield;
    private Vector2 toMouse;

    //Shield stuff
    public Shield script;
    public bool isShielding = false;
    public float shieldCooldown = 0.0f;
    public float shieldLast = 3.0f;
    public bool allowShielding = false;
    public GameObject shieldBarPrefab;
    public GameObject ShieldBar;
    public GameObject uiCanvas;
    public float shieldLength = 20.0f;
    public ShieldBar shieldBarScript;

    //Stunned
    public bool isStunned = false;
    public float stunLength = 20.0f;

    //Attacks
    public bool isNotHit = false;
    public float playerDamage;
    public bool playerHitting = false;

    //Side Specials
    public float SideSpecialCooldown = 20.0f;
    public bool sideSpecialActive = false;
    public bool isActiveSide = false;

    //Down Special
    public float downSpecialActivation;
    public bool characterSwitch = false;
    public bool isSwitching = false;
    public GameObject playerSwitchForm;

    //Up Special
    public float holdUpSpecialTime = 5.0f;
    public float tempUpBuff = 10.0f;
    public bool isUpSpecial = false;
    public float upSpecialCoolDown = 2.0f;
    
    //Neutral Special
    public int isNeutralSpecial = 0;
    public float neutralCoolDown = 0.0f;
    public float neutralChargeTime = 0.0f;
    public bool isNeutralSpecialAttacking = false;
    public GameObject PlayerBlast;
    public bool endAnimation = false;
    public float attackCooldown = 2.0f;
    public bool fixNeutralSpecial = false;    

    //Player Combat
    public float continuedHit = 1.0f;
    public int combat = 0;
    public GameObject hurtBoxPrefab;
    public GameObject tempHurtBox;
 
    public int tempButton = 0;
    public bool ifButton = false;
    public float cantSpam = 1.0f;

    //Getting Hit
    protected float hitFlashTicker = -0.1f;
    protected float hitFlashDuration = -0.1f;
    protected float isStunnedTicker = 0.0f;
    protected bool playerisStunned = false;

    //Figures out the attack
    public int attackInput = 0;
    //If input = 1, punch, if input = 2, kick , if input = 3, forward air is inputted, if input = 4, down air is inputted, if input = 5, neutral special. If input = 6, up air will be inputted If shield is not active, none of these inputs will cause damage.
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        trans = GetComponent<Transform>();
        toMouse = new Vector2(0, 0);
         uiCanvas = GameObject.Find("UI");
        gravity = character.gravityScale;
        healthCurrent = GameObject.FindWithTag("HealthUI").GetComponent<ValueManager>().getCurrentHealth();
        Player1 = GameObject.FindWithTag("Player");
        hpScript = GameObject.FindWithTag("Player1HP").GetComponent<HealthBar_Player1>();

    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log("q");
        //Get Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        //Debug.Log(continuedHit);
        if(Input.GetKeyUp("q"))
        {
              healthCurrent -= 20.0f;
            hpScript.ChangeFill(healthCurrent, healthMax);
            //Debug.Log("q");

       }
       // Debug.Log(attackCooldown);
        death();
        isNotAttacking();
        isFacingRight();
        teleportTech();
        jumpTech();
        playerShield();
        UpdateAnimations();
        //   isNeutralAttacking();
        //Debug.Log(grounded);

        playerStunned();
        playerSideSpecial();
        playerDownSpecial();
       // Debug.Log(downSpecialActivation);
        playerNeutralSpecial();
        playerCombat();
        playerForwardAir();
        playerDownAir();
        playerUpSpecial();
        playerUpAir();
    }


    public void isNotAttacking()
    {
        anim.SetBool("DefaultState", true);
    }
    public void initalAnimation()
    {
        anim.Play("Player1_InitialState");
    }
    public void FixedUpdate()
    {
        if (true && isShielding == false && isStunned == false && endAnimation == false && isSwitching == false)
        {
            if (movement.magnitude > 1)
                movement.Normalize();

            Move(movement * movespeed * moveModifier);
        }


    }


    public void isNeutralAttacking()
    {

    }

    private void Move(Vector2 move)
    {
        float temp = Mathf.Abs(movement.x);
        if (isNeutralSpecialAttacking == false)
        character.velocity = new Vector2(move.x * 4.0f, character.velocity.y);
        else
            character.velocity = new Vector2(move.x * 1.0f, character.velocity.y);

        if (Mathf.Abs(character.velocity.x) > movespeed && isActiveSide != true)
        {
            character.velocity = new Vector2(Mathf.Sign(character.velocity.x) * movespeed, character.velocity.y);
        }
        if(Mathf.Abs(character.velocity.x) > movespeed && isActiveSide == true)
        {
            
            character.velocity = new Vector2(Mathf.Sign(temp) * movespeed, character.velocity.y);
        }
      


    }
    private void teleportTech()
    {
        if (Input.GetKeyDown("l") && teleportTimer <= 0)
        {
            anim.Play("Player1_Teleport");

            if (facingRight == true)
            {
                character.transform.position = new Vector2(character.position.x + 5.0f, character.position.y);

            }
            else
            {
                character.transform.position = new Vector2(character.position.x - 5.0f, character.position.y);
            }
            teleportTimer = 5.0f;

        }
        if (teleportTimer >= 0)
            teleportTimer -= Time.deltaTime;

        teleport = false;
    }

    public void isFacingRight()
    {
        if (movement.x > 0)
        {
            facingRight = true;
            anim.SetBool("isFacingRight", true);
        }
        else if (movement.x < 0)
        {
            facingRight = false;
            anim.SetBool("isFacingRight", false);
        }
    }

    private void jumpTech()
    {
        if (Input.GetKeyDown("space") && jumpCurrent > 0 && isStunned == false)
        {

            if (facingRight == true)
                anim.Play("Player1_Jump");
            else if (facingRight == false)
                anim.Play("Player1_JumpLeft");

            jumpCurrent--;
            grounded = false;
            character.velocity = new Vector2(character.velocity.x, 0);
            character.AddForce(new Vector2(0.0f, jumpForce));
            character.gravityScale = gravity;
            isActiveSide = false;
        }
        if (Input.GetKeyDown("space") && jumpCurrent <= 0 && loop1 == 1 && isStunned == false)
        {
            if (facingRight == true)
                anim.Play("Player1_Flip");
            else if (facingRight == false)
                anim.Play("Player1_FlipLeft");

            loop1 = 0;
        }

    }
    private void playerShield()
    {
        if (Input.GetKey("f") && grounded == true && shieldCooldown <= 0.0f && shieldLast > 0.0f && isStunned == false)
        {
            script.shielded(true);
            isShielding = true;
            Move(new Vector2(0, 0));
            shieldLast -= Time.deltaTime;


        }
        else
        {
            isShielding = false;
            script.shielded(false);
        }
        if (Input.GetKeyUp("f") && grounded == true && shieldCooldown <= 0.0f)
        {
            shieldCooldown = 2.0f;
        }
        if (shieldCooldown >= 0 && isShielding == false)
        {
            shieldCooldown -= Time.deltaTime;
        }
        if (isShielding == false && shieldLast <= 3.0f)
        {
            shieldLast += Time.deltaTime;
        }
        if (isShielding == true && Input.GetKeyDown("f"))
        {
            ShieldBar = Instantiate(shieldBarPrefab);
            ShieldBar.GetComponentInChildren<ShieldBar>().Setup(this.gameObject, uiCanvas.GetComponent<RectTransform>());
            //ShieldBar.GetComponent<ShieldBar>().Setup(this.gameObject);
            ShieldBar.GetComponentInChildren<ShieldBar>().ChangeFill(0);
            ShieldBar.transform.SetParent(uiCanvas.GetComponent<RectTransform>(), false);
            //shieldBarScript.showImages(true);


        }
        if (isShielding == true)
        {
            ShieldBar.GetComponentInChildren<ShieldBar>().ChangeFill(shieldLast, shieldLength);
        }
        if (isShielding == false)
        {
            if (ShieldBar != null)
                Destroy(ShieldBar);
        }
        if (shieldLast <= 0.0f && isShielding == true)
        {
            isStunned = true;

        }


    }
    //Combats
    public void playerCombat()
    {
        if (Input.GetKeyUp("j") && playerHitting != true && tempButton == 0 && grounded == true)

        {
            if(facingRight == true)
            anim.Play("Player1_PunchRight");
          else if (facingRight == false)
            anim.Play("Player1_PunchLEft");

            damage = 2.0f;


            attackInput = 1;
            

        }
        if (Input.GetKeyUp("j") && tempButton == 1 && continuedHit > .4f && continuedHit <= 1.0f && grounded == true)
        {
            if (facingRight == true)
            {
                anim.Play("Player1_Kick");
            }
            else if (facingRight == false)
            {
                anim.Play("Player1_KickLeft");
            }
            damage = 4.0f; 
            attackInput = 2;
        }

        if (Input.GetKeyUp("j"))
        {
            ifButton = true;
        }
        if(ifButton == true)
        {
            continuedHit -= Time.deltaTime;
           
        }
        if (continuedHit <= .38)
        {
            ifButton = false;
            tempButton = 0;
            continuedHit = 1.0f;
        }
        
    }
        public void playerForwardAir()
    {
        if (Input.GetKeyUp("j") && playerHitting != true  && grounded == false && movement.x >= .1 && facingRight == true)
        {
            damage = 4.0f;
            playerHitting = true;
            anim.Play("Player1_FowardAir");
            character.AddForce(new Vector2(20.0f, 20.0f));
            attackInput = 3;


        }
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && movement.x <= -.1 && facingRight == false)
        {
            damage = 4.0f;
            playerHitting = true;
            anim.Play("Player1_ForwardAir_Left");
            character.AddForce(new Vector2(-20.0f, -20.0f));
            attackInput = 3;

        }

    }//end playerForwardAir()
    public void playerDownAir()
    {
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == false && Input.GetKey("s") && facingRight == true)
        {
            damage = 5.0f;
            playerHitting = true;
            anim.Play("Player1_UpAir");
            attackInput = 4;
            destroyHitBox();

        }
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && Input.GetKey("s") && facingRight == false)
        {
            damage = 5.0f;
            playerHitting = true;
            anim.Play("Player1_UpAirLeft");
            attackInput = 4;

            destroyHitBox();

        }
       

    }//end playerDownAir()

    public void playerUpAir()
    {
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == false && Input.GetKey("w") && facingRight == true)
        {
            damage = 6.0f;
            playerHitting = true;
            anim.Play("Player1_DownAir");
            attackInput = 6;


        }
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == false && Input.GetKey("w") && facingRight == false)
        {
            damage = 6.0f;
            playerHitting = true;
            anim.Play("Player1_DownAirLeft");
            attackInput = 6;


        }
    }//end playerUpAir()
    public void spawnDownAirHitBox()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-.3f, .5f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 1.5f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }//end upAiHitBox
    public void spawnDownAirHitBoxLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.3f, .5f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 1.5f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }//end upAiHitBox
    public void spawnUpAirHitBox()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(1.25f, 0.3f);
       GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.0f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);
          
    }//end upAiHitBox
    public void spawnLeftUpAirHitBox()
    {
        playerHitting = false;
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-1.25f, 0.3f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.0f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }//end upAiHitBox
    //Specials
    public void playerSideSpecial()
    {
        /*
        if (Input.GetKeyUp("k") && movement.x > 0 && sideSpecialActive == false && playerHitting == false)
        {
            character.AddForce(new Vector2(20000.0f, 1000.0f));
            anim.Play("Player1_SideSpecialRightGrounded");
            // Debug.Log("q");
            character.gravityScale = .5f;
            sideSpecialActive = true;
            isActiveSide = true;
            playerHitting = true;
        }
        else
            playerHitting = false;
            if(SideSpecialCooldown  <= 20.0f )
        {
            
            SideSpecialCooldown -= Time.deltaTime;
        }
            if(SideSpecialCooldown <= 0 || grounded == true)
        {
            sideSpecialActive = false;
            SideSpecialCooldown = 20.0f;
        }
            */
        if (Input.GetKey("k") && playerHitting != true && movement.x!= 0)
        {
           
            isNeutralSpecialAttacking = true;
     
         

        }//end playerHitting

        if (Input.GetKey("k") && movement.x != 0 && isNeutralSpecialAttacking == true)
        {
            isNeutralSpecial = 1;
            playerHitting = true;
        }
       // if (Input.GetKey("k") && Input.GetKey("d"))
       // {//
           // isNeutralSpecial = 1;
        //}


    }
    public void playerDownSpecial()
    {
        if (Input.GetKey("s") && Input.GetKey("k") && grounded == true )
        {
            downSpecialActivation += Time.deltaTime;
            isSwitching = true;
            playerHitting = true;

        }
        else
        {
            isSwitching = false;
            playerHitting = false;
        }
        if(downSpecialActivation >= 0.0f && isSwitching == false)
        {
            downSpecialActivation -= Time.deltaTime;
        }

            if (downSpecialActivation >=3.0f)
        {
            if (facingRight == true)
            {
                anim.Play("Player1_DownSpecial_SwitchingForms");
                downSpecialActivation = 0.0f;
            }
            else if(facingRight == false)
            {
                anim.Play("Player1_DownSpecial_SwitchingFormsLeft");
                downSpecialActivation = 0.0f;
            }
        }
     
    }
    public void playerUpSpecial()
    {
       
        if (Input.GetKey("w") && Input.GetKey("k") && grounded == true && holdUpSpecialTime >= 0.0f && playerHitting != true)
        {
            holdUpSpecialTime -= Time.deltaTime;
            tempUpBuff = 10.0f;
            movespeed = 2.5f;
            isUpSpecial = true;
            
            upSpecialCoolDown = 15.0f;
        }
        else if (!Input.GetKey("w") && !Input.GetKey("k") )
             isUpSpecial = false;

        if (holdUpSpecialTime<= 0)
        {
           
            isUpSpecial = false;
            upSpecialCoolDown -= Time.deltaTime;
            
        }
        if (holdUpSpecialTime <= 0 && upSpecialCoolDown <=0)
        {
            isUpSpecial = false;
            holdUpSpecialTime = 5.0f;

        }
        if (tempUpBuff <= 11.0f && holdUpSpecialTime <= 0)
        {
            movespeed = 4.0f;
            tempUpBuff -= Time.deltaTime;
            Player1.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if(tempUpBuff<=0)
        {
            movespeed = 2.0f;
            Player1.GetComponent<SpriteRenderer>().color = Color.white;

        }

}//Buff //playerUpSpecial
    
    
    
        public void playerNeutralSpecial()
    {
        if (playerHitting == false && Input.GetKey("k") && movement.x == 0 && !Input.GetKey("w") && attackCooldown <= 0.0f)
        {
            if(facingRight == true)
            anim.Play("Player1_NeutralSpecialRock");
            else if(facingRight == false)
            anim.Play("Player1_NeutralSpecialRock_Left");
            endAnimation = true;
            playerHitting = true;
            character.velocity = new Vector2(0, 0);
            damage = 10.0f;
            fixNeutralSpecial = false;
            attackInput = 5;


        }
        if (attackCooldown >= 0.0)
        {

            attackCooldown -= Time.deltaTime;
        }
        if (!Input.GetKey("k") && !Input.GetKey("w"))
        {
            endAnimation = false;
        }
            if(fixNeutralSpecial == true && attackCooldown >=0.0f)
        {
           
        }
        
    }//end NeutralSpecial()
    
    public void playerNeutralRock()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.Find("Player1").transform;
        playerHitting = false;
     
       


    }//end playerNeutralRock()
    public void playerNeutralRockLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player").transform;
        playerHitting = false;
        
    }

    public void neutralProjectile()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset * .7f;
        colliderOffset += new Vector2(.2f, 0f) ;
        GameObject tempSwordSlash = Instantiate(PlayerBlast, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        playerHitting = false;
       
    }
    public void neutralProjectileLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset * .7f;
        colliderOffset += new Vector2(-.2f, 0f);
        GameObject tempSwordSlash = Instantiate(PlayerBlast, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 180));
        playerHitting = false;
    }

    public void switchForms()
    {
        Destroy(this.gameObject);
        Instantiate(playerSwitchForm, transform.position, Quaternion.Euler(0, 0, 0));
    

    }
  
    public void Ground()
    {
        grounded = true;
        jumpCurrent = 2;
    }


    public void destroyHitBox()
    {
        // if( continuedHit <= 0.0f)
        //{
        //  combat = 0;
        //}
        GameObject[] things = GameObject.FindGameObjectsWithTag("DamageHurtBox");
       // GameObject temp = GameObject.FindGameObjectWithTag("DamageHurtBox");
      for(int i = 0; i<things.Length; i++)
        {
            Destroy(things[i]);
            
        }

        playerHitting = false;
        if(tempButton == 0)
            tempButton = 1;
        attackCooldown = 2.0f; 


    }
    public void tempButtonReset()
    {
        tempButton = 0;
    }
    public void resetCharge()
    {
       
            neutralChargeTime = 2.0f;
            isNeutralSpecial = 0;
            isNeutralSpecialAttacking = false;
            playerHitting = false;
        
    }
        
   public void playerStunned()
    {
      
       
        if (stunLength >= 0.0f && isStunned == true)
        {
            stunLength -= Time.deltaTime;
        }
        if (stunLength <= 0.0)
        {
            stunLength = 20.0f;
            isStunned = false;
        }
    
    }
  
    public void createHurtBoxforPunchRight()
    {//creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.5f, 0.0f);
      
        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player").transform;
        playerHitting = false;

        tempButton = 1;
    }

    public void createHurtBoxForPunchLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-.5f, 0.0f);
   
        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player").transform;
        tempButton = 1;
      
        playerHitting = false;
    }

    public void death()
    {//Kills the player when HP hits 0
        if(healthCurrent <= 0)
        {
            Destroy(this.gameObject);
            Text textTemp = Instantiate(gameOverText);
            textTemp.transform.SetParent(GameObject.Find("UI").GetComponent<RectTransform>(), false);
        }
        
    }
    public float getHPCurrent()
    {
        return healthCurrent;
    }
    public void changeButton()
    {
        
    }
    private void UpdateAnimations()
    {
        //Send movement Info to Animator
        if (isShielding == false )
        {
            anim.SetFloat("Horizontal", movement.x);

            anim.SetFloat("Horizontal", 0);
        }
        if(isShielding == true)
        {
            anim.SetFloat("Velocity", 0);
        }
        float speed = movement.magnitude; //Slightly faster: movement.sqrMagnitude
        if (grounded == true && isShielding ==false)
        {
            loop1 = 1;
            anim.SetInteger("FlipLoop", 1);
            anim.SetFloat("Velocity", speed);
        }
        else if (grounded == false && isShielding ==false)
            anim.SetFloat("Velocity", 0);

        if (grounded == true)
            anim.SetBool("Grounded", true);
        else if(grounded == false)
                anim.SetBool("Grounded", false);

        anim.SetBool("Shielding", isShielding);
        //Face the Mouse
        //If not moving or attacking, face the mouse
        anim.SetBool("Stunned", isStunned);

        anim.SetBool("Switching", isSwitching);

        anim.SetInteger("NeutralSpecial", isNeutralSpecial);

        anim.SetInteger("Combat", 0);

        anim.SetBool("endNeutralSpecialRock", endAnimation);
        anim.SetBool("upSpecial", isUpSpecial);
    } //end UpdateAnimations


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
    public int getInput()
    {
       return attackInput;
    }
    public float getDamage()
    {
        return damage;
    }
    public bool getFacingRight()
    {
        return facingRight;
    }
}//end everything

