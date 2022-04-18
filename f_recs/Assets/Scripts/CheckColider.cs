using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColider : MonoBehaviour
{
    List<GameObject> ConnectorListOnFlour = new List<GameObject>(){};
    private int length;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] ConectorArray = GameObject.FindGameObjectsWithTag("RoomConnector");
        length = ConectorArray.Length;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
