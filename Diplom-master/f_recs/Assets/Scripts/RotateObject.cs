using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RotateObject : MonoBehaviour
{
    public GameObject ObjectToBeRotated;
    private GameObject ActiveTouch;
    private GameObject PreviosTouch;
    List<float> touch_positions_x = new List<float>(10){};
    List<float> touch_positions_y = new List<float>(10){};
    public float RotationalSpeed;
    public int waiting_step;
    public int length;
    float angle;
    

    public Camera camer;
    private float distance_active_frame;
    private float distance_lact_frame;
    private int frameElapsedCounter = 0;
    public float ZoomingСoefficient;

    void Zoom()
    {
        Vector2 finger_1 = Input.GetTouch(0).position;
        Vector2 finger_2 = Input.GetTouch(1).position;
        
        if(frameElapsedCounter != 0)
        {
            distance_active_frame = Vector2.Distance(finger_1, finger_2);
        
            if(distance_active_frame > distance_lact_frame) 
            {
                camer.fieldOfView -= ZoomingСoefficient; 
            }
            else if(distance_active_frame < distance_lact_frame)
            {
                camer.fieldOfView += ZoomingСoefficient;
            }  
            else if(distance_active_frame == distance_lact_frame)
            {
                //nothing doing
            }
        }
        else
        {
            frameElapsedCounter ++;
        }
        distance_lact_frame = distance_active_frame;   
    }
    void Rotate_Object()
    {
         Vector2 delta = Input.GetTouch(0).deltaPosition;
            
                Touch touch = Input.GetTouch(0);
            
                switch (touch.phase)
                {
                    case TouchPhase.Began:                                         //fix with change touch postion      
                    
                        for(int i = 0; i <= waiting_step; i++)
                        {
                            touch_positions_x[i] = touch.position.x;
                            touch_positions_y[i] = touch.position.y;
                        }

                    break;
                   
                    case TouchPhase.Moved:
                    
                        if(touch_positions_x.Count > length)
                        {
                            touch_positions_x.RemoveRange(length - 1, touch_positions_x.Count - length);
                            touch_positions_y.RemoveRange(length - 1, touch_positions_y.Count - length);
                        }
            
                        for(int i = touch_positions_x.Count - 1 ; i >= 1 ; i--)
                        {
                            touch_positions_x[i] = touch_positions_x[i - 1];
                            touch_positions_y[i] = touch_positions_y[i - 1]; 
                        }
            
                        touch_positions_x.Insert(0, touch.position.x);      
                        touch_positions_y.Insert(0, touch.position.y);

                        ActiveTouch.transform.position = new Vector2(touch_positions_x[0],touch_positions_y[0]);
                        PreviosTouch.transform.position = new Vector2(touch_positions_x[waiting_step],touch_positions_y[waiting_step]);    

                        //draw_touch(ActiveTouch, Color.green);
                        //draw_touch(PreviosTouch, Color.red); 
                    
                        angle = Vector2.SignedAngle (PreviosTouch.transform.position, ActiveTouch.transform.position); 

                        //Debug.Log(angle);
                        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                        {
                            if(Mathf.Abs(touch.position.y) <= Screen.height /2)
                            {
                                ObjectToBeRotated.transform.Rotate(0, angle * RotationalSpeed, 0 , Space.Self);
                            }
                            else
                            {
                                ObjectToBeRotated.transform.Rotate(0, -angle * RotationalSpeed, 0, Space.Self);
                            }                            
                        }
                        else
                        {
                            ObjectToBeRotated.transform.Rotate(0 , 0, angle * RotationalSpeed, Space.Self);
                        }
                    
                    break;
                
                    case TouchPhase.Ended:
                    
                        if(buttonTouch == false)
                        {
                            for( int i = 0; i <= touch_positions_x.Count; i++)
                            {
                                touch_positions_x.RemoveAt(i);
                                touch_positions_y.RemoveAt(i);
                            }
                        }
                        else
                        {
                            buttonTouch = false;
                        }
                            
                    break;   
                        
                }
    }

    private bool buttonTouch = false;
    
    public void SetStartPosition()
    {
        buttonTouch = true;
        Camera.main.transform.position = new Vector3(0, 250, -11);
        Camera.main.fieldOfView = 62.4f;

        ObjectToBeRotated.transform.position = new Vector3(13.5f, -7.629f, -18.29f);
        ObjectToBeRotated.transform.rotation = Quaternion.Euler(0, 90, 0); 
    }
    
    void Start()
    {
        frameElapsedCounter = 0;
        ActiveTouch = new GameObject();
        PreviosTouch = new GameObject();
        touch_positions_x = new List<float>(10){};
        touch_positions_y = new List<float>(10){};
        buttonTouch = false;
    }
    void Update()
    {   
    
       // Debug.Log("ActiveTouch x:" + ActiveTouch.transform.position.x + " |  ActiveTouch y:" + ActiveTouch.transform.position.y);
       // Debug.Log("PreviosTouch x:" + PreviosTouch.transform.position.x + " |  PreviosTouch y:" + PreviosTouch.transform.position.y);
        if(Input.touchCount > 0 )
        { 
            if (Input.touchCount == 2)
            {
                Zoom();
            } 
            else if (Input.touchCount == 1)
            {
                frameElapsedCounter = 0;
                Rotate_Object(); 
            }
            else
            {
                frameElapsedCounter = 0;
            }
        }
    }
}
