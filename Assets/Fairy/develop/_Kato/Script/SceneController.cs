using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene‚Ìenum
/// </summary>
public enum SceneName : int
{
    GameTitle,
    InGame,
    Result
}

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; set; }

     public SceneName CurrentScene {  get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SceneØ‚è‘Ö‚¦ˆ—
    /// </summary>
    /// <param name="scene">Scene‚ÌØ‚è‘Ö‚¦æ</param>
    public void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene($"{scene}");
    }
}
