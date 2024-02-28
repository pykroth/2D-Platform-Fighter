using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetHurtBoxSwitchedForm : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Player_SwitchedForm script;


    public bool test = false;

    public Collider2D myCollider;
    void Start()
    {
        script = transform.root.GetComponent<Player_SwitchedForm>();
        myCollider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(test);



    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {

            script.Ground();
            test = true;

        }

    }
    private void OnDrawGizmos()
    {
        myCollider = GetComponent<Collider2D>(); ;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(myCollider.bounds.center, myCollider.bounds.extents * 2);


    }
}
