using UnityEngine.SceneManagement;

/// <summary>
/// Sceneのenum
/// </summary>
public enum SceneName : byte
{
    GameTitle,
    InGame,
    Result
}

public static class SceneController
{
    public static SceneName CurrentScene { get; private set; }

    /// <summary>
    /// Scene切り替え処理
    /// </summary>
    /// <param name="scene">Sceneの切り替え先</param>
    public static void LoadScene(SceneName scene)
    {
        CurrentScene = scene;
        SceneManager.LoadScene($"{scene}");
    }
}
