using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationAroundPoint : MonoBehaviour
{
    public GameObject pivotObject;
    public RectTransform touchChecker;
    //=================================================================================
    void RotateAroundObject()
    {
        Vector2 delta = Input.GetTouch(0).deltaPosition;
    
        Touch touch = Input.GetTouch(0);

        touchChecker.position = new Vector2(touch.position.x, touch.position.y);
    
        Image image = touchChecker.GetComponent<Image>();
    
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (Mathf.Abs(touchChecker.position.y) <= Screen.height /2) 
            {
                image.color = new Color(0, 255, 0, 255);  
                transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), delta.x * Time.deltaTime);
            }
            else
            {
                image.color = new Color(255, 0, 0, 255);  
                transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), -delta.x * Time.deltaTime);
            }
        }    
        else
        {   
            if (Mathf.Abs(touchChecker.position.y) <= Screen.height /2) 
            {
                image.color = new Color(0, 255, 0, 255); 
                transform.RotateAround(pivotObject.transform.position, new Vector3(1, 0, 0), -delta.y * Time.deltaTime); 
            }
            else
            {
                image.color = new Color(255, 0, 0, 255);  
                transform.RotateAround(pivotObject.transform.position, new Vector3(1, 0, 0), -delta.y * Time.deltaTime);
            }
            
        }        
    }
    //================================================================================
    float distance;
    void Zoom()
    {
        Vector2 finger1 = Input.GetTouch(0).position;
        Vector2 finger2 = Input.GetTouch(1).position;   

        if(distance == 0) distance = Vector2.Distance(finger1, finger2);

        float delta = Vector2.Distance(finger1, finger2) - distance;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, delta * Time.deltaTime );

        distance = Vector2.Distance(finger1, finger2);
    }
     //================================================================================
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        RotateAroundObject();
        else if (Input.touchCount == 2)  Zoom();
        else if(distance != 0) distance = 0;
    }
}
