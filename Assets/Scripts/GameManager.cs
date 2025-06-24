using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Für TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime = 300f; // 5 Minuten
    private float remainingTime;

    public TextMeshProUGUI timerText;       
    public TextMeshProUGUI killCounterText; 
    public TextMeshProUGUI gameOverText;     // ✅ NEU für Game Over Anzeige

    private int ghostKills = 0;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        remainingTime = gameTime;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false); // Anfangs ausblenden

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }

        UpdateUI();
    }

    public void AddKill()
    {
        if (gameEnded) return;

        ghostKills++;
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

    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true); // ✅ Game Over anzeigen

        Debug.Log("🔚 Spielzeit vorbei – Game Over!");

        // Nach 5 Sekunden zur Startszene zurück
        StartCoroutine(WaitAndLoadStartScene());
    }

    private System.Collections.IEnumerator WaitAndLoadStartScene()
    {
        yield return new WaitForSecondsRealtime(5f); // ⏱ Echtzeit trotz Time.timeScale = 0
        Time.timeScale = 1f;
        SceneManager.LoadScene("1 Start Scene"); // ✅ Stelle sicher, dass dieser Name exakt im Build Settings steht
    }
}