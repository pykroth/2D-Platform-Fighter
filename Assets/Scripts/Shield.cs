using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public CapsuleCollider2D shield;
    public SpriteRenderer shieldImage;
    public GameObject getShield;
    // Start is called before the first frame update
    void Start()
    {
        getShield.GetComponent<CapsuleCollider2D>().enabled = false;
        getShield.GetComponent<SpriteRenderer>().enabled = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(shield.enabled);
    }

    public void shielded(bool shielded)
    {
        if (shielded == true)
        {
            getShield.GetComponent<CapsuleCollider2D>().enabled = true;
            getShield.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            getShield.GetComponent<CapsuleCollider2D>().enabled = false;
            getShield.GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
    private void OnDrawGizmos()
    {
        shield = GetComponent<CapsuleCollider2D>(); ;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shield.bounds.center, shield.bounds.extents * 2);


    }
}
