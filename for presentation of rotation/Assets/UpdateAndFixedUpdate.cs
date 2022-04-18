using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAndFixedUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update() {
        Debug.Log("Update time:" + Time.deltaTime);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate time:" + Time.deltaTime); 
    }
}
