using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime = 300f; // 5 Minuten in Sekunden
    private float remainingTime;

    public Text timerText;
    public Text killCounterText;

    private int ghostKills = 0;

    void Start()
    {
        remainingTime = gameTime;
        UpdateUI();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"Zeit: {minutes:00}:{seconds:00}";
        }

        if (killCounterText != null)
        {
            killCounterText.text = $"Kills: {ghostKills}";
        }
    }

    public void AddKill()
    {
        ghostKills++;
        UpdateUI();
    }

    void EndGame()
    {
        Debug.Log("Spielzeit vorbei!");
        
    }
}