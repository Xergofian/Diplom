using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCameraPosition : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(new Vector3(target.transform.rotation.x,target.transform.rotation.y, target.transform.rotation.z ));
       transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z );
    }
}
