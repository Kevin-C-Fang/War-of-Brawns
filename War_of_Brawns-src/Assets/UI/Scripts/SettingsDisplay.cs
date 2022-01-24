using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class SettingsDisplay : MonoBehaviour
{
    //Declare
   /// public GameObject settingsWindow; //for pop up window
    public Text cw, gw, h, s, al, gpw, a;
    private int currentWeight, goalWeight, height, sex, activityLevel, goalPerWeek, age;
    public Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Info info;
    // Start is called before the first frame update

    private async void Awake()
    {
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;
        FirebaseApp.DefaultInstance
            .SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");
        info = new Info();
        await getStuff();
        setInfo();
    }

    void setInfo()
    {
        //// THIS IS WHERE YOU ASSIGN THE RIGHT VALUE FROM DATABASE. Do not re-name variables.
        double currentWeight = (int)info.currentWeight;
        int goalWeight = info.goalWeight;
        double height = (int)info.height;
        string sex = info.sex;
        double activityLevel = (int)info.activityLevel;
        int goalPerWeek = info.goalPerWeek;
        int age = info.age;

        //// DO NOT CHANGE THIS CODE. This code is what lets it display on the screen.
        cw.text = currentWeight.ToString();
        gw.text = goalWeight.ToString();
        h.text = height.ToString();
        al.text = activityLevel.ToString();
        gpw.text = goalPerWeek.ToString();
        s.text = sex;
        a.text = age.ToString();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task getStuff()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/dietJournal")
        .GetValueAsync().ContinueWith(t => t);
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Database Read faulted: " + task.Exception);
            return;
        }
        DataSnapshot snapshot = task.Result;
        info = JsonUtility.FromJson<Info>(snapshot.GetRawJsonValue());
        info.Out();
        return;
    }
}