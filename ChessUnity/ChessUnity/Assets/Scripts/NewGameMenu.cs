using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{
    //PlayerPrefs prefs = new PlayerPrefs();
    //public int mode;

    public void StartMateinOne()
    {
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartMateinTwo()
    {
        PlayerPrefs.SetInt("mode", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartMateinThree()
    {
        PlayerPrefs.SetInt("mode", 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartOnlineGame()
    {
        PlayerPrefs.SetInt("mode", 4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGameAI()
    {
        PlayerPrefs.SetInt("mode", 5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
