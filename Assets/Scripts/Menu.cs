using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Use this for initialization
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Application.Quit();
    }
}