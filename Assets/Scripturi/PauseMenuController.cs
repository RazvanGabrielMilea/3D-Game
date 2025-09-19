using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup pauseCanvasGroup;
    public Button      backButton;
    public Button      saveButton;
    public Button      mainMenuButton;

    [Header("Extras to Toggle")]
    [Tooltip("Root GameObject of your health-bar UI")]
    public GameObject healthBarUI;
    [Tooltip("Script that handles camera/player look or movement")]
    public MonoBehaviour cameraController;

    [Header("Save Settings")]
    [Tooltip("Drag your Player GameObject here (must have Transform)")]
    public Transform playerTransform;

    private bool isPaused = false;

    void Start()
    {
        // Hook up button callbacks
        backButton.onClick.AddListener(ResumeGame);
        saveButton.onClick.AddListener(OnSaveGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Ensure the menu starts hidden
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else          PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 0f, 1f, 0.2f, true));

        // Hide health bar and disable camera input
        if (healthBarUI      != null) healthBarUI.SetActive(false);
        if (cameraController != null) cameraController.enabled = false;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 1f, 0f, 0.2f, false));

        // Show health bar and re-enable camera input
        if (healthBarUI      != null) healthBarUI.SetActive(true);
        if (cameraController != null) cameraController.enabled = true;
    }

    private void OnSaveGame()
    {
        if (SaveLoadManager.Instance != null && playerTransform != null)
        {
            SaveLoadManager.Instance.SaveGame(playerTransform);
            Debug.Log("Game saved.");
        }
        else
        {
            Debug.LogWarning("SaveLoadManager or Player Transform not assigned!");
        }
        // keep the game paused so player can resume or go to main menu
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Fades a CanvasGroup in or out.
    /// </summary>
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration, bool enableOnEnd)
    {
        float elapsed = 0f;
        cg.interactable   = enableOnEnd;
        cg.blocksRaycasts = enableOnEnd;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
        cg.interactable   = enableOnEnd;
        cg.blocksRaycasts = enableOnEnd;
    }
}
