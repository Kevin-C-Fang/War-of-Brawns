using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GoogleSignIn : MonoBehaviour
{
    // This is the script that is linked with the Google sign in button on Login page.
    // Start is called before the first frame update
    public void SignIn()
    {
        /// This line of code loads the home page.
        /// This should only be done if sign in through google was successful.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
