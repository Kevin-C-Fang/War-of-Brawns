using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Squats : MonoBehaviour
{

    public string squat;
    public int squats;
    public GameObject squatInput;
    public Text squatText;

    // Start is called before the first frame update

    public void storeSquats()
    {
        squat = squatInput.GetComponent<Text>().text;
        squats = int.Parse(squat);

        squatText.text = squat;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        if(sceneName == "Squats")
        {
            PlayerPrefs.SetInt("Squat Count", squats);
        }

        if(sceneName == "Pushups")
        {
            PlayerPrefs.SetInt("Pushup Count", squats);
        }
    }


}
