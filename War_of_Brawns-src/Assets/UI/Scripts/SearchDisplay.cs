using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchDisplay : MonoBehaviour
{
    public string foodItem;
    public GameObject inputFood;
    ///public GameObject foodDisplay;
    public GameObject showResult;
    public Text resultDisplay, searchDisplay;
    public int cal;
    public Text calorie;
    public string food;

    public void search()
    {
        ///Get the fooditem from the user input
        foodItem = inputFood.GetComponent<Text>().text;
        ///foodDisplay.GetComponent<Text>().text = foodItem;
        showResult.SetActive(true);
        resultDisplay.text = "Results for \"" + foodItem + "\":";

        //This is where the Databae needs to assign the right values from the search
        cal = 0;
        food = " ";

        //if no result cal = null and food = " ";


        //This goes after database stuff. Don't change, it gets assigned to screen.

        calorie.text = cal.ToString();

        searchDisplay.text = food;



        //Getting Scene name to add to accurate list
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        ///Search the database for the userinput

        ///Store it in the appropriate meal
        if (sceneName == "Breakfast Search")
        {
            /// ADD DATA TO BREAKFAST LIST
        }

        if (sceneName == "Lunch Search")
        {
            /// ADD DATA TO LUNCH LIST
        }

        if (sceneName == "Dinner Search")
        {
            /// ADD DATA TO DINNER LIST
        }

        if (sceneName == "Snack Search")
        {
            /// ADD DATA TO SNACK LIST
        }


        /// Assign the right calories to the food Item

        int calories = 0; //GET THIS FROM DATABASE


        ///Display calories
        calorie.text = calories.ToString(); //calories


    }

}
