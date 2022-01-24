using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindowPopUp : MonoBehaviour
{

    public GameObject Window;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void popup()
    {
        Window.SetActive(true);
    }

    // Update is called once per frame
    public void closeWindow()
    {
        Window.SetActive(false);
    }
}
