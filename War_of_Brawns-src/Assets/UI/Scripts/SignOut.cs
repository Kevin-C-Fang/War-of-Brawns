using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Firebase.Auth.FirebaseAuth;

public class SignOut : MonoBehaviour
{
    public void signout()
    {
        Firebase.Auth.FirebaseAuth auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }
}
