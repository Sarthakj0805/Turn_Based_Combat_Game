using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    public void Playgame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    public void Restartgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    } 
}
