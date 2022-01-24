using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDungeon : MonoBehaviour
{
    public GameObject victoryScreen; 
    public GameObject defeatScreen;
    public GameObject healthBar;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Displays victory screen, disables UI, shows player victory pose
    public void showVictory()
    {
        FindObjectOfType<AudioManager>().Stop("DungeonBGM");
        FindObjectOfType<AudioManager>().Stop("DungeonAmbient");
        FindObjectOfType<AudioManager>().Play("Victory");

        playerAnimator.SetBool("Victory", true);
        victoryScreen.SetActive(true);
        healthBar.SetActive(false);
    }

    //Displays defeat screen, disables UI
    public void showDefeat()
    {
        FindObjectOfType<AudioManager>().Stop("DungeonBGM");
        FindObjectOfType<AudioManager>().Stop("DungeonAmbient");
        FindObjectOfType<AudioManager>().Play("Defeat");

        defeatScreen.SetActive(true);
        healthBar.SetActive(false);
    }
}
