using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDemonstration : MonoBehaviour
{
    public GameObject Canvas;
    public Camera cam;
    public GameObject Flour;
    public GameObject Active_touch;
    public GameObject Previos_touch;
    List<float> touch_positions_x = new List<float>(10){};
    List<float> touch_positions_y = new List<float>(10){};
    public int waiting_step;
    public int length;
    float angle;
    //Vector3 previos_point = new Vector3();
    //Vector3 active_point = new Vector3();
    void draw_touch(GameObject obj, Color color) 
    {
        Debug.DrawLine(Flour.transform.position, obj.transform.position, color);
    }
    void Start()
    {
        RectTransform RT = Canvas.GetComponent<RectTransform>();
        cam.transform.position = new Vector3(RT.pivot.x + RT.rect.width / 2, RT.pivot.y + RT.rect.height / 2, 0);
    }
    async void FixedUpdate()
    {   
        //float angle = Vector2.SignedAngle(previos_point, active_point);
        //active_point = active_touch.transform.position - transf
        
        if(Input.touchCount > 0 )
        {   
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    
                
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

                    Active_touch.transform.position = new Vector2(touch_positions_x[0],touch_positions_y[0]);
                    Previos_touch.transform.position = new Vector2(touch_positions_x[waiting_step],touch_positions_y[waiting_step]);    

                    draw_touch(Active_touch, Color.green);
                    draw_touch(Previos_touch, Color.red); 
                    
                    angle = Vector2.SignedAngle (Previos_touch.transform.position, Active_touch.transform.position); 

                    if(Mathf.Abs(touch.position.y) <= Screen.height /2)
                    {
                        Flour.transform.Rotate(0, 0,-angle * 2 , Space.World);
                    }
                    else
                    {
                        Flour.transform.Rotate(0, 0, angle * 2 , Space.World);
                    }         
                    
                    break;
                
                case TouchPhase.Ended:

                    for( int i = 0; i <= touch_positions_x.Count; i++)
                    {
                        touch_positions_x.RemoveAt(i);
                        touch_positions_y.RemoveAt(i);
                    }

                    break;
            }

        }

                        
    }
}
