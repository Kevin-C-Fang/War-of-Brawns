using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class GoalPerWeek : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text goalpw;
    private string goalPerWeek;
    private int gpw;
    private DatabaseReference history, current;

    private void Awake()
    {
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;
        FirebaseApp.DefaultInstance
            .SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");
        history = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("history");
    }

    // Start is called before the first frame update
    void Start()
    { 
        if (goalPerWeek == null || int.Parse(goalPerWeek) == 0)
            goalpw.text = "0 lbs";
        else if (int.Parse(goalPerWeek) < 0)
            goalpw.text = goalPerWeek.Substring(1) + " lbs";
        else if (int.Parse(goalPerWeek) > 0)
            goalpw.text = "-" + goalPerWeek + " lbs";

    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("goalPerWeek");
        await current.SetValueAsync(gpw);
        Debug.Log("Goal Per Week set as " + gpw);
        //auto generate new update for progress tracking
    }

    public async void StoreGoalPerWeek()
    {
        goalPerWeek = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = goalPerWeek;
        //update the goal per weekl using typecasted integer from input field
        gpw = 0;
        if (!int.TryParse(goalPerWeek, out gpw))
        {
            Debug.LogError("Entered goal per week is not valid");
            return;
        }
        int.TryParse(goalPerWeek, out gpw);
        await storeStuff();
        return;
    }
}