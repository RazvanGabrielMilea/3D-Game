using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    private bool shouldLoadOnNextScene = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (shouldLoadOnNextScene && scene.name == "Map")  
        {
            RestorePlayerPosition();
            shouldLoadOnNextScene = false;
        }
    }

    public void SaveGame(Transform playerTransform)
    {
        PlayerPrefs.SetFloat("Save_PosX", playerTransform.position.x);
        PlayerPrefs.SetFloat("Save_PosY", playerTransform.position.y);
        PlayerPrefs.SetFloat("Save_PosZ", playerTransform.position.z);
        PlayerPrefs.SetFloat("Save_RotY", playerTransform.eulerAngles.y);
        PlayerPrefs.Save();
        Debug.Log("Game saved (position only).");
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey("Save_PosX");
    }

    public void PrepareLoad()
    {
        shouldLoadOnNextScene = true;
    }

    void RestorePlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No Player tagged object found to restore position.");
            return;
        }

        float x = PlayerPrefs.GetFloat("Save_PosX");
        float y = PlayerPrefs.GetFloat("Save_PosY");
        float z = PlayerPrefs.GetFloat("Save_PosZ");
        float r = PlayerPrefs.GetFloat("Save_RotY");

        player.transform.position = new Vector3(x, y, z);
        player.transform.rotation = Quaternion.Euler(0, r, 0);

        Debug.Log($"Restored position to ({x:F1},{y:F1},{z:F1}).");
    }
}
