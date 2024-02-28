using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameratoFollow : MonoBehaviour
{

    public Transform followTransform;
    public Transform followTransform2;
    private float camOrthsize; //Zoom level (used for y-height of view)
    private float cameraRatio; //Aspect ratio (used for x-width of view)
    public Camera mainCam; //Reference to this
    private Vector3 smoothPos;
    private float camX, camY;
    public float smoothSpeed = 0.5f;
    public bool arseneActive = false;

    //Define other pieces of objects
    public SpriteRenderer background;
    public TilemapRenderer ground;

    //Timers for Animation
    double tempTimer = 1.0f;

    //Reference for other scripts
    public Zenitsu zenitsuScript;

    //Variables for camera
    float tempCam = 0; //The first cam
    float tempCam2 = 0; //The second pos
    float tempCam3 = 0; //The y cam
    float orthographicSizeNum = 0;
    float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (followTransform != null && followTransform.position.x > followTransform2.position.x)
        {
            //Defines variables
            zenitsuScript = GameObject.FindWithTag("Player2").GetComponent<Zenitsu>();

            //Immediately jump to the thing we are following                                        //Keep the camera's z
            transform.position = new Vector3(followTransform.position.x - followTransform2.position.x, followTransform.position.y - followTransform2.position.y, transform.position.z);
             mainCam = GetComponent<Camera>();
           
            camOrthsize = mainCam.orthographicSize;
            cameraRatio = mainCam.aspect * camOrthsize;
            background = GameObject.FindWithTag("Background").GetComponent<SpriteRenderer>();
           
        }
    }

    void Update()
    {
        arseneActive = zenitsuScript.isArseneActive();
        if(GameObject.FindWithTag("Player"))
             followTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        else if (GameObject.FindWithTag("Player1Switch"))
            followTransform = GameObject.FindWithTag("Player1Switch").GetComponent<Transform>();

        followTransform2 = GameObject.FindWithTag("Player2").GetComponent<Transform>();
        //Code will run when arseneActive becomes true
        arsene();
        

    }
    // Update is called once per frame
    
    private void FixedUpdate()
    {
        if (arseneActive == false)
        {
          //  Debug.Log((tempCam + tempCam3)/2);
            orthographicSizeNum = (tempCam + tempCam3) / 2;
            //Calculations to change orthographic 
            if (followTransform.position.x >= followTransform2.position.x)
            {
                 tempCam = followTransform.position.x - followTransform2.position.x;
                 tempCam2 = followTransform.position.x + tempCam / 2.0f;
            }
            else if(followTransform2.position.x > followTransform.position.x)
            {
                 tempCam = followTransform2.position.x - followTransform.position.x;
                tempCam2 = followTransform2.position.x + tempCam / 2.0f;
            }
            //Figures out the y point
            tempCam3 = Mathf.Abs(followTransform.position.y - followTransform2.position.y);
            
              if (orthographicSizeNum < 2)
              {
                  mainCam.orthographicSize = 2;
              }
            else
                mainCam.orthographicSize = (tempCam + tempCam3) / 2;
            /*
            else if (Mathf.Abs(followTransform.position.y - followTransform2.position.y)< 5 && Mathf.Abs(followTransform.position.y - followTransform2.position.y) > 1)
            {
              mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, (tempCam+tempCam3)/2, );
            }
            else
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, tempCam/2, tempCam2);
           */

            //Calculate the goal position of the camera but restrict it from going to the edge





            //The calculations for the change in position

            if (followTransform != null && followTransform2 != null)
            {
                if (followTransform.position.x > followTransform2.position.x)
                {
                    //Calculate the goal position of the camera but restrict it from going to the edge
                    camX = Mathf.Clamp((followTransform.position.x + followTransform2.position.x) / 2, followTransform2.position.x - 5.0f, followTransform.position.x + 5.0f);





                }
                if (followTransform.position.x < followTransform2.position.x)
                {
                    //Calculate the goal position of the camera but restrict it from going to the edge
                    camX = Mathf.Clamp((followTransform.position.x + followTransform2.position.x) / 2, followTransform.position.x - 5.0f, followTransform2.position.x + 5.0f);


                    //Create the goal as a Vector3

                }
                if (followTransform.position.y < followTransform2.position.y + 3.0f)
                {
                    //Calculate the goal position of the camera but restrict it from going to the edge

                    camY = Mathf.Clamp((followTransform.position.y + followTransform2.position.y) / 2, followTransform2.position.y -5.0f, followTransform.position.y + 5.0f);

                    //Create the goal as a Vector3

                }
                else if (followTransform.position.y > followTransform2.position.y + 3.0f)
                {
                    //Calculate the goal position of the camera but restrict it from going to the edge

                    camY = Mathf.Clamp((followTransform.position.y + followTransform2.position.y)/2, followTransform.position.y -5.0f, followTransform.position.y + 5.0f);

                    //Create the goal as a Vector3

                }
                else
                    camY = Mathf.Clamp(followTransform.position.y + followTransform2.position.y, followTransform.position.y -1.0f, followTransform.position.y + 1.0f);
                Vector3 cameraGoalPosition = new Vector3(camX, camY, transform.position.z);

                //Use Lerp to calculate a position between the starting position and the goal position.
                smoothPos = Vector3.Lerp(transform.position, cameraGoalPosition, smoothSpeed);


                //Set the Camera's Position:
                transform.position = smoothPos;
            }
                
        }
    }
    
        public void arsene()
    {
        if (arseneActive == true)
        {


            Debug.Log(tempTimer);
            //Finds transform of the zenitsu
            followTransform2 = GameObject.FindWithTag("Player2").GetComponent<Transform>();
    
            //Changes it, zooms in and sets background to black
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 3, .01f);
            background.enabled = false;


            //Sets a small timer that zooms in more, and changes ground black
            tempTimer -= Time.deltaTime;
            if (tempTimer <= .1)
            {
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 2.5f, .05f);

                
                ground.enabled = false;

            }



            camX = Mathf.Clamp(followTransform2.position.x, followTransform2.position.x - 5.0f, followTransform2.position.x + 5.0f);
            camY = Mathf.Clamp(followTransform2.position.y + 1.0f, followTransform2.position.y, followTransform2.position.y + 5.0f);
            Vector3 cameraGoalPosition = new Vector3(camX, camY, transform.position.z);
            smoothPos = Vector3.Lerp(transform.position, cameraGoalPosition, smoothSpeed);
            transform.position = smoothPos;

        }
        else if (arseneActive == false)
        {
           
            ground.enabled = true;
            background.enabled = true;
        }
    }
 
}
