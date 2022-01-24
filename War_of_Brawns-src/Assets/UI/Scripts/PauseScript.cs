using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseIcon, playIcon;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseIcon.SetActive(false);
        playIcon.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = true;
    }

    public void Pause()
    {
        pauseIcon.SetActive(true);
        playIcon.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


}
