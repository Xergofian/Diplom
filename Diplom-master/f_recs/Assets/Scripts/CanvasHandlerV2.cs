using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasHandlerV2 : MonoBehaviour
{
    private GameObject AlertPanel;
    public GameObject PathRoot;
    private int connectorLenght;
    
    public GameObject[] floorsList;
    public List<Transform> visibleFloorList;
    
    private List<Transform> connectorList = new List<Transform>();
  //==============================================================  
    private Transform endPoint;
    private int indexOfConnectorEndPoint;
    private int indexStartFloorInFloorList;
    
    private Transform startPoint;
    private int indexOfConnectorStartPoint;
    private int indexEndFloorInFloorList;
 //===============================================================   
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
        /*
        string startRoom = CheckInputFeild(start_room);
        string endRoom = CheckInputFeild(end_room);

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
        }
         */

        char[] numbersInStartRoomNumber = start_room.ToCharArray(0, start_room.Length - 1);
        char[] numbersInEndRoomNumber = end_room.ToCharArray(0, end_room.Length - 1);

        int startRoomNumber = Convert.ToInt32(numbersInStartRoomNumber[0].ToString());
        int endRoomNumber = Convert.ToInt32(numbersInEndRoomNumber[0].ToString());

        Debug.Log($"indexOfStartFloor : {startRoomNumber} | indexOfEndFloor : {endRoomNumber}");

        int indexStartFloorInFloorList = 0;
        int indexEndFloorInFloorList = 0;
        
        visibleFloorList.Clear();
        
        for (int i = 0; i < PathRoot.transform.childCount; i++)
        {
            var tempFloorName = PathRoot.transform.GetChild(i).name;
            string[] checkPartsOfTestName = tempFloorName.Split('_');
            
            if (Convert.ToInt32(checkPartsOfTestName[2])==startRoomNumber)
            {
                indexStartFloorInFloorList = i;
            }
            
            if(Convert.ToInt32(checkPartsOfTestName[2])==endRoomNumber)
            {
                indexEndFloorInFloorList = i;
            }
        }
        
        Debug.Log($"indexOfStartFloorList : {indexStartFloorInFloorList} | indexOfEndFloorList : {indexEndFloorInFloorList}");
        
        if (indexEndFloorInFloorList == indexStartFloorInFloorList)
        {
            visibleFloorList.Add(PathRoot.transform.GetChild(indexStartFloorInFloorList));
            Debug.Log("S================Name Of Floors In visibleFloorList===============S");
            for (int j = 0; j < visibleFloorList.Count; j++)
            {
                Debug.Log(visibleFloorList[j].name);
            }
            Debug.Log("E================Name Of Floors In visibleFloorList===============E");
        }
        else
        {
            if (indexStartFloorInFloorList < indexEndFloorInFloorList)
            {
                for (int i = indexStartFloorInFloorList; i <= indexEndFloorInFloorList; i++)
                {
                    visibleFloorList.Add(PathRoot.transform.GetChild(i));
                }
                
                Debug.Log("S================Name Of Floors In visibleFloorList===============S");
            
                for (int j = 0; j < visibleFloorList.Count; j++)
                {
                    Debug.Log(visibleFloorList[j].name);
                }
                Debug.Log("E================Name Of Floors In visibleFloorList===============E");
            }
            else if (indexStartFloorInFloorList > indexEndFloorInFloorList)
            {
                for (int i = indexStartFloorInFloorList; i >= indexEndFloorInFloorList; i--)
                {
                    Debug.Log(i);
                    visibleFloorList.Add(PathRoot.transform.GetChild(i));
                }
                Debug.Log("S================Name Of Floors In visibleFloorList===============S");
                for (int j = 0; j < visibleFloorList.Count; j++)
                {
                    Debug.Log(visibleFloorList[j].name);
                }
                Debug.Log("E================Name Of Floors In visibleFloorList===============E");
            }
        }
//======================================================================================================================
                                                                                                                        //чистка коріного об`єкту від непотрібних поверхів
//======================================================================================================================
        for (int i = 0; i < PathRoot.transform.childCount; i++)
        {
            bool flag = false;

            for (int j = 0; j < visibleFloorList.Count; j++)
            {
                if (visibleFloorList[j].name == PathRoot.transform.GetChild(i).name)
                { 
                    flag = true;
                }
            }
            if (flag == false)
            { 
                PathRoot.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            { 
                PathRoot.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
//======================================================================================================================
                                                                                                                        //Пригаємо по конекторам початкового поверху та шукаємо кабінет біля якого знаходиться студент
//======================================================================================================================
        var startFloor = PathRoot.transform.GetChild(indexStartFloorInFloorList);                                      
        
        var pathObjectOfStartFlour = startFloor.GetChild(startFloor.childCount-1);
        
        for (int connectorIndex = 0; connectorIndex < pathObjectOfStartFlour.childCount; connectorIndex++)                                                              
        {
            var tempConnector = pathObjectOfStartFlour.GetChild(connectorIndex);
            var roomsAmount = tempConnector.childCount;
            
            for (int roomIndex = 0; roomIndex < roomsAmount; roomIndex++)
            { 
                if (start_room == tempConnector.GetChild(roomIndex).name)
                {
                    indexOfConnectorStartPoint = connectorIndex;                                                    //indexOfConnectorStartPoint
                    startPoint = tempConnector.transform.GetChild(roomIndex);                                       //startPoint
                }
            }
        }
        Debug.Log($"indexOfConnectorStartPoint : {indexOfConnectorStartPoint} | startPoint : {startPoint.name}");
//======================================================================================================================
                                                                                                                        //пригаємо по конекторам кінцевого поверху та шукаємо кабінет який потрібен студенту
//======================================================================================================================
        var endFloor = PathRoot.transform.GetChild(indexEndFloorInFloorList);
        
        var pathObjectOfEndFlour = endFloor.GetChild(endFloor.childCount-1);
        
        for (int connectorIndex = 0; connectorIndex < pathObjectOfEndFlour.childCount; connectorIndex++)                                                              
        {
            var tempConnector = pathObjectOfEndFlour.GetChild(connectorIndex);
            var roomsAmount = tempConnector.childCount;

            for (int roomIndex = 0; roomIndex < roomsAmount; roomIndex++)
            {
                if (end_room == tempConnector.GetChild(roomIndex).name)
                {
                    indexOfConnectorEndPoint= connectorIndex;                                                   //indexOfConnectorEndPoint
                    endPoint = tempConnector.transform.GetChild(roomIndex);                                     //endPoint
                }
            }
        }
        Debug.Log($"indexOfConnectorEndPoint : {indexOfConnectorEndPoint} | endPoint : {endPoint.name}");
//======================================================================================================================
                                                                                                                        //Будуємо шлях до найближчих сходів на поверсі на якому знаходиться студент
//======================================================================================================================
        var pathListOfCoordinates = new List<Transform> {startPoint};

        if (indexStartFloorInFloorList == indexEndFloorInFloorList)
        {
            if (indexOfConnectorStartPoint < indexOfConnectorEndPoint)
            {
                for (int i = indexOfConnectorStartPoint; i <= indexOfConnectorEndPoint; i++)
                {
                    pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                }
                pathListOfCoordinates.Add(endPoint);
            }
            else if (indexOfConnectorStartPoint == indexOfConnectorEndPoint)
            {
                pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(indexOfConnectorStartPoint));
                pathListOfCoordinates.Add(endPoint);
            }
            else if(indexOfConnectorStartPoint > indexOfConnectorEndPoint)
            {
                for (int i = indexOfConnectorStartPoint; i >= indexOfConnectorEndPoint; i--)
                {
                    pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                }
                pathListOfCoordinates.Add(endPoint);
            }
        }
        else
        {
            if (indexOfConnectorStartPoint < Mathf.RoundToInt(pathObjectOfStartFlour.childCount / 2))
            {
                for (int i = indexOfConnectorStartPoint; i >= 0; i--)
                {
                    pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                }
            
                for (int floorIndex = 0; floorIndex < visibleFloorList.Count; floorIndex++)
                {
                    Debug.Log($"floorIndex => {floorIndex}");
                    var tempFloor = visibleFloorList[floorIndex];
                    var tempPathFloorObject = tempFloor.GetChild(tempFloor.childCount - 1);
                    pathListOfCoordinates.Add(tempPathFloorObject.GetChild(0).GetChild(0));                                   //Збираємо позиції сходів, які знаходяться ліворуч      !!!!!!!
                }
                
                Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
                for (int i = 0; i < pathListOfCoordinates.Count; i++)
                {
                    Debug.Log(pathListOfCoordinates[i].name);
                }
                Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
            
                var endTempFloorObject = endFloor.GetChild(endFloor.childCount - 1);
                
                for (int i = 0; i <= indexOfConnectorEndPoint; i++)
                { 
                    pathListOfCoordinates.Add(endTempFloorObject.GetChild(i));
                }
                
                pathListOfCoordinates.Add(endPoint);
                Debug.Log("=============END============");
                for (int i = 0; i < pathListOfCoordinates.Count; i++)
                {
                    Debug.Log(pathListOfCoordinates[i].name);
                }
                Debug.Log("=============END============");
            }
            else if (indexOfConnectorStartPoint == Mathf.RoundToInt(pathObjectOfStartFlour.childCount / 2))                //Перевірка чи потрібно іти ліворуч чи праворуч при випадку небезпечної ситуації, якщо людина знаходиться в центральних кабінетах на поверсі, вона мусить іти ліворуч.     
            {
                if (pathObjectOfStartFlour.GetChild(indexOfConnectorStartPoint).GetChild(0).name == start_room)             //Додаємо сходи які знаходяться ліворуч
                {
                    for (int i = indexOfConnectorStartPoint; i >= 0; i--)
                    { 
                        pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                    }
                
                    for (int floorIndex = 0; floorIndex < visibleFloorList.Count; floorIndex++)
                    {
                        Debug.Log($"floorIndex => {floorIndex}");
                        var tempFloor = visibleFloorList[floorIndex];
                        var tempPathFloorObject = tempFloor.GetChild(tempFloor.childCount - 1);
                        pathListOfCoordinates.Add(tempPathFloorObject.GetChild(0).GetChild(0));                                   //Збираємо позиції сходів, які знаходяться ліворуч      !!!!!!!
                    }
                
                    Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
                
                    for (int i = 0; i < pathListOfCoordinates.Count; i++)
                    {
                        Debug.Log(pathListOfCoordinates[i].name);
                    }
                    Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
                
                    var endTempFloorObject = endFloor.GetChild(endFloor.childCount - 1);
                
                    for (int i = endTempFloorObject.childCount-1; i >= indexOfConnectorEndPoint; i--)
                    { 
                        pathListOfCoordinates.Add(endTempFloorObject.GetChild(i));
                    }
                
                    pathListOfCoordinates.Add(endPoint);
                
                    Debug.Log("=============END============");
                    for (int i = 0; i < pathListOfCoordinates.Count; i++)
                    {
                        Debug.Log(pathListOfCoordinates[i].name);
                    }
                    Debug.Log("=============END============");
                }
                else if (pathObjectOfStartFlour.GetChild(indexOfConnectorStartPoint).GetChild(1).name == start_room)
                {
                    for (int i = indexStartFloorInFloorList; i < pathObjectOfStartFlour.childCount; i++)
                    {
                        pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                    }

                    for (int check = 0; check < pathListOfCoordinates.Count; check++)
                    {
                        Debug.Log(pathListOfCoordinates[check]);
                    }
                
                    for (int floorIndex = 0; floorIndex < visibleFloorList.Count; floorIndex++)
                    {
                        Debug.Log($"floorIndex => {floorIndex}");
                        var tempFloor = visibleFloorList[floorIndex];
                        var tempPathFloorObject = tempFloor.GetChild(tempFloor.childCount - 1);
                        pathListOfCoordinates.Add(tempPathFloorObject.GetChild(tempPathFloorObject.childCount - 1).GetChild(1));                                   //Збираємо позиції сходів, які знаходяться ліворуч      !!!!!!!
                    }
                
                    Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
                    for (int i = 0; i < pathListOfCoordinates.Count; i++)
                    {
                        Debug.Log(pathListOfCoordinates[i].name);
                    }
                    Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
               
                    var endTempFloorObject = endFloor.GetChild(endFloor.childCount - 1);
                
                    for (int i = endTempFloorObject.childCount-1; i >= indexOfConnectorEndPoint; i--)
                    { 
                        pathListOfCoordinates.Add(endTempFloorObject.GetChild(i));
                    }
                
                    pathListOfCoordinates.Add(endPoint);
                
                    Debug.Log("=============END============");
                    for (int i = 0; i < pathListOfCoordinates.Count; i++)
                    {
                        Debug.Log(pathListOfCoordinates[i].name);
                    }
                    Debug.Log("=============END============");
                }
            }
            else if(indexOfConnectorStartPoint > Mathf.RoundToInt(pathObjectOfStartFlour.childCount / 2))                                                                                                        //Ідемо до сходів які знаходяться праворуч
            {
                Debug.Log("okuiasu==========okuiasu");
            
                for (int i = indexOfConnectorStartPoint; i < pathObjectOfStartFlour.childCount; i++)
                {
                    pathListOfCoordinates.Add(pathObjectOfStartFlour.GetChild(i));
                } 
            
                for (int floorIndex = 0; floorIndex < visibleFloorList.Count; floorIndex++)
                {
                    Debug.Log($"floorIndex => {floorIndex}");
                    var tempFloor = visibleFloorList[floorIndex];
                    var tempPathFloorObject = tempFloor.GetChild(tempFloor.childCount - 1);
                    pathListOfCoordinates.Add(tempPathFloorObject.GetChild(tempPathFloorObject.childCount - 1).GetChild(1));                                   //Збираємо позиції сходів, які знаходяться ліворуч      !!!!!!!
                }
            
                Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
            
                for (int i = 0; i < pathListOfCoordinates.Count; i++)
                {
                    Debug.Log(pathListOfCoordinates[i].name);
                }
                
                Debug.Log(",,,,,,,,,,,Add Stears connectors,,,,,,,,,,,,");
                var endTempFloorObject = endFloor.GetChild(endFloor.childCount - 1);
                
                for (int i = endTempFloorObject.childCount-1; i >= indexOfConnectorEndPoint; i--)
                { 
                    pathListOfCoordinates.Add(endTempFloorObject.GetChild(i));
                }
                
                pathListOfCoordinates.Add(endPoint);
                
                Debug.Log("=============Add EndFloor connectors============");
                for (int i = 0; i < pathListOfCoordinates.Count; i++)
                {
                    Debug.Log(pathListOfCoordinates[i].name);
                }
                Debug.Log("=============Add EndFloor connectors============");

            //Зібрали позицію сходів, які знаходяться на початковому поверсі праворуч
        }
//======================================================================================================================
                                                                                                                        //Будуємо шлях який студент має пройти використовуючи сходи
//======================================================================================================================
        Debug.Log("=========DrawPath==========");
        for (int i = 0; i < pathListOfCoordinates.Count; i++) 
        {
            Debug.Log(pathListOfCoordinates[i].name);
        }
        Debug.Log("=========DrawPath=========="); 
        }
        return pathListOfCoordinates;
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
        //============================================================================================
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
        //==============================================================================================================
        var PathListOfCoordinates = FindPath(start_room, end_room);                                                     // отрисовка всех точек которые участвуют в потсроении пути.   
        
        //==============================================================================================================
        int amountOfPathDrawers = FindAmount(PathListOfCoordinates);                                                    // нахождение нужного количества Генераторов частиц 
        
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
   
    
    private GameObject activeTouch;
    private GameObject previosTouch;
    List<float> touch_positions_x = new List<float>(10){};
    List<float> touch_positions_y = new List<float>(10){};
    public float RotationalSpeed = 0.7f;
    public int waiting_step = 2;
    public int length = 6;
    float angle;

    public GameObject PathRootObject;
    public Camera camer;
    private float distance_active_frame;
    private float distance_lact_frame;
    private int frameElapsedCounter = 0;
    public float ZoomingСoefficient = 0.7f;

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

                        activeTouch.transform.position = new Vector2(touch_positions_x[0],touch_positions_y[0]);
                        previosTouch.transform.position = new Vector2(touch_positions_x[waiting_step],touch_positions_y[waiting_step]);    

                        //draw_touch(activeTouch, Color.green);
                        //draw_touch(previosTouch, Color.red); 
                    
                        angle = Vector2.SignedAngle (previosTouch.transform.position, activeTouch.transform.position); 

                        //Debug.Log(angle);
                        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                        {
                            if(Mathf.Abs(touch.position.y) <= Screen.height /2)
                            {
                                PathRoot.transform.Rotate(0, angle * RotationalSpeed, 0 , Space.World);
                            }
                            else
                            {
                                PathRoot.transform.Rotate(0, -angle * RotationalSpeed, 0, Space.World);
                            }                            
                        }
                        else
                        {
                            PathRoot.transform.Rotate(angle * RotationalSpeed, 0 , 0, Space.World);
                        }
                    
                    break;  
                
                    case TouchPhase.Ended:
                    
                        if(buttonTouch == false)
                        {
                            for( int i = 0; i < touch_positions_x.Count; i++)
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
    

    private List<Transform> vfloorList = new List<Transform>();

    private void findTheCenterOfMass()
    {
        for (int i = 0; i < PathRootObject.transform.childCount; i++) {
            if (PathRootObject.transform.GetChild(i).gameObject.activeSelf)
            {
                vfloorList.Add(PathRootObject.transform.GetChild(i));
            }
        }
        Debug.Log(vfloorList.Count);
        
        if (vfloorList.Count > 1)
        {
            float maxXPosition = 0; 
            float minXPosition = 0;
            float maxYPosition = 0;
            float minYPosition = 0;
            float maxZPosition = 0;
            float minZPosition = 0;
            
            for(int i = 0; i < vfloorList.Count; i++)
            {
                var tempFloor = vfloorList[i];
            
                if (tempFloor.position.x > maxXPosition)
                {
                    maxXPosition = tempFloor.position.x;
                }

                if (tempFloor.position.x < minXPosition)
                {
                    minXPosition = tempFloor.position.x;
                }
            
                if (tempFloor.position.y > maxYPosition)
                {
                    maxYPosition = tempFloor.position.y;
                }

                if (tempFloor.position.y < minYPosition)
                {
                    minYPosition = tempFloor.position.y;
                }
            
                if (tempFloor.position.z > maxZPosition)
                {
                    maxZPosition = tempFloor.position.z;
                }

                if (tempFloor.position.z < minZPosition)
                {
                    minZPosition = tempFloor.position.z;
                }
            }

            Vector3 centrePosition = Vector3.Lerp(new Vector3(minXPosition, minYPosition, minZPosition),
                new Vector3(maxXPosition, maxYPosition,maxZPosition), 0.5f);
        
            Debug.Log($"minX : {minXPosition} | minY : {minYPosition} | minZ : {minZPosition}");
            Debug.Log($"centreX : {centrePosition.x} | centreY : {centrePosition.y} | centreZ : {centrePosition.z}");
            Debug.Log($"maxX : {maxXPosition} | maxY : {maxYPosition} | maxZ : {maxZPosition}");

            List<GameObject> tempList = new List<GameObject>();
            for (int i = 0; i < PathRoot.transform.childCount; i++) {
                tempList.Add(PathRoot.transform.GetChild(i).gameObject);
            }

            PathRoot.transform.DetachChildren();
            
            PathRoot.transform.position = centrePosition;
            
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList[i].transform.SetParent(PathRoot.transform);
            }
        }
    }
    public void SetStartPosition()
    {
        Camera.main.transform.position = new Vector3(0, 250, -11);
        Camera.main.fieldOfView = 72.9f;

        PathRoot.transform.position = new Vector3(-7.8f, -31.0f, -20.4f);
        PathRoot.transform.rotation = Quaternion.Euler(0, 0, 0); 
    }
    public void StartDraw()
    {
        ClearPathDraws();
        findTheCenterOfMass();
        StartCoroutine(DrawPath());
    }
    public void StopDraw()
    {
        ClearPathDraws();
        StopAllCoroutines();
    }
    private void Start()
    {
        SetStartPosition();
        floorsList = GameObject.FindGameObjectsWithTag("Floor");
        frameElapsedCounter = 0;
        activeTouch = new GameObject();
        previosTouch = new GameObject();
        touch_positions_x = new List<float>(10){};
        touch_positions_y = new List<float>(10){};
        buttonTouch = false;
    }

    void Update()
    {
        Debug.Log($"Camera.main.transform.position : {Camera.main.transform.position}");
        Debug.Log($"Camera.main.fieldOfView : {Camera.main.fieldOfView}");
        Debug.Log($"PathRoot.transform.position : {PathRoot.transform.position}");
        Debug.Log($"PathRoot.transform.rotation : {PathRoot.transform.rotation}"); 
        Debug.Log("==============================================================");
        Debug.Log($"RotationalSpeed : {RotationalSpeed}");
        Debug.Log($"ZoomingСoefficient : {ZoomingСoefficient}");
        Debug.Log("==============================================================");
        // Debug.Log("activeTouch x:" + activeTouch.transform.position.x + " |  activeTouch y:" + activeTouch.transform.position.y);
        // Debug.Log("previosTouch x:" + previosTouch.transform.position.x + " |  previosTouch y:" + previosTouch.transform.position.y);
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
