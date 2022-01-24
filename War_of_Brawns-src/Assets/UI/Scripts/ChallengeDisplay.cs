using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChallengeDisplay : MonoBehaviour
{
    public int PushupCount, SquatCount;
    public float RunningTime;
    public Text runningtime, pushupcount, squatcount;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Squat Count", 0);
        PlayerPrefs.SetInt("Pushup Count", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetFloat("Running Time") != 0)
        {
            RunningTime = PlayerPrefs.GetFloat("Running Time");
            runningtime.text = RunningTime.ToString();
        }


        PushupCount = PlayerPrefs.GetInt("Pushups");
        pushupcount.text = PushupCount.ToString();

        SquatCount = PlayerPrefs.GetInt("Squats");
        squatcount.text = SquatCount.ToString();
    }
}
