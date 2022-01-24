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

public class Weight : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text cw;
    private string weight;
    private int w, count;
    private dbUpdate u;
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
        if (weight == null)
            cw.text = "0 lbs";
        else
            cw.text = weight + " lbs";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("currentWeight");
        await current.SetValueAsync(w);
        await num();
        Debug.Log("Current Weight set as " + w);
        //auto generate new update for progress tracking
        DatabaseReference r = history.Push();
        await r.SetValueAsync(w);
        //Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
        //childUpdates["/week " + ] = entryValues;
    }
    
    public async Task num()
    {
        var task = await history.GetValueAsync().ContinueWith(t => t);
        if(task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Database Read faulted: " + task.Exception);
            return;
        }
        DataSnapshot snapshot = task.Result;
        count = (int)snapshot.ChildrenCount;
        Debug.Log("number of child nodes in history: " + count);
        return;
    }

    public async void StoreWeight()
    {
        weight = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = weight;
        //update the weight using typecasted integer from input field
        w = 0;
        if (!int.TryParse(weight, out w))
        {
            Debug.LogError("Entered weight is not valid");
            return;
        }
        int.TryParse(weight, out w);
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("currentWeight");
        await storeStuff();
        return;
    }
}
public class dbUpdate
{
    public int week;
    public int weight;
    public void printUpdate()
    {
        Debug.Log("week: " + week);
        Debug.Log("weight: " + weight);
    }
}