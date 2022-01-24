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

public class GoalWeight : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text goal;
    private string goalWeight;
    private int gw;
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
        if (goalWeight == null)
            goal.text = "0 lbs";
        else
            goal.text = goalWeight + " lbs";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("goalWeight");
        await current.SetValueAsync(gw);
        Debug.Log("Goal Weight set as " + gw);
        //auto generate new update for progress tracking
    }

    public async void StoreGoalWeight()
    {
        goalWeight = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = goalWeight;
        //update the goal weight using typecasted integer from input field
        gw = 0;
        if (!int.TryParse(goalWeight, out gw))
        {
            Debug.LogError("Entered goal weight is not valid");
            return;
        }
        int.TryParse(goalWeight, out gw);
        await storeStuff();
        return;
    }
}