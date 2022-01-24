using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CustomEntry : MonoBehaviour
{
    public string foodName;
    public string calories;
    public int cal;
    public GameObject foodInput;
    public GameObject calorieInput;

    // Start is called before the first frame update
    public void storeFood()
    {



        ///Getting user entry
        foodName = foodInput.GetComponent<Text>().text;
        calories = calorieInput.GetComponent<Text>().text;

        //change calories to int
        cal = int.Parse(calories);

        ///Getting the scene name to add to accurate list
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        ///Store both values in database may need to change calories to int
        ///
        if (sceneName == "Breakfast Custom Entry")
        {
            /// ADD DATA TO BREAKFAST LIST
            PlayerPrefs.SetInt("Breakfastcal1", cal);
            PlayerPrefs.SetString("Breakfast1", foodName);

        }

        if (sceneName == "Lunch Custom Entry")
        {
            PlayerPrefs.SetInt("Lunchcal1", cal);
            PlayerPrefs.SetString("Lunch1", foodName);
        }

        if (sceneName == "Dinner Custom Entry")
        {
            /// ADD DATA TO DINNER LIST
            PlayerPrefs.SetInt("Dinnercal1", cal);
            PlayerPrefs.SetString("Dinner1", foodName);
        }

        if (sceneName == "Snack Custom Entry")
        {
            /// ADD DATA TO SNACK LIST
            PlayerPrefs.SetInt("Snackcal1", cal);
            PlayerPrefs.SetString("Snack1", foodName);
        }
    }
}
