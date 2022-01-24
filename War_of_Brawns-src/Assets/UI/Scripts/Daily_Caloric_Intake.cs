using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class Daily_Caloric_Intake : MonoBehaviour
{
	public Firebase.Auth.FirebaseAuth auth;
	private Firebase.Auth.FirebaseUser user;
	public int currentWeight, goalWeight, height, activityLevel, goalPerWeek, age;
	public string sex;
	private Info info;
	
	private async void Awake()
	{
		auth = GameObject.Find("Data Storage").GetComponent<dataStorage>().auth;
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance
			.SetEditorDatabaseUrl("https://war-of-brawns.firebaseio.com/");
		info = new Info();
		await getStats();
		Debug.Log("DCI: info saved as: ");
		info.Out(); //whatever

		Initial_calculation(currentWeight,  goalWeight,  height,  age,  sex,  activityLevel,  goalPerWeek);
		//Initial_calculation(info.currentWeight, info.goalWeight, info.height, info.age, info.sex, info.activityLevel, info.goalPerWeek);
	}

    // Start is called before the first frame update
    void Start()
    {
		
		// Possible values: sedentary(x1.2), Lightly active(x1.375),
		// Mod active(x1.55), very active(x1.725), Extreme active(x1.9)
	}

	public async Task getStats()
	{
		var task = await FirebaseDatabase.DefaultInstance.GetReference("players/" + user.UserId + "/dietJournal")
		.GetValueAsync().ContinueWith(t => t);
		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.LogError("Database Read faulted: " + task.Exception);
			return;
		}
		DataSnapshot snapshot = task.Result;
		info = JsonUtility.FromJson<Info>(snapshot.GetRawJsonValue());
		setInfo();
		Debug.Log("inner call from get stats");
		info.Out();
		return;
	}

	void setInfo()
    {
		currentWeight = (int)info.currentWeight;
		goalWeight = info.goalWeight;
		height = (int)info.height;
		sex = info.sex;
		activityLevel = (int)info.activityLevel;
		goalPerWeek = info.goalPerWeek;
		age = info.age;
	}

	// Update is called once per frame - every frame
	void Update()
    {

    }

	// Calculation that is based on sex of the user
	// Will calculate the BMR, TDEE, and finally the suggested caloric intake.
	// Caloric intake will be stored in variable for UI designer to display on diet journal page.
	void Initial_calculation(double current_Weight, int goal_Weight, double height, int age, string sex, double activity_level, int weight_Loss_Gain_Range)
    {
		int BMR, TDEE;
		int suggested_Caloric_Intake;

		// Check if sex is male or female
		if (sex == "male")
		{
			BMR = (int)((10 * current_Weight) + (6.25 * height) - (5 * age) + 5);   // Mifflin-St Jeor Formula - Male
			TDEE = TDEE_Calculation(BMR, activity_level);
			suggested_Caloric_Intake = check_Current_VS_Goal(current_Weight, goal_Weight, TDEE, weight_Loss_Gain_Range);
			if (suggested_Caloric_Intake < 1500)
			{
				// If suggested caloric intake is less than 1500, then add the 500 because 1500 is the lowest recommended
				suggested_Caloric_Intake = suggested_Caloric_Intake + 500;
			}
			//Store suggested_Caloric_Intake in database
			///Debug.Log(suggested_Caloric_Intake);
			PlayerPrefs.SetInt("DailyCaloricIntake", suggested_Caloric_Intake);
		}
		else if (sex == "female")
		{
			BMR = (int)((10 * current_Weight) + (6.25 * height) - (5 * age) - 161); // Mifflin-St Jeor Formula - Female
			TDEE = TDEE_Calculation(BMR, activity_level);
			suggested_Caloric_Intake = check_Current_VS_Goal(current_Weight, goal_Weight, TDEE, weight_Loss_Gain_Range);
			if (suggested_Caloric_Intake < 1200)
			{
				// If suggested caloric intake is less than 1200, then add 500 because 1200 is the lowest recommended
				suggested_Caloric_Intake = suggested_Caloric_Intake + 500;
			}
			//Store suggested_Caloric_Intake in database
			///Debug.Log(suggested_Caloric_Intake);
			PlayerPrefs.SetInt("DailyCaloricIntake", suggested_Caloric_Intake);
		}
		else    // Makes sure that gender is either Male or Female
		{
			///Debug.Log("Gender is not correct or missing!");
			///Debug.Log("Error: Change gender to either Male or Female!");
		}
	}

	// Multiplies the BMR by the activity level.
	static int TDEE_Calculation(double BMR, double activity_level)
	{
		return (int)(BMR * activity_level);
	}

	// Checks the current weight against the goal weight.
	// If the user wants to lose/gain weight, it depends on the 1-2lb weight loss/gain value in weight_Loss_Gain_Range variable.
	static int check_Current_VS_Goal(double current_Weight, double goal_Weight, double TDEE, int weight_Loss_Gain_Range)
	{
		if (current_Weight > goal_Weight) // Loss weight
		{
			if (weight_Loss_Gain_Range == 1)    // User wants a 1lb loss per week
			{
				return ((int)(TDEE - 500));
			}
			else                                // User wants a 2lb loss per week
			{
				return ((int)(TDEE - 1000));
			}
		}
		else if (current_Weight < goal_Weight) //Gain weight
		{
			if (weight_Loss_Gain_Range == 1)         // User wants a 1lb gain per week
			{
				return ((int)(TDEE + 500));
			}
			else                                     // User wants a 2lb gain per week
			{
				return ((int)(TDEE + 1000));
			}
		}
		else    // If here, then TDEE is fine because current weight is equal to goal weight
		{
			return ((int)TDEE);
		}
	}
}

public class Info
{
	public double activityLevel;
	public int age;
	public double currentWeight;
	public int goalPerWeek;
	public int goalWeight;
	public double height;
	public string sex;

	public void Out()
    {
		Debug.Log(activityLevel);
		Debug.Log(age);
		Debug.Log(currentWeight);
		Debug.Log(goalPerWeek);
		Debug.Log(goalWeight);
		Debug.Log(height);
		Debug.Log(sex);
    }
}