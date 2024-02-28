using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    //Instance Variables
   
    public GameObject timeText;
    public float time = 100;
    public double value = 1;
    public float timeTicker = 0;
    public GameObject GameOver;
    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Subtracts time for the timer
        timeTicker = timeTicker + Time.deltaTime;
        if (value < timeTicker && time > 0)
        {
            time = time - 1;
            timeText.GetComponent<Text>().text = "" + time;
            timeTicker = 0;
        }
    
        //Creates gameover when game ends
        //*Come back when adding players to delete them*
        if(time <= 1)
        {
           
            //Spawn the game over text and make the  Canvas its parent
            Text textTemp = Instantiate(gameOverText);
            textTemp.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);


        }

        //Changes font when it is last 10 seconds
        if(time <= 10)
        {
            timeText.GetComponent<Text>().color = Color.red;
            timeText.GetComponent<Text>().fontSize = 23;
        }

    }
}
