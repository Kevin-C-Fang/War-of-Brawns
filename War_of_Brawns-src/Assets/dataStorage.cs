using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using static Firebase.Auth.FirebaseAuth;

public class dataStorage : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public int strength, defense, endurance, health;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }  

    public void setStats(int s, int d, int e, int h)
    {
        strength = s;
        defense = d;
        endurance = e;
        health = h;
    }
/*  void Start()
    {
        Debug.LogFormat("datastorage exists");
    }

    // Update is called once per frame
    void Update()
    {

    }
    */
}
