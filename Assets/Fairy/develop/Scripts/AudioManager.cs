using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum SEClipType
{
    Stamp,
    Paper
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Setting")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Pool Setting")]
    [SerializeField] private int poolSize = 8; // 同時に鳴らせる汎用SEの数

    [Header("BGM Clips")]
    [SerializeField] private AudioClip titleClip;
    [SerializeField] private AudioClip inGameClip;
    [SerializeField] private AudioClip resultClip;
    [Header("SE Clips")]
    [SerializeField] private AudioClip StampClip;
    [SerializeField] private AudioClip PaperClip;

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
        
        bgmSource.loop = true;
    }
    private void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip) return; // 同じ曲なら再生し直さない

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
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
        } 
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

    public void ChangeBGM(SceneName type)
    {
        switch (type)
        {
            case SceneName.GameTitle:
                PlayBGM(titleClip);
                break;

            case SceneName.InGame:
                PlayBGM(inGameClip);
                break;

            case SceneName.Result:
                PlayBGM(resultClip);
                break;
        }
    }

    public void ChangeSE(SEClipType type)
    {
        switch (type)
        {
            case SEClipType.Stamp:
                PlaySE(StampClip);
                break;
            case SEClipType.Paper:
                PlaySE(PaperClip);
                break;
        }
    }
}