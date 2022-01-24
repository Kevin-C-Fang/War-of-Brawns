using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject Show;
    public GameObject Hide;

    public void pauseDisplay()
    {
        Show.SetActive(true);
        Hide.SetActive(false);
    }
}
