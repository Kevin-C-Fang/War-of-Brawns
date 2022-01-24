using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class DietJournalDisplay : MonoBehaviour
{
    public Text bc1, bc2, lc1, lc2, dc1, dc2, snc1, snc2, DailyCalorieIntake, TotalFoodCalorie, RemainingCalorie, doneReaminingCalorie;
    public Text b1, b2, l1, l2, d1, d2, sn1, sn2;
    private int brcal1, brcal2, lunchcal1, lunchcal2, dinnercal1, dinnercal2, snackcal1, snackcal2, Daily_Calorie_Intake, Total_Food_Calorie, Remaining_Calorie, button;
    private string breakfast1, breakfast2, lunch1, lunch2, dinner1, dinner2, snack1, snack2;
    public GameObject Panel; // which panel


    // Start is called before the first frame update
    void Update()
    {

        if (PlayerPrefs.GetInt("buttonCounter") != null)
        {
            button = PlayerPrefs.GetInt("buttonCounter");
            if(button == 1)
            {
               Panel.SetActive(true);
            }

        }

        /// DO NOT change the variable names.
        ///Breakfast Food Name RETRIEVE FROM DATABASE HERE
        ///
        breakfast1 = PlayerPrefs.GetString("Breakfast1");
        breakfast2 = PlayerPrefs.GetString("Breakfast2");

        ///Breakfast Calorie RETRIEVE FROM DATABASE HERE


        brcal1 = PlayerPrefs.GetInt("Breakfastcal1");
        brcal2 = PlayerPrefs.GetInt("Breakfastcal2");


        ///Lunch Food Name RETRIEVE FROM DATABASE HERE
        lunch1 = PlayerPrefs.GetString("Lunch1");
        lunch2 = PlayerPrefs.GetString("Lunch2");

        ///Lunch Calorie RETRIEVE FROM DATABASE HERE
        lunchcal1 = PlayerPrefs.GetInt("Lunchcal1");
        lunchcal2 = PlayerPrefs.GetInt("Lunchcal2");

        ///Dinner Food Name RETRIEVE FROM DATABASE HERE
        dinner1 = PlayerPrefs.GetString("Dinner1");
        dinner2 = PlayerPrefs.GetString("Dinner2");

        ///Dinner Calorie RETRIEVE FROM DATABASE HERE
        dinnercal1 = PlayerPrefs.GetInt("Dinnercal1"); ;
        dinnercal2 = PlayerPrefs.GetInt("Dinnercal2");

        ///Snack Food Name RETRIEVE FROM DATABASE HERE
        snack1 = PlayerPrefs.GetString("Snack1");
        snack2 = PlayerPrefs.GetString("Snack2");

        ///Dinner Calorie RETRIEVE FROM DATABASE HERE
        snackcal1 = PlayerPrefs.GetInt("Snackcal1"); ;
        snackcal2 = PlayerPrefs.GetInt("Snackcal2"); ;

        //calculate total food calories for the day



        Total_Food_Calorie = brcal1 + brcal2 + lunchcal1 + lunchcal2 + dinnercal1 + dinnercal2 + snackcal1 + snackcal2;


        ///PULL THIS INFO FROM DATABASE
        Daily_Calorie_Intake = PlayerPrefs.GetInt("DailyCaloricIntake");

        //calculate remaing calories for the day
        Remaining_Calorie = Daily_Calorie_Intake - Total_Food_Calorie;





        //// DO NOT CHANGE CODE. This makes the value show up on the UI.
        ///
        if (brcal1 != 0)
        {
            bc1.text = brcal1.ToString();
        }
        if (brcal2 != 0)
        {
            bc2.text = brcal2.ToString();
        }
        if (lunchcal1 != 0)
        {
            lc1.text = lunchcal1.ToString();
        }
        if (lunchcal2 != 0)
        {
            lc2.text = lunchcal2.ToString();
        }
        if (dinnercal1 != 0)
        {
            dc1.text = dinnercal1.ToString();
        }
        if (dinnercal2 != 0)
        {
            dc2.text = dinnercal2.ToString();
        }
        if (snackcal1 != 0)
        {
            snc1.text = snackcal1.ToString();
        }
        if (snackcal2 != 0)
        {
            snc2.text = snackcal2.ToString();
        }



        DailyCalorieIntake.text = Daily_Calorie_Intake.ToString();
        RemainingCalorie.text = Remaining_Calorie.ToString();
        TotalFoodCalorie.text = Total_Food_Calorie.ToString();
        doneReaminingCalorie.text = Remaining_Calorie.ToString();

        b1.text = breakfast1;
        b2.text = breakfast2;
        l1.text = lunch1;
        l2.text = lunch2;
        d1.text = dinner1;
        d2.text = dinner2;
        sn1.text = snack1;
        sn2.text = snack2;




    }



    public void buttonCounting()
    {
        PlayerPrefs.SetInt("buttonCounter", 1);
    }
/*
    private async void Awake()
    {
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;
        FirebaseApp.DefaultInstance
            .SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");
        history = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("history");
        bool called = false;
        if (called)
        {
            await storeStuff();
            Debug.Log("New Weight update created.");
        }
    }

    public async Task storeStuff()
    {
        current = FirebaseDatabase.DefaultInstance.RootReference.Child("players")
            .Child(user.UserId).Child("dietJournal").Child("currentWeight");
        await current.SetValueAsync(w);
        Debug.Log("Current Weight set as " + w);
        //auto generate new update for progress tracking
        history = history.Push();
        Dictionary<string, object> newUpdate = new Dictionary<string, object>();
        newUpdate["date"] = DateTime.Now;
        newUpdate["weight"] = w;
        await history.SetValueAsync(newUpdate);
    }

    public async Task getStats()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/avatar")
        .GetValueAsync().ContinueWith(t => t);
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
*/
}
