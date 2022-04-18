using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CanvasHandler : MonoBehaviour
{
    private GameObject AlertPanel;
    public GameObject PathRoot;
   

   private int connectorLenght;
    private List<Transform> ConnectorList = new List<Transform>();
    private Transform EndPoint;
    private int counterEndPoint;
    private Transform StartPoint;
    private int counterStartPoint;

    public string[] GetNames() => InputValueOfRooms.Select(InputValueOfRooms => InputValueOfRooms.text).ToArray();

    [SerializeField]
    [Range(0.5f, 2f)]
    public float StepDelay;

    [Range(10f, 15f)]

    public float DistanceBetweenPoints;
    public GameObject PathDrawerPrefab;
    public List<GameObject> PathDrawers;

    private TMP_InputField[] InputValueOfRooms;

    [SerializeField]
    private float PathDelay = 0.5f;

    private List<Transform> FindPath(string start_room, string end_room)
    {
        start_room = CheckInputFeild(start_room);
        end_room = CheckInputFeild(end_room);

        if (start_room == "err" || start_room == "errN")
        {
            if (start_room == "err")
            {
                // OpenAlerPanel("<b> Error </b> with <b> audience near which you are </b>");
            }
            else if (start_room == "errN")
            {
                //OpenAlerPanel("<b> Error </b> with <b> audience near which you are N </b>");
            }

            return null;
        }
        else if (end_room == "err" || end_room == "errN")
        {
            if (end_room == "err")
            {
                //OpenAlerPanel("<b> Error </b> with <b> audience you want to reach </b>"); 
            }
            else if (end_room == "errN")
            {
                //OpenAlerPanel("<b> Error </b> with <b> audience you want to reach N </b>");
            }

            return null;
        }
        else
        {
            for (int i = 0; i < PathRoot.transform.childCount; i++)
            {
                ConnectorList.Insert(i, PathRoot.transform.GetChild(i));
            }

            for (int i = 0; i < PathRoot.transform.childCount; i++)
            {
                for (int j = 0; j < ConnectorList[i].childCount; j++)
                {
                    if (start_room == ConnectorList[i].transform.GetChild(j).name)
                    {
                        counterStartPoint = i;
                        StartPoint = ConnectorList[i].transform.GetChild(j);                    //StartPoint
                    }
                }
            }

            for (int i = 0; i < PathRoot.transform.childCount; i++)
            {
                for (int j = 0; j < ConnectorList[i].childCount; j++)
                {
                    if (end_room == ConnectorList[i].transform.GetChild(j).name)
                    {
                        counterEndPoint = i;
                        EndPoint = ConnectorList[i].transform.GetChild(j);                    //StartPoint
                    }
                }
            }

            var PathListOfCoordinates = new List<Transform> { StartPoint };

            if (counterStartPoint == counterEndPoint)
            {
                PathListOfCoordinates.Add(ConnectorList[counterEndPoint]);
            }
            else if (counterStartPoint < counterEndPoint)
            {
                for (int i = counterStartPoint; i <= counterEndPoint; i++)
                {
                    PathListOfCoordinates.Add(ConnectorList[i]);
                }
            }
            else if (counterStartPoint > counterEndPoint)
            {
                for (int i = counterStartPoint; i >= counterEndPoint; i--)
                {
                    PathListOfCoordinates.Add(ConnectorList[i]);
                }
            }
            PathListOfCoordinates.Add(EndPoint);

            return PathListOfCoordinates;
        }
    }
    public string CheckInputFeild(string inputValue)
    {
        if (Int32.TryParse(inputValue, out int room_number))
        {
            if (room_number < 711 || room_number > 0)
            {
                return inputValue;
            }
            else
            {
                inputValue = "errN";
                return inputValue;
            }
        }
        else
        {
            if (inputValue == "222a" || inputValue.ToLower() == "буфет")
            {
                return inputValue;
            }
            else
            {
                inputValue = "err";
                return inputValue;
            }
        }
    }
    private IEnumerator DrawPath()
    {
        InputValueOfRooms = GameObject.FindGameObjectsWithTag("InputField").Select(GameObject => GameObject.GetComponent<TMP_InputField>()).ToArray();

        string start_room = "";

        string end_room = "";

        for (int i = 0; i < InputValueOfRooms.Length; i++)
        {
            if (InputValueOfRooms[i].gameObject.name == "StartRoomInputField")
            {
                start_room = InputValueOfRooms[i].GetComponent<TMP_InputField>().text;
            }
            else if (InputValueOfRooms[i].gameObject.name == "EndRoomInputField")
            {
                end_room = InputValueOfRooms[i].GetComponent<TMP_InputField>().text;
            }
        }

        var PathListOfCoordinates = FindPath(start_room, end_room);

        for (int i = 0; i < PathListOfCoordinates.Count; i++)
        {
            Debug.Log(PathListOfCoordinates[i].gameObject.name);
        }

        int amountOfPathDrawers = FindAmount(PathListOfCoordinates);
        
        Debug.Log($"amountOfPathDrawers : {amountOfPathDrawers}");
        
        for (int i = 0; i < amountOfPathDrawers; i++)
        {

            var PathDrawer = Instantiate(PathDrawerPrefab);
            PathDrawers.Add(PathDrawer);

            yield return new WaitForSeconds(PathDelay);
            StartCoroutine(DrawIndividualPath(PathDrawer, PathListOfCoordinates));
        }
    }
 
    private IEnumerator DrawIndividualPath(GameObject PathDrawer, List<Transform> PathListOfCoordinates)
    {
        PathDrawer.transform.position = PathListOfCoordinates[0].position;
        PathDrawer.SetActive(false);
        yield return null;

        PathDrawer.SetActive(true);
        yield return null;

        var waitTime = 0.5f;

        for (int i = 0; i < PathListOfCoordinates.Count - 1; i++)
        {
            var elapsedTime = 0.0f;
            while (elapsedTime < waitTime)
            {
                PathDrawer.transform.position = Vector3.Lerp(PathListOfCoordinates[i].position, PathListOfCoordinates[i + 1].position, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            PathDrawer.transform.position = PathListOfCoordinates[i + 1].position;
            yield return null;
        }

        yield return new WaitForSeconds(PathDelay);
        StartCoroutine(DrawIndividualPath(PathDrawer, PathListOfCoordinates));
    }
    private int FindAmount(List<Transform> ListCordinates)
    {
        float distance = 0.0f;
        for (int i = 0; i < ListCordinates.Count - 1; i++)
        {
            distance += Vector3.Distance(ListCordinates[i].position, ListCordinates[i+1].position);
        }
        int amountOfPathDrawers = Mathf.RoundToInt(distance / DistanceBetweenPoints);
        return amountOfPathDrawers;
    }
    private void ClearPathDraws()
    {
        foreach (var PathDraw in PathDrawers)
            Destroy(PathDraw);
        PathDrawers.Clear();
    }
    //==================================================================================================================================================
    public void StartDraw()
    {
        ClearPathDraws();
        StartCoroutine(DrawPath());
    }
    public void StopDraw()
    {
        ClearPathDraws();
        StopAllCoroutines();

    }
    
    
    private void Update()
    {
        Transform PathObject = PathRoot.transform.GetChild(PathRoot.transform.childCount-1);
        if (PathObject.childCount % 2 != 0)
        {
            Debug.Log(PathObject.transform.GetChild(Mathf.RoundToInt(PathObject.transform.childCount / 2)));
        }
    }
}