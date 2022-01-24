using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;
using System.Threading;
using System.Threading.Tasks;

public class Exp_stat_distribution : MonoBehaviour
{
    private Firebase.Auth.FirebaseUser user;
    public Firebase.Auth.FirebaseAuth auth;
    public int str_xp, def_xp, end_xp, heal_xp;
    public Text eincrease, sincrease, dincrease, hincrease;
    private xp xp;

    // Start is called before the first frame update
    private async void Start()
    {
        auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
        user = auth.CurrentUser;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");

        await retrieveXP();

        Debug.Log("Strength total XP after retrieve: " + str_xp);

        // This block would figure out which exercises have been completed and would designate the additional exp increase.
        // Increase the xp reward exponentionally to show improvement
        int str_increase = 0, end_increase = 0, def_increase = 0, health_increase = 0;
        int str = 1, end = 1, def = 1, heal = 1;

        // str + end = def, str + end + def = HP
        // Running +end, push-ups +str, squats+str+end
        float runningCompletion = PlayerPrefs.GetFloat("Running Time");
        int pushUpsCompletion = PlayerPrefs.GetInt("Pushup Count");
        int squatsCompletion = PlayerPrefs.GetInt("Squat Count");

        // Assign multiplier of exp against the time/number of reps of exercises
        str_increase = (3 * pushUpsCompletion) + (3 * squatsCompletion);
        end_increase = (5 * (int)(runningCompletion)) + (3 * squatsCompletion);
        def_increase = end_increase + str_increase;
        health_increase = end_increase + str_increase + def_increase;

        ///save to playprefs
        PlayerPrefs.SetInt("end_increase", end_increase);
        PlayerPrefs.SetInt("str_increase", str_increase);
        PlayerPrefs.SetInt("def_increase", def_increase);
        PlayerPrefs.SetInt("health_increase", health_increase);

        str_xp += str_increase;
        end_xp += end_increase;
        def_xp += def_increase;
        heal_xp += health_increase;

        stat_Calculation(str_xp, end_xp, def_xp, heal_xp, ref str, ref end, ref def, ref heal);

        ///save new stat levels
        PlayerPrefs.SetInt("endurance", end);
        PlayerPrefs.SetInt("strength", str);
        PlayerPrefs.SetInt("defense", def);
        PlayerPrefs.SetInt("heal", heal);

        ///This should be the variable values of the stat levels.
        UpdateStats(str, end, def, heal, str_xp, end_xp, def_xp, heal_xp);
    }

    public async Task retrieveXP()
    {
        var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/xp")
            .GetValueAsync().ContinueWith(t => t);
       
            if (task.IsFaulted)
            {
                Debug.LogError("Database Read faulted: " + task.Exception);
                return;
            }
            else if (task.IsCompleted) //username = snapshot.Value.ToString();
            {
                DataSnapshot snapshot = task.Result;
                xp = JsonUtility.FromJson<xp>(snapshot.GetRawJsonValue());
            def_xp = xp.defense;
            end_xp = xp.endurance;
            heal_xp = xp.health;
            str_xp = xp.strength;
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Don't have defense more than 30 or 40
    // Have stats == 20 for prototype purposes.
    void stat_Calculation(int strXP, int endXP, int defXP, int healXP, ref int str, ref int end, ref int def, ref int heal)
    {
        // Run level/stat distribution/calculation that grabs the total exp and calculates the new stat levels.
        str = strXP / 5;
        end = endXP / 5;
        def = defXP / 10;
        heal = healXP / 15;
    }

    void UpdateStats(int str, int end, int def, int heal, int strXP, int endXP, int defXP, int healXP)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("players").Child(user.UserId).Child("avatar").Child("strength").SetValueAsync(str);
        reference.Child("players").Child(user.UserId).Child("avatar").Child("defense").SetValueAsync(def);
        reference.Child("players").Child(user.UserId).Child("avatar").Child("endurance").SetValueAsync(end);
        reference.Child("players").Child(user.UserId).Child("avatar").Child("health").SetValueAsync(heal);
        reference.Child("players").Child(user.UserId).Child("xp").Child("strength").SetValueAsync(str_xp);
        reference.Child("players").Child(user.UserId).Child("xp").Child("defense").SetValueAsync(def_xp);
        reference.Child("players").Child(user.UserId).Child("xp").Child("endurance").SetValueAsync(end_xp);
        reference.Child("players").Child(user.UserId).Child("xp").Child("health").SetValueAsync(heal_xp);
    }
}

public class xp
{
    public int defense, endurance, health, strength;
}