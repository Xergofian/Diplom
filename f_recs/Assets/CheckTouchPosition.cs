using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CheckTouchPosition : MonoBehaviour
{
    
    public RectTransform touchChecker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchChecker.position = new Vector2(touch.position.x, touch.position.y);
             Image image = touchChecker.GetComponent<Image>();

            if( Mathf.Abs(touchChecker.position.y) <= Screen.height /2 )
            {
                image.color = new Color(0, 255, 0, 255);  
            }
            else{

                image.color = new Color(255, 0, 0, 255);  
            }
        } 
    }
}

