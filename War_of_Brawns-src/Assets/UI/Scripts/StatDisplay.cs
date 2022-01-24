using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class StatDisplay : MonoBehaviour //class is fully functional in its current state
{
    //Declaring text and int fields for all stats.
    public Text end, str, def, heal, name;
    public Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    public int endurance, strength, defense, health;
    private string username;
    public Avatar avatar; //special holding class for stat variables
    
    private async void Awake()
    {
        //initialize Firebase resources
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;
        Debug.Log("Login Call | current user: " + user.UserId);
        Avatar avatar = new Avatar();

        //get username
        await getUsername();
        name.text = username;
        await getStats();
        end.text = endurance.ToString();
        str.text = strength.ToString();
        def.text = defense.ToString();
        heal.text = health.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task getUsername()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/username")
                .GetValueAsync().ContinueWith(t =>t);
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Database Read faulted: " + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            username = snapshot.Value.ToString();
            Debug.Log("Retrieved from database: " + username);
            return;
    }

    public async Task getStats()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/avatar")
        .GetValueAsync().ContinueWith(t =>t);
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Database Read faulted: " + task.Exception);
            return;
        }
        DataSnapshot snapshot = task.Result;
        avatar = JsonUtility.FromJson<Avatar>(snapshot.GetRawJsonValue());
        GameObject.Find("Data Storage").GetComponent<dataStorage>()
            .setStats(avatar.strength, avatar.defense, avatar.endurance, avatar.health);
        endurance = avatar.endurance;
        strength = avatar.strength;
        defense = avatar.defense;
        health = avatar.health;
        return;
    }
}

public class Avatar
{
    public int defense;
    public int endurance;
    public int health;
    public int level;
    public int strength;
    public int xp;

    public void Out()
    {
        Debug.Log("defense: " + defense);
        Debug.Log("endurance: " + endurance);
        Debug.Log("health: " + health);
        Debug.Log("level: " + level);
        Debug.Log(strength);
        Debug.Log(xp);
    }
}