using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Collider2D myCollider;
    public int attackInput = 0;
    public float damage = 0;
    public GameObject sandBag;
    public int attackInputSwitch = 0;
    public bool facingRight = false;
  //  public SandBagScript script;

    // Start is called before the first frame update
    void Start()
    {
        sandBag = GameObject.FindWithTag("Player2");
        myCollider = GetComponent<Collider2D>();
     //   script = GameObject.FindWithTag("Player2").GetComponent<SandBagScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindWithTag("Player"))
        {
            attackInput = GameObject.FindWithTag("Player").GetComponent<Player>().getInput();
            damage = GameObject.FindWithTag("Player").GetComponent<Player>().getDamage();
            facingRight = GameObject.FindWithTag("Player").GetComponent<Player>().getFacingRight();
        }
        if (GameObject.FindWithTag("Player1Switch"))
        {
            attackInputSwitch = GameObject.FindWithTag("Player1Switch").GetComponent<Player_SwitchedForm>().getInput();
            damage = GameObject.FindWithTag("Player1Switch").GetComponent<Player_SwitchedForm>().getDamage();
            facingRight = GameObject.FindWithTag("Player1Switch").GetComponent<Player_SwitchedForm>().getFacingRight();
        }
     
    }

    private void OnDrawGizmos()
    {
        myCollider = GetComponent<Collider2D>(); 
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(myCollider.bounds.center, myCollider.bounds.extents * 2);


    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (attackInput != null)
        {


            if (other.CompareTag("Player2") && attackInput == 1)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(2.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-2.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInput == 2)
            {
                if (facingRight == true)
                {
                    //Debug.Log("l");
                    Vector2 temp = new Vector2(16.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);

                }
                else if (facingRight == false)
                {
                    //Debug.Log("l");
                    Vector2 temp = new Vector2(-16.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }

            }
            if (other.CompareTag("Player2") && attackInput == 3)
            {
                if (facingRight == true)
                {
                    //        Debug.Log("l");
                    Vector2 temp = new Vector2(9.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {

                    //        Debug.Log("l");
                    Vector2 temp = new Vector2(-9.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    
                }
            }
            if (other.CompareTag("Player2") && attackInput == 4)
            {
                if (facingRight == true)
                {
                            Debug.Log("l");
                    Vector2 temp = new Vector2(10.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {

                    //        Debug.Log("l");
                    Vector2 temp = new Vector2(-10.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);

                }
            }
            if (other.CompareTag("Player2") && attackInput == 5)
            {
                if (facingRight == true)
                {
                    Debug.Log("l");
                    Vector2 temp = new Vector2(40.0f, 15.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {

                    //        Debug.Log("l");
                    Vector2 temp = new Vector2(-40.0f, 15.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);

                }
                
            }
            if (other.CompareTag("Player2") && attackInput == 6)
            {
                if (facingRight == true)
                {
                    
                    Vector2 temp = new Vector2(-6.0f, 9.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {

                    //        Debug.Log("l");
                    Vector2 temp = new Vector2(6.0f, 9.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);

                }

            }

        }//checks for the player
        if(attackInputSwitch != null)
        {
            if (other.CompareTag("Player2") && attackInputSwitch == 1)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(2.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-2.0f, 2.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 2)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(2.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-2.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 3)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(10.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-10.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 4)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(10.0f, 10.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-10.0f, 10.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 5)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(8.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-8.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }

            }
            if (other.CompareTag("Player2") && attackInputSwitch == 6)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(6.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-6.0f, 5.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 7)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(10.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(-10.0f, 0.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 8)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(0.0f, -15.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(0.0f, -15.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
            if (other.CompareTag("Player2") && attackInputSwitch == 9)
            {
                if (facingRight == true)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(0.0f, 10.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                    //   Debug.Log("This Works");
                    //hitforce = new Vector()
                    //Get the enemy script
                    ///Enemy script = other.GetComponent<Enemy>();
                    //script.Hit(damage, hitForce);
                }
                else if (facingRight == false)
                {
                    //  Debug.Log("l");
                    Vector2 temp = new Vector2(0.0f, 10.0f);
                    sandBag.GetComponent<SandBagScript>().Hit(damage, temp);
                }
            }
        }//Checks if the switchTransformation is present
    } //end OnTriggerEnter2D

}
