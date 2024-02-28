using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player1Switched;
    public float HealthCurrent = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(GameObject.Find("Player"))
        {
            player1 = GameObject.Find("Player");
        }
        if(GameObject.Find("Player_SwitchedForm"))
        {
            player1Switched = GameObject.Find("Player1_SwitchedForm");
        }
     
        if (player1 != null)
            HealthCurrent = player1.GetComponent<Player>().getHPCurrent();
        if (player1Switched != null)
            HealthCurrent = player1Switched.GetComponent<Player_SwitchedForm>().getHPCurrent();
    }
    public float getCurrentHealth()
    {
        return HealthCurrent;
    }
}
