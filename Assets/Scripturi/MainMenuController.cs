using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject loadingPanel;

    [Header("Loading UI")]
    public Slider loadingBar;
    public Text   progressText;

    [Header("Loading Settings")]
    public float minLoadingTime = 1.5f;

    // New Game button
    public void OnNewGame()
    {
        // No save to load
        StartCoroutine(LoadSceneWithLoading("Map", false));
    }

    // Load Game button
    public void OnLoadGame()
    {
        if (!SaveLoadManager.Instance.HasSave())
        {
            Debug.LogWarning("No saved game to load.");
            return;
        }

        // Tell the SaveLoadManager to restore after Map loads
        SaveLoadManager.Instance.PrepareLoad();
        StartCoroutine(LoadSceneWithLoading("Map", true));
    }

    public void OnOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnBackFromOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnExit()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    /// <summary>
    /// Loads a scene asynchronously, shows the loading UI, and optionally triggers the save-load callback.
    /// </summary>
    private IEnumerator LoadSceneWithLoading(string sceneName, bool isLoadGame)
    {
        // Hide menus, show loading UI
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        loadingPanel.SetActive(true);

        // Start loading
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float startTime = Time.time;

        // Step 1: wait until progress 0.9
        while (op.progress < 0.9f)
        {
            float prog = op.progress / 0.9f;
            loadingBar.value    = prog;
            progressText.text   = $"{(int)(prog * 100)}%";
            yield return null;
        }
        Debug.Log("Scene data loaded; waiting on min time...");

        // Step 2: enforce minimum loading screen duration
        while (Time.time - startTime < minLoadingTime)
        {
            float t = (Time.time - startTime) / minLoadingTime;
            loadingBar.value    = t;
            progressText.text   = $"{(int)(t * 100)}%";
            yield return null;
        }

        // Step 3: finish up and activate
        loadingBar.value  = 1f;
        progressText.text = "100%";
        Debug.Log("Activating scene now");
        op.allowSceneActivation = true;
        yield return null;  // wait one frame for scene swap

        // (Optionally) you could log here if you like:
        Debug.Log($"Scene '{sceneName}' activated. isLoadGame={isLoadGame}");
        // The actual restoring of position happens in SaveLoadManager.OnSceneLoaded
    }
}
