using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    GameTitle,
    InGame,
    Result
}
public class SceneController : MonoBehaviour
{
    [SerializeField] private SceneName sceneType;
    [SerializeField] private FadeType fadeType;

    private static SceneController instance;
    private Fade fade;
    private bool isLoading = false;

    private void Awake()
    {
        fade = GetComponentInChildren<Fade>();
        instance = this;
    }

    private void Start()
    {
        // シーンに入った瞬間BGM変更
        //AudioManager.Instance.ChangeBGM(sceneType);
    }

    public　static void LoadScene(SceneName sceneName)
    {
        if (instance == null) return;

        if (instance.isLoading)
        {
            Debug.Log("Already Loading");
            return;
        }

        instance.LoadSceneInternal(sceneName).Forget();
    }

    private async UniTask LoadSceneInternal(SceneName sceneName)
    {
        isLoading = true;

        // まず暗転させる
        if (fade != null)
            await fade.FadeOutAsync(fadeType);

        // シーンを非同期で読み込む
        await SceneManager.LoadSceneAsync(sceneName.ToString());

        // シーン切り替え後にBGMを変える
        if (AudioManager.Instance != null)
            AudioManager.Instance.ChangeBGM(sceneName);

        // 明転させる (FadeInAsyncを呼ぶのが正解)
        if (fade != null)
            await fade.FadeInAsync();

        isLoading = false;
    }
    public void FadeAndLoadSceneAsyncButton(SceneName sceneName)
    {
        LoadSceneInternal(sceneName).Forget();
    }
}