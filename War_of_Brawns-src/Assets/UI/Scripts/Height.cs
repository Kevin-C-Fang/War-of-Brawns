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

public class Height : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text ht;
    private string height;
    private int h;
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
        if (height == null)
            ht.text = "0 in";
        else
            ht.text = height + " in";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("height");
        await current.SetValueAsync(h);
        Debug.Log("Height set as " + h);
        //auto generate new update for progress tracking
    }

    public async void StoreHeight()
    {
        height = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = height;
        //update the height using typecasted integer from input field
        h = 0;
        if (!int.TryParse(height, out h))
        {
            Debug.LogError("Entered height is not valid");
            return;
        }
        int.TryParse(height, out h);
        await storeStuff();
        return;
    }
}