using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Setting")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioMixer audioMixer;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // 他のクラスから AudioManager.PlaySE(クリップ) で呼べるようにする
    public static void PlaySE(AudioClip clip)
    {
        if (Instance == null || clip == null) return;
        Instance.seSource.PlayOneShot(clip);
    }
    // 【追加】今鳴っているSEをすべて止める
    public static void StopSE()
    {
        if (Instance == null) return;
        Instance.seSource.Stop();
    }
    public static void SetBGMVolume(float value)
    {
        if (Instance == null || Instance.audioMixer == null) return;
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        Instance.audioMixer.SetFloat("BGMVol", dB);
    }
    public static void SetSEVolume(float value)
    {
        if (Instance == null || Instance.audioMixer == null) return;
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        Instance.audioMixer.SetFloat("SEVol", dB);
    }
}