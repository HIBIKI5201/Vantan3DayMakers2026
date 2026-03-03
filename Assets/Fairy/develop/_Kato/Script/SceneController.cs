using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene‚Ìenum
/// </summary>
public enum SceneName : byte
{
    GameTitle,
    InGame,
    Result
}

public static class SceneController
{
    public static SceneName CurrentScene;

    /// <summary>
    /// SceneØ‚è‘Ö‚¦ˆ—
    /// </summary>
    /// <param name="scene">Scene‚ÌØ‚è‘Ö‚¦æ</param>
    public static void LoadScene(SceneName scene)
    {
        SceneManager.LoadScene($"{scene}");
    }
}
