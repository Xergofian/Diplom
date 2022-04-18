using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomDraw : MonoBehaviour
{
	public GameObject TextPrefab;
	private GameObject[] rooms;
	private GameObject[] connectors;
	private List<GameObject> roomLables;
    private void Start()
    {
	    rooms = GameObject.FindGameObjectsWithTag("Room");
	    connectors = GameObject.FindGameObjectsWithTag("connector");
        
	    foreach (var room in rooms)
	    {
		    var visibility = room.GetComponent<MeshRenderer>();
		    visibility.enabled = false;
		    
		    var roomLabel = Instantiate(
			    TextPrefab,
			    new Vector3(room.transform.position.x, room.transform.position.y + 5 , room.transform.position.z),
			    Quaternion.Euler(90, 0, 0),
			    room.transform);
		    
		    roomLabel.name += $"{room.name}";
		    var textMeshRoom = roomLabel.GetComponent<TextMesh>();
		    textMeshRoom.text = room.name;
		   // roomLables.Add(roomLabel);
	    }

	    foreach (var connector in connectors)
	    {
		    var visibility = connector.GetComponent<MeshRenderer>();
		    visibility.enabled = false;
	    }
    }

    
}
