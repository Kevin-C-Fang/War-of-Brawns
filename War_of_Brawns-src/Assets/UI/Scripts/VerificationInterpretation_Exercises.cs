using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VerificationInterpretation_Exercises : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        string dataPath;

        // Scenes have a specific input for ECG
        if(sceneName == "Running")
        {
            dataPath = @"Assets\ECG_data\ECG_data_running.txt"; /// Issue here, IDK the data path to Assets and here.
            string runningCompletion = initial_Calculation(dataPath);
            PlayerPrefs.SetString("runningCompletion", runningCompletion);
        }

        if (sceneName == "Pushups")
        {
            dataPath = @"Assets\ECG_data\ECG_data_pushups.txt"; // if this doensn't work then move to scripts.
            string pushUpsCompletion = initial_Calculation(dataPath);
            PlayerPrefs.SetString("pushUpsCompletion", pushUpsCompletion);
        }

        if (sceneName == "Squats")
        {
            dataPath = @"Assets\ECG_data\ECG_data_squats.txt";
            string squatsCompletion = initial_Calculation(dataPath);
            PlayerPrefs.SetString("squatsCompletion", squatsCompletion);
        }

        // function that pauses or plays the algorithm input/timer
        // Idea would be that there could be a function/script attached to pause/play.
        // I would write the function to pause/play algorithm.
        // From April's end

        // May need to do some kind of exception handling to register the events within the play and pause.
        /*if(play)
        {
            // play...not correct syntax
            initial_Calculation(dataPath);
        }
        else
        {
            //pause.. not correct syntax.
        }*/


        /// One BIG PROBLEM. I managed to get the result and confirmation, but I don't know how to correlate the input of ECG to the timer.

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function will take in the input from datapath in format: heart rate; in one sentence/row.
    // It will then calculate the avg of the heart rate input and compare to a hardcoded guideline
    // If it is greater than the guideline then they did enough exercise to complete the exercise
    // Would return, pass, IDK something to designate that exercise as completed.
    string initial_Calculation(string dataPath)
    {
        try
        {
            int total = 0;
            using (StreamReader sr = new StreamReader(dataPath))
            {
                string s;
                s = sr.ReadLine();

                // Split sentence into words/numbers
                string[] initial = s.Split(';');

                for (int j = 0; j < initial.Length - 1; j++)
                {
                    int next = System.Convert.ToInt32(initial[j]);
                    total += next;
                }
                int avg = total / (initial.Length - 1);

                if(avg >= 90)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        catch (System.Exception e) // If algorithm fails for any reason, then output msg below
        {
            Debug.Log("Error: Verification Interpretation Algorithm has failed.");
            return "No";
        }
    }
}
