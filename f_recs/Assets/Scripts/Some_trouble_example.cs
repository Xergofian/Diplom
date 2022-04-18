using UnityEngine;
using System.Collections;

public class ZoomHandler : MonoBehaviour
{
    float distance;

    void Update() {
        if (Input.touchCount == 2) Zoom();
        else if (distance != 0) distance = 0;
    }

    void Zoom()
    {
        Vector2 finger_1 = Input.GetTouch(0).position;
        Vector2 finger_2 = Input.GetTouch(1).position;

        if (distance == 0) distance = Vector2.Distance(finger_1, finger_2);

        float delta = Vector2.Distance(finger_1, finger_2) - distance;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, delta * Time.deltaTime);
    
        distance = Vector2.Distance(finger_1,finger_2);
    }
}