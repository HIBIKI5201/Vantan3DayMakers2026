using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public enum SceneName
{
    GameTitle,
    InGame,
    Result
}
/// <summary>
/// シングルトン実装
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);}
        else{
            Destroy(gameObject);}
    }
    /// <summary> 外部から呼び出す関数 </summary>
    /// <param name="sceneType"></param>
    public void LoadScene(int sceneType)
    {
        string sceneName = ((SceneName)sceneType).ToString();
        LoadSceneAsync(sceneName).Forget();
        // ロード後にBGM切り替え
        // ※Fadeはコンポーネント指向で書く
    }
    async UniTask LoadSceneAsync(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName);
        AudioManager.Instance.ChangeBGM((SceneName)System.Enum.Parse(typeof(SceneName), sceneName));
    }
}
