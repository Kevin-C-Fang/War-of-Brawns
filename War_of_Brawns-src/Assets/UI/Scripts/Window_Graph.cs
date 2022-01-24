using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class update
{
    public int weight;
}


public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    public List<GameObject> gameObjectList;
    private List<int> valueList1;
    private int goalPerWeek, currentWeight;
    private Info info;
    public Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;

    private async void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
        List<int> updates = new List<int>();
        
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");

        /*foreach(int i in updates){
            Debug.Log("Values: " + i);
        }*/
        //*******************************************************NEEDS UPDATE WITH THE DATA FROM THE DATABASE*********************************************//
        valueList1 = new List<int>(); //Fetch the list of weights here.
        //int goalPerWeek = 4; //Fetch goal per week from the database.
        
        await getGoal();
        await getHistory();
//***********************************************************************************************************************************************//


        int currentWeight = valueList1[0];

        List<int> valueList = new List<int>();
        valueList.Add(currentWeight);

        for (int i = 1; i < valueList1.Count; i++){
            currentWeight = currentWeight - goalPerWeek;
            valueList.Add(currentWeight);
        }

        foreach(int value in valueList){
            Debug.Log("Values: " + value);
        }

        ShowGraph(valueList, valueList1);
        ShowGraph1(valueList1);
    }

    private async Task getHistory()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/history")
            .GetValueAsync().ContinueWith(t => t);
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Database read is faulted: " + task.Exception);
            Debug.Log("ERROR");
            return;
        }
        
        DataSnapshot snapshot = task.Result;
        
        foreach (var Child in snapshot.Children)
        {
            //string r = child.Reference
            Debug.Log(Child.Value);
            int w = Convert.ToInt32(Child.Value);
            //int w = Child.Value;
            //int weight = (int)w;
            valueList1.Add(w);
        }
    }

    private async Task getGoal()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/dietJournal")
            .GetValueAsync().ContinueWith(t => t);
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Database read is faulted: " + task.Exception);
            Debug.Log("ERROR");
            return;
        }

        DataSnapshot snapshot = task.Result;
        info = JsonUtility.FromJson<Info>(snapshot.GetRawJsonValue());
        currentWeight = (int)info.currentWeight;
        goalPerWeek = info.goalPerWeek;
        info.Out();
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph1(List<int> valueList) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 0f;
        float xSize = 100f;


        foreach (int value in valueList) {
            if (value > yMaximum){
                yMaximum = value;
            }
        }

        yMaximum = yMaximum * 1.2f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if (lastCircleGameObject != null) {

                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);}

            lastCircleGameObject = circleGameObject;
        }
    }

    private void ShowGraph(List<int> valueList, List<int> valueList1) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 0f;
        float xSize = 100f;

        foreach (int value in valueList1) {
            if (value > yMaximum){
                yMaximum = value;
            }
        }

        yMaximum = yMaximum * 1.2f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if (lastCircleGameObject != null) {
                CreateDotConnection1(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = "Week " + (1+ i).ToString();
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(45f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum) + " lb".ToString();
        }
    }


    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private void CreateDotConnection1(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(0,1,0, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }



    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

}

/*public class Info
{
    public double activityLevel;
    public int age;
    public double currentWeight;
    public int goalPerWeek;
    public int goalWeight;
    public double height;
    public string sex;

    public void Out()
    {
        Debug.Log(activityLevel);
        Debug.Log(age);
        Debug.Log(currentWeight);
        Debug.Log(goalPerWeek);
        Debug.Log(goalWeight);
        Debug.Log(height);
        Debug.Log(sex);
    }
}*/