using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeethurtBox : MonoBehaviour
{
    // Start is called before the first frame update
    Player script;
    Character playerScript;

    public bool test = false;

    public Collider2D myCollider;
    void Start()
    {
        script = transform.root.GetComponent<Player>();
          myCollider = GetComponent<Collider2D>();
        playerScript = transform.root.GetComponent<Character>();
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
            if (playerScript != null)
                playerScript.checkifGrounded();
            else
                playerScript = transform.root.GetComponent<Character>();
            //script.Ground();
            test = true;

        }

    }
    private void OnDrawGizmos()
    {
        myCollider = GetComponent<Collider2D>(); ;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(myCollider.bounds.center, myCollider.bounds.extents*2);
      
        
    }
}
