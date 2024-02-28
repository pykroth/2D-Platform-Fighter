using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player_SwitchedForm : MonoBehaviour
{
    //Instance variables for movement
    public float movespeed = 3f;
    public float jumpForce = 750f;
    public int jumpMax = 2;
    public int jumpCurrent = 2;
    private Vector2 movement = new Vector2();
    private Vector2 upwardMovement = new Vector2(); 
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
    public float healthObject = 0.0f;
    //Damage
    public float damage = 0;
    //Death
    //public GameObject GameOver;
    //public Text gameOverText;

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
    public float shieldLength = 3.0f;
    public ShieldBar shieldBarScript;

    //Stunned
    public bool isStunned = false;
    public float stunLength = 20.0f;

    //Hitbox
    public GameObject hurtBoxPrefab;
    public int inputAttack = 0;
    //If input is 1, kick is initated, if input 2, punch is initated, if input 3, multipunch is initiated, if input 4, upSpecial is initiated, if input 5, forward special is initated, if input 6, neutralspecial is initated...
    //if input 7, foward air is inputted, if input 8, down air is inputted, if input 9, upair is inputted. Thats all i think


    //Health script
    public ValueManager healthValueScript;

    //Checking Animation
    public bool isSideSpecialAttacking = false;
    public bool animationPlaying = false;
    public bool playerHitting = false;



    //Down Special
    public float downSpecialActivation;
    public bool characterSwitch = false;
    public bool isSwitching = false;
    public GameObject Player_Switch;

    //Neutral Special
    public bool isNeutralSpecial = false;
    public float NeutralSpecialCharge = 0.0f;
    public bool neutralSpecialRelease = false;
    public float neutralSpecialCoolDown = 2.0f;

    //Combat
    public int combatLevel = 0;
    public float continueCombat = 1.0f;
    public bool firstHit = false;

    //Up Special
    public float upSpecialCoolDown = 3.0f;

  
    // Start is called before the first frame update
    void Start()
    {
        hpScript = GameObject.FindWithTag("Player1HP").GetComponent<HealthBar_Player1>();
        healthCurrent = GameObject.FindWithTag("HealthUI").GetComponent<ValueManager>().getCurrentHealth();
        character = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        trans = GetComponent<Transform>();
        toMouse = new Vector2(0, 0);
        uiCanvas = GameObject.Find("UI");
        gravity = character.gravityScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("q"))
        {
            healthCurrent -= 20.0f;
            hpScript.ChangeFill(healthCurrent, healthMax);
            //Debug.Log("q");
        }
        //Get Inputs
        upwardMovement.y = Input.GetAxisRaw("Vertical");
        movement.x = Input.GetAxisRaw("Horizontal");
        UpdateAnimations();
        isFacingRight();
        jumpTech();
        sideSpecial();
        playerShield();
        playerStunned();
        playerUpAir();
        playerDownSpecial();
        playerForwardAir();
        playerDownAir();
        playerNeutralSpecial();
        playerCombat();
        playerUpSpecial();
        // if(Input.GetKeyDown("p"))
        //  {
        //    anim.Play("Player1_Switched_UpAirLeft");
        // }
        //Debug.Log(jumpCurrent);
        //if (Input.GetKeyUp("q"))
        //   {
        //    healthCurrent -= 20.0f;
        //    hpScript.ChangeFill(healthCurrent, healthMax);
        //     Debug.Log("q");
        //  }
        //  if (Input.GetKeyUp("q"))
        //{
        //  anim.Play("Player1_Switched_ForwardAir");
        //}


    }

    public void FixedUpdate()
    {
        if (true && isShielding == false && isStunned == false && isSideSpecialAttacking == false && isNeutralSpecial == false)
        {
            if (movement.magnitude > 1)
                movement.Normalize();

            Move(movement * movespeed * moveModifier);
        }


    }
    //Movements: Jump, etc
    private void Move(Vector2 move)
    {
        float temp = Mathf.Abs(movement.x);


        character.velocity = new Vector2(move.x * 1.0f, character.velocity.y);

        if (Mathf.Abs(character.velocity.x) > movespeed)
        {

            character.velocity = new Vector2(Mathf.Sign(temp) * movespeed, character.velocity.y);
        }
    }

    private void jumpTech()
    {
        if (Input.GetKeyDown("space") && jumpCurrent > 0 && isStunned == false && isSideSpecialAttacking == false)
        {

            //if (facingRight == true)
            //  anim.Play("Player1_Jump");
            //else if (facingRight == false)
            //anim.Play("Player1_JumpLeft");

            jumpCurrent--;
            grounded = false;
            character.velocity = new Vector2(character.velocity.x, 0);
            character.AddForce(new Vector2(0.0f, jumpForce));
            character.gravityScale = gravity;
            // isActiveSide = false;
        }
        if (Input.GetKeyDown("space") && jumpCurrent <= 0 && loop1 == 1 && isStunned == false && isSideSpecialAttacking == false)
        {
            if (facingRight == true)
                anim.Play("Player1_Switched_FlipRight");
            else if (facingRight == false)
                anim.Play("Player1_Switched_Flip");

            loop1 = 0;
        }

    }
    //Player stunned
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
            uiCanvas = GameObject.Find("UI");
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
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == true && continueCombat >= 0.0f)
        {
            combatLevel += 1;
            firstHit = true;
        }
           if(firstHit == true)
        {
            continueCombat -= Time.deltaTime;

        }
           if(continueCombat <= 0)
        {
            continueCombat = 1.0f;
            firstHit = false;
            combatLevel = 0;
        }

           if(combatLevel >3)
        {
            combatLevel = 0;
        }
    }//end playerCombat()

    public void playerCombatRight()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 2.0f;
        inputAttack = 1;

    }//end playerCombatRight()
    public void playerCombatLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-0.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 2.0f;
        inputAttack = 1;

    }//end playerCombatRight()
    public void playerPunchRight()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 2.0f;
        inputAttack = 2;

    }//end playerCombatRight()
    public void playerPunchLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-0.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 2.0f;
        inputAttack = 2;

    }//end playerCombatRight()
    public void playerMultiHitPunch()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(0.75f, 0.0f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;


        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.0f;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 5.0f;
        inputAttack = 3;


    }//end playerCombatRight()

    public void playerMultiHitPunchLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-0.75f, 0.0f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;


        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.0f;
        playerHitting = false;
        continueCombat = 1.0f;
        damage = 5;
        inputAttack = 3;

    }//end playerCombatRight()
    public void playerUpAir()
    {
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == false && Input.GetKey("w") && facingRight == true)
        {
            damage = 10.0f;
            playerHitting = true;
            anim.Play("Player1_Switch_UpAir");

            inputAttack = 9;
        }
        if (Input.GetKeyDown("j") && playerHitting != true && grounded == false && Input.GetKey("w") && facingRight == false)
        {
            damage = 10.0f;
            playerHitting = true;
            anim.Play("Player1_Switched_UpAirLeft");
            inputAttack = 9;

        }


    }//end playerUpAir()
    public void spawnUpAirHitBox()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(0.0f, 1.5f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;


        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.0f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }//end upAirHitBox

    public void playerForwardAir()
    {
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && movement.x >= .1 && facingRight == true)
        {
            damage = 5.0f;

            anim.Play("Player1_Switched_ForwardAir");
            playerHitting = true;
            inputAttack = 7;



        }
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && movement.x <= -.1 && facingRight == false)
        {
            damage = 5.0f;
            playerHitting = true;
            anim.Play("Player1_Switched_ForwardAir_Left");
            inputAttack = 7;

        }

    }//end playerForwardAir()
    public void createHurtBoxforPunchRight()
    {//creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        playerHitting = false;

        //    tempButton = 1;
    }
    public void createHurtBoxForPunchLeft()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-.5f, 0.0f);

        GameObject tempHurtBox = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        tempHurtBox.transform.parent = GameObject.FindWithTag("Player1Switch").transform;
        // tempButton = 1;
        playerHitting = false;
    }

    public void playerDownAir()
    {
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && Input.GetKey("s") && facingRight == true)
        {
            damage = 7.0f;

            anim.Play("Player_Switched_DownAir");
            playerHitting = true;
            inputAttack = 8;



        }
        if (Input.GetKeyUp("j") && playerHitting != true && grounded == false && Input.GetKey("s") && facingRight == false)
        {
            damage = 7.0f;
            playerHitting = true;
            anim.Play("Player_Switched_DownAirLeft");

            inputAttack = 8;
        }
    }
    public void createHurtBoxforSideSpecial()
    {//creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(2.25f, 0.3f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.5f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);
    }
    public void createHurtBoxforSideSpecialLeft()
    {//creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-2.25f, 0.3f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 2.5f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);
    }


    //Specials

    public void playerUpSpecial()
    {
        if (Input.GetKeyUp("k") && upwardMovement.y > 0 && grounded == true && playerHitting != true && upSpecialCoolDown <= 0)
        {
            if (facingRight == true)
            {
                anim.Play("Player1_Switched_UpSpecial");
            }
            else if(facingRight == false)
            {
                anim.Play("Player1_Switched_UpSpecialLeft");
            }
            damage = 10.0f;
            playerHitting = true;
            inputAttack = 4;
        }

        if(upSpecialCoolDown  >=0)
        {
            upSpecialCoolDown -= Time.deltaTime;
        }
    }//end playerUpSpecial()
    public void createUpSpecialHitBox()
    {
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(0f, .50f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 3.5f;
        
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);
    }//end createUpSpecialHitBox()
    public void destroyUpAirHitBox()
    {
        upSpecialCoolDown = 5.0f;
        GameObject[] things = GameObject.FindGameObjectsWithTag("DamageHurtBox");
        // GameObject temp = GameObject.FindGameObjectWithTag("DamageHurtBox");
        for (int i = 0; i < things.Length; i++)
        {
            Destroy(things[i]);

        }
        playerHitting = false;
       
    }
    public void sideSpecial()
    {
        if (Input.GetKeyUp("k") && movement.x >= .1 && animationPlaying == false && grounded == true && playerHitting != true)
        {
            anim.Play("Player1_Switch_SideSpecial");
            damage = 20.0f;
            playerHitting = true;
            inputAttack = 5;
        }
        else if (Input.GetKeyUp("k") && movement.x <= -.1 && animationPlaying == false && grounded == true && playerHitting != true)
        {
            anim.Play("Player1_Switched_SideSpecialLeft");
            damage = 20.0f;
            playerHitting = true;
            inputAttack = 5;
        }
    }


    public void playerDownSpecial()
    {
        if (Input.GetKey("s") && Input.GetKey("k") && grounded == true)
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
        if (downSpecialActivation >= 0.0f && isSwitching == false)
        {
            downSpecialActivation -= Time.deltaTime;
        }

        if (downSpecialActivation >= 3.0f)
        {

            anim.Play("Player1_Switched_Switch");
            downSpecialActivation = 0.0f;

        }

    }
    public void playerNeutralSpecial()
    {
        if (movement.x == 0 && Input.GetKey("k") && playerHitting != true && neutralSpecialRelease == false && neutralSpecialCoolDown <= 0 && !Input.GetKey("w"))
        {
            isNeutralSpecial = true;

            playerHitting = true;
            if (facingRight == true)
            {
                anim.Play("Player1_Switched_NeutralSpecialCharge");
            }
            else if (facingRight == false)
            {
                anim.Play("Player1_Switched_NeutralSpecialChargeLeft");
            }
            damage = 5.0f;
            inputAttack = 6;
        }
        if (neutralSpecialCoolDown >= 0)
        {
            neutralSpecialCoolDown -= Time.deltaTime;
        }

    }
    public void animateNeutralSpecial()
    {//animates the neutral special
        neutralSpecialRelease = true;
        anim.Play("Player1_Switch_NeutralSpecialRelease");

    }//animateNeutralSpecial()

    public void animateNeutralSpecialLeft()
    {
        neutralSpecialRelease = true;
        anim.Play("Player1_Switched_NeutralSpecialReleaseLeft");
    }
    public void playerNeutralSpecialHitbox()
    {
        NeutralSpecialCharge = 0.0f;
        //creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(1.25f, 0.0f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 3.0f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }
    public void playerNeutralSpecialHitboxLeft()
    {
        NeutralSpecialCharge = 0.0f;
        //creates the Hurtbox
        Vector2 colliderOffset = GetComponent<CapsuleCollider2D>().offset;
        colliderOffset += new Vector2(-1.25f, 0.0f);
        GameObject temporary = Instantiate(hurtBoxPrefab, (Vector2)transform.position + colliderOffset, Quaternion.Euler(0, 0, 0));
        temporary.transform.parent = GameObject.FindWithTag("Player1Switch").transform;

        //tempHurtBox.transform.localScale = new Vector3(20, 20, 20);
        temporary.GetComponent<CircleCollider2D>().radius = 3.0f;
        //.transform.localScale = new Vector3(3, 3, tempUpAir.transform.scale.current.z);

    }
    public void Ground()
    {
        grounded = true;
        jumpCurrent = 2;
    }

    private void UpdateAnimations()
    {
        float speed = movement.magnitude; //Slightly faster: movement.sqrMagnitude
        anim.SetFloat("Velocity", speed);
        anim.SetBool("Grounded", true);
        if (isShielding == false)
        {
            anim.SetFloat("Horizontal", movement.x);

            anim.SetFloat("Horizontal", 0);
        }
        if (isShielding == true)
        {
            anim.SetFloat("Velocity", 0);
        }
        anim.SetBool("Shielding", isShielding);
        anim.SetBool("Stunned", isStunned);
        anim.SetBool("Switching", isSwitching);
        if (grounded == true && isShielding == false)
        {
            loop1 = 1;
            anim.SetInteger("FlipLoop", 1);
            anim.SetFloat("Velocity", speed);
        }
        anim.SetInteger("Combat Level", combatLevel);

    }
    public void isFacingRight()
    {
        if (movement.x > 0)
        {//Facing Right
            facingRight = true;
            anim.SetBool("isFacingRight", true);
        }
        else if (movement.x < 0)
        {
            facingRight = false;
            anim.SetBool("isFacingRight", false);
        }

    }
    //Animation stuff I guess for now


    public void sideSpecialAnimationJumpGlitchBeg()
    {
        animationPlaying = true;
        character.velocity = new Vector2(0, 0);

        isSideSpecialAttacking = true;
    }
    public void sideSpecialAnimationJumpGlitchEnd()
    {
        isSideSpecialAttacking = false;
        animationPlaying = false;
    }
    public void endAnimation()
    {
        anim.Play("Player1_Switched_Initial");
    }
    public void endAnimationLeft()
    {
        anim.Play("Player1_Switched_LeftInitial");
    }
    public float getHPCurrent()
    {
        return healthCurrent;
    }
    public void switchForms()
    {
        Destroy(this.gameObject);
        Instantiate(Player_Switch, transform.position, Quaternion.Euler(0, 0, 0));


    }

    public void destroyHitBox()
    {
        // if( continuedHit <= 0.0f)
        //{
        //  combat = 0;
        //}
        GameObject[] things = GameObject.FindGameObjectsWithTag("DamageHurtBox");
        // GameObject temp = GameObject.FindGameObjectWithTag("DamageHurtBox");
        for (int i = 0; i < things.Length; i++)
        {
            Destroy(things[i]);

        }
        playerHitting = false;
        isNeutralSpecial = false;
        neutralSpecialRelease = false;
        neutralSpecialCoolDown = 2.0f;
    }



    public float getDamage()
    {
        return damage;
    }
    public bool getFacingRight()
    {
        return facingRight;
    }
    public int getInput()
    {
        return inputAttack;
    }

}
  
