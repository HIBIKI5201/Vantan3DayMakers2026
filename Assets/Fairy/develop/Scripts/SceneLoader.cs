using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private SceneName _sceneToLoad;

    public void LoadScene()
    {
        SceneController.LoadScene(_sceneToLoad);
    }
}
