using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenitsu : Character
{
    //Instance Variables

    //Defining other scripts
    private Animator anim;


    //Figures out what tag
    public int tag = 0;


    //Varaibles for Arsene
    public bool arseneFormActive = false;

   
    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
   public override void Update()
    {
        base.Update();
       //How Arsene Will Start
       if(Input.GetKey("m"))
        {
            setArseneFormActive();
        }
        updateAnimations();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //How Arsene Will Start
        if (Input.GetKey("m"))
        {
            setArseneFormActive();
        }
        updateAnimations();
    }
    #region Arsene
    //Code where zenitsu transforms after taking a certain amount of damage
    public void setArseneFormActive()
    {
        arseneFormActive = true;
    }
    public void setArseneFormDisable()
    {
        arseneFormActive = false;
    }
    //Checks if arsene is Active
    public bool isArseneActive()
    {
        return arseneFormActive;
    }
    #endregion
    #region Potentially future use for controls
    //Code that finds the tag of the player
    public void whatTag()
    {
        //if (this.GameObject.CompareTag("Player")
         //   {
           // tag = 1;
            //}
        //else if(this.GameObject.CompareTag("Player2"))
        //{
         //   tag = 2;
        //}
        
    }
    #endregion 
   

    private void updateAnimations()
    {
       // Debug.Log(tempVelocity);
        anim.SetBool("isArseneActive", arseneFormActive);
        anim.SetFloat("Movement", tempVelocity);
     //   anim.SetBool("WaveDashing", airDashisActive);
        anim.SetBool("isGrounded", isGrounded);
        
    }
}


