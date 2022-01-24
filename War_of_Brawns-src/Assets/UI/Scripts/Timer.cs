using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float timeStart;
    public int pushupcount;
    public int squatcount;
    public Text textboxCount;
    public Text textBox;
    public GameObject pauseIcon;
    public GameObject playIcon;

    bool timerActive = false;
    // Start is called before the first frame update
    void Start()
    {
        if (textBox != null)
            textBox.text = timeStart.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            timeStart += Time.deltaTime;
            textBox.text = timeStart.ToString("F2");
            pauseIcon.SetActive(true);
            playIcon.SetActive(false);
        }

    }
    public void timerButton()
    {
        timerActive = !timerActive;
        pauseIcon.SetActive(false);
        playIcon.SetActive(true);

    }

    public void saveTime()
    {
        PlayerPrefs.SetFloat("Running Time", timeStart);
    }
}
