using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Firebase.Auth.FirebaseAuth;

public class Login : MonoBehaviour
{
    public Firebase.Auth.FirebaseUser user;
    public Firebase.Auth.FirebaseAuth auth;
    private string email, password;
    private bool clicked;
 
    void Awake()
    {
        PlayerPrefs.DeleteAll();  //Deletes all player data
        InitializeFirebase();
        clicked = false;
        //uncomment either of these lines AND
        //the email and password fields in login to login w/out typing
        //email = "tnova002@odu.edu";
        //password = "firebase";
        //email = "blueberryrando@gmail.com";
        //password = "theultimate434!";
    }

    async void Start()
    {
        if (auth.CurrentUser == user && user != null)
        {
            GameObject.Find("Data Storage").GetComponent<dataStorage>().auth = auth;
            SceneManager.LoadScene("Home");
        }
        if(clicked == true)
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            GameObject.Find("Data Storage").GetComponent<dataStorage>().auth = auth;
            SceneManager.LoadScene("Home");
        }
    }

    public void login()
    {
        email = GameObject.Find("Email").GetComponent<InputField>().text;
        password = GameObject.Find("Password").GetComponent<InputField>().text;
        clicked = true;
        Start();
    }

    public void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Authentication Call | Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Authentication Call | Signed in " + user.UserId);
            }
        }
    }
}