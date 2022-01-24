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

public class ActivityLevel : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text act;
    private string activityLevel;
    private double al;
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
        if (activityLevel == null || int.Parse(activityLevel) < 1 || int.Parse(activityLevel) > 5)
            act.text = "None";
        else
            act.text = activityLevel;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("activityLevel");
        await current.SetValueAsync(al);
        Debug.Log("Activity Level set as " + al);
        //auto generate new update for progress tracking
    }

    public async void StoreActivityLevel()
    {
        activityLevel = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = activityLevel;
        //update the activity level using typecasted integer from input field
        al = 0;
        if (!double.TryParse(activityLevel, out al))
        {
            Debug.LogError("Entered activity level is not valid");
            return;
        }
        double.TryParse(activityLevel, out al);
        await storeStuff();
        return;
    }
}