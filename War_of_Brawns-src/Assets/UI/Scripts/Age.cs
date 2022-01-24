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

public class Age : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text year;
    private string age;
    private int a;
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
        if (age == null)
            year.text = "0 years";
        else
            year.text = age + " years";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("age");
        await current.SetValueAsync(a);
        Debug.Log("Age set as " + a);
        //auto generate new update for progress tracking
    }

    public async void StoreAge()
    {
        age = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = age;
        //update the age using typecasted integer from input field
        a = 0;
        if (!int.TryParse(age, out a))
        {
            Debug.LogError("Entered age is not valid");
            return;
        }
        int.TryParse(age, out a);
        await storeStuff();
        return;
    }
}