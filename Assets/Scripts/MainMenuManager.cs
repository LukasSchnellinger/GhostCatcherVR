using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GhostCatcherVR"); // Passe den Namen deiner Spielszenen-Datei an
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
