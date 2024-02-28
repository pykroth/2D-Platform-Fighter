using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{   //Fireball stats
    [Tooltip("Speed of the the fireball (Recommended >= 7)")]
    public float moveSpeed = 8.0f;

    [Tooltip("The damage to the target the fireball hits")]
    public int damage = 5;
    [Tooltip("The knockback force to the target the fireball hits")]
    public float knockback = 3.0f;
    [Tooltip("Number of enemies can the fireball hit before disappearing")]
    public int impactsLeft = 1;
    [Tooltip("Number of units the fireball travels before disappearing.")]
    public float range = 8;
    //Internal Instance Variables
    private Vector2 startingPosition;
    private List<GameObject> alreadyHit;
    private bool isActive = true;
    private float offsetY = -0.3f;
    //private float moveModifier = 1.0f;

    //Components
    private Rigidbody2D body;
    private Collider2D myCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public GameObject sandBag;
    public bool facingRight;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isActive = true;
        //Make note of the starting position.
        startingPosition = transform.position;
        sandBag = GameObject.FindWithTag("Player2");
        facingRight = GameObject.FindWithTag("Player").GetComponent<Player>().getFacingRight();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Layer order
        //Note: The value must be between -32768 and 32767.
        spriteRenderer.sortingOrder = 30000 - (int)((spriteRenderer.bounds.min.y + offsetY) * 100);

        if (Vector2.Distance(transform.position, startingPosition) > range && isActive)
            animator.Play("Projectile_Impact");


    }
    public void FixedUpdate()
    {
        //transform.right is the "forward" vector
        Vector2 moveAmount = transform.right * moveSpeed * Time.deltaTime;

        if (isActive)
            body.MovePosition((Vector2)transform.position + moveAmount);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player2") && facingRight == false)
        {
            Vector2 temp = new Vector2(4.0f, 4.0f);
            sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
            isActive = false;

            animator.Play("Projectile_Hit");

        }
        else if(other.CompareTag("Player2") && facingRight == true)
        {

            Vector2 temp = new Vector2(-4.0f, 4.0f);
            sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
            isActive = false;

            animator.Play("Projectile_Hit");
        }
        if (other.CompareTag("Ground"))
        {
            animator.Play("Projectile_Hit");

        }
    }
    public void destroyTheObject()
    {
        Destroy(GameObject.FindWithTag("PlayerProjectile"));
    }
}
