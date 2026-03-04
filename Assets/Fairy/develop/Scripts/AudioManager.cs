using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Setting")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Pool Setting")]
    [SerializeField] private int poolSize = 8; // 同時に鳴らせる汎用SEの数
    private List<AudioSource> sePool = new List<AudioSource>();
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioMixerGroup seGroup = audioMixer.FindMatchingGroups("SE")[0];

        // 汎用SE用のプールを生成
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SE")[0];
            sePool.Add(newSource);
        }
    }
    // 他のクラスから AudioManager.PlaySE(クリップ) で呼べるようにする
    public static void PlaySE(AudioClip clip)
    {
        if (Instance == null || clip == null) return;

        // 空いている（鳴っていない）Sourceを探す
        foreach (var source in Instance.sePool)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                return;
            }
        } // 全て埋まっていたら、一番古いものを上書きするか、諦める（今回は諦める）
    }
    public static void PlaySystemSE(AudioClip clip)
    {
        if (Instance == null || clip == null) return;

        // これにより、連打しても音が重ならず、常に最初から再生されます
        Instance.seSource.Stop();

        Instance.seSource.clip = clip;
        Instance.seSource.Play();
    }
    // 【追加】今鳴っているSEをすべて止める
    public static void StopSE(AudioClip clip)
    {
        if (Instance == null || clip == null) return;
        Instance.seSource.Stop(); // 前の音を止める
        Instance.seSource.clip = clip;
        Instance.seSource.Play();
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
    public static void SetMasterVolume(float value)
    {
        if (Instance == null || Instance.audioMixer == null) return;
        // 0.0001f ～ 1f の範囲にクランプしてデシベル変換
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        Instance.audioMixer.SetFloat("MasterVol", dB); // Mixer側の名前と合わせる
    }
}