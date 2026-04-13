using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // NEW: Needed for timers (Coroutines)!

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject losePanel;    // NEW: The Game Over screen
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Score Settings")]
    private int currentScore = 0;
    private int highScore = 0;

    private bool isPaused = false;
    private bool isDead = false; // NEW: Prevents pausing while dead

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();

        ResumeGame();
    }

    void Update()
    {
        // Prevent pausing if the player is already dead
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // --- SCORE METHODS ---
    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (highScoreText != null) highScoreText.text = "Best: " + highScore;
    }

    // --- GAME STATE METHODS ---
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- HEALTH METHODS ---
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Don't take damage if already dead

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        isDead = true;

        // 1. Show the Game Over panel
        losePanel.SetActive(true);

        // 2. Freeze the game so enemies stop moving
        Time.timeScale = 0f;

        // 3. Start the timer to return to the menu
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    // NEW: The Timer Method
    private IEnumerator ReturnToMenuAfterDelay()
    {
        // Because Time.timeScale is 0 (frozen), standard WaitForSeconds won't work.
        // We MUST use WaitForSecondsRealtime to count real-world seconds!
        yield return new WaitForSecondsRealtime(1f);

        // Load the menu after 1 real-time second has passed
        LoadMainMenu();
    }
}