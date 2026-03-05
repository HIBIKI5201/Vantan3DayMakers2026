using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    [SerializeField] private SceneName sceneType;

    private static SceneController instance;
    private Fade fade;
    private bool isLoading = false;

    private void Awake()
    {
        fade = GetComponentInChildren<Fade>();
        instance = this;
    }

    /// <summary>これは必要</summary>
    private void Start()
    {
        // シーンに入った瞬間BGM変更
        Debug.Log($"<color=cyan>[SceneCheck] 現在のシーン: {SceneManager.GetActiveScene().name} / 要求BGM: {sceneType}</color>");
        AudioManager.Instance.ChangeBGM(sceneType);
    }

    /// <summary> SceneLoaderから呼ばれる </summary>
    /// <param name="sceneName"></param>
    public　static void LoadScene(SceneName sceneName)
    {
        if (instance == null) return;

        if (instance.isLoading)
        {
            Debug.Log("Already Loading");
            return;
        }

        instance.LoadSceneInternal(sceneName).Forget();//
    }

    private async UniTask LoadSceneInternal(SceneName sceneName)
    {
        // isLoading = true;
    
        // // まず暗転させる
        // if (fade != null)
        //     await fade.FadeOutAsync(fadeType);
    
        // シーンを非同期で読み込む
        await SceneManager.LoadSceneAsync(sceneName.ToString());
    
         // シーン切り替え後にBGMを変える
        if (AudioManager.Instance != null)
            AudioManager.Instance.ChangeBGM(sceneName);
    
        // 明転させる (FadeInAsyncを呼ぶのが正解)
        // if (fade != null)
        //     await fade.FadeInAsync();
    
        // isLoading = false;
    }
    public void FadeAndLoadSceneAsyncButton(SceneName sceneName)
    {
        LoadSceneInternal(sceneName).Forget();
    }
}