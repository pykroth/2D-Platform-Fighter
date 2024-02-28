using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Player1 : MonoBehaviour
{
    public RectTransform barBack; //We need RectTransform because we will be moving this
    public RectTransform barFront;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColors(Color foreground, Color background)
    {
        barBack.GetComponent<Image>().color = background;
        barFront.GetComponent<Image>().color = foreground;
    }
    public void ChangeFill(float ratio)
    {
        barFront.GetComponent<Image>().fillAmount = ratio;
    }
    public void ChangeFill(float current, float max)
    {
        barFront.GetComponent<Image>().fillAmount = current / max;
    }

}
