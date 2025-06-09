using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("GhostCatcherVR");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
