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

public class Sex : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public GameObject inputField;
    public GameObject textDisplay;
    public Text sx;
    private string sex, s;
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
        if (sex == null || sex == "")
            sx.text = "None";
        else if(sex == "m" || sex == "male")
            sx.text = "Male";
        else if (sex == "f" || sex == "female")
            sx.text = "Female";
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("sex");
        await current.SetValueAsync(sex);
        Debug.Log("Sex set as " + sex);
        //auto generate new update for progress tracking
    }

    public async void StoreSex()
    {
        sex = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = sex;
        //update the sex using typecasted integer from input field
        await storeStuff();
        return;
    }
}