using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    //unpauses the ame by hiding the pause canvas, unfreezing time for anything in FixedUpdate, and turning the pause bool off

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
       
        //Locks the cursor back to the center and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }

    //pauses the ame by bringing up the pause canvas, freezing time for anything in FixedUpdate, and turning the pause bool on
    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        //Allows buttons to be clicked
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu()
    {
        //unpauses the game by returning the time scale back to normal
        Time.timeScale = 1f;
        //specifically loads the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
