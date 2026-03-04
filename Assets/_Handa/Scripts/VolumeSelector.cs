using UnityEngine;
using UnityEngine.UI;

public class VolumeSelector : MonoBehaviour
{
    [Header("Master Settings")]
    [SerializeField] private Button[] masterButtons;
    [SerializeField] private Button muteMasterButton;

    [Header("BGM Settings")]
    [SerializeField] private Button[] bgmButtons;
    [SerializeField] private Button muteBgmButton; // 追加

    [Header("SE Settings")]
    [SerializeField] private Button[] seButtons;
    [SerializeField] private Button muteSeButton; // 追加
    //
    // [Header("Colors")]
    // [SerializeField] private Color activeColor = Color.white;
    // [SerializeField] private Color inactiveColor = Color.clear;

    [Header("Test SE")]
    [SerializeField] private AudioClip testSE; // 確認用に鳴らす音をインスペクターで入れる
    [SerializeField] private TitleUIManager titleUIManager;

    private const string MASTER_KEY = "MASTER_VOL";
    private const string BGM_KEY = "BGM_VOL";
    private const string SE_KEY = "SE_VOL";

    void Start()
    {
        // 保存された音量をロード（初期値は1.0 = 100%） 
        float m = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float b = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float s = PlayerPrefs.GetFloat(SE_KEY, 1f);

        // 初期音量を適用
        Debug.Log($"m:{m}, b:{b}, s:{s}");
        ApplyMasterVolume(m, false);
        ApplyVolume(true, b, false);
        ApplyVolume(false, s, false);
        titleUIManager.UpdateAudioUI(muteMasterButton.transform.parent.gameObject, m == 0.0001f);
        titleUIManager.UpdateAudioUI(muteBgmButton.transform.parent.gameObject, b == 0.0001f);
        titleUIManager.UpdateAudioUI(muteSeButton.transform.parent.gameObject, s == 0.0001f);

        // ボタンクリック時は true にして音が鳴るようにする
        for (int i = 0; i < masterButtons.Length; i++)
        {
            float vol = (i + 1) * 0.2f;
            masterButtons[i].onClick.AddListener(() => ApplyMasterVolume(vol, true));
        }
        muteMasterButton.onClick.AddListener(() => ApplyMasterVolume(0.0001f, true));
        for (int i = 0; i < bgmButtons.Length; i++)
        {
            float vol = (i + 1) * 0.2f;
            bgmButtons[i].onClick.AddListener(() => ApplyVolume(true, vol, true));
        }
        muteBgmButton.onClick.AddListener(() => ApplyVolume(true, 0.0001f, true));

        for (int i = 0; i < seButtons.Length; i++)
        {
            float vol = (i + 1) * 0.2f;
            seButtons[i].onClick.AddListener(() => ApplyVolume(false, vol, true));
        }
        muteSeButton.onClick.AddListener(() => ApplyVolume(false, 0.0001f, true));
    }
    void ApplyVolume(bool isBgm, float vol, bool playTestSound)
    {
        if (isBgm)
        {
            AudioManager.SetBGMVolume(vol);
            PlayerPrefs.SetFloat(BGM_KEY, vol);
            UpdateButtonVisuals(bgmButtons, muteBgmButton, vol);
        }
        else
        {
            AudioManager.SetSEVolume(vol);
            PlayerPrefs.SetFloat(SE_KEY, vol);
            UpdateButtonVisuals(seButtons, muteSeButton, vol);

            // playTestSound が true の時だけ音を鳴らす
            if (playTestSound && !isBgm && vol > 0.01f && testSE != null)
            {
                // 他の汎用SE（スタンプ音など）を止めずに、確認音だけを鳴らし直す
                AudioManager.PlaySystemSE(testSE);
            }
        }
    }
    void ApplyMasterVolume(float vol, bool playTestSound)
    {
        AudioManager.SetMasterVolume(vol);
        PlayerPrefs.SetFloat(MASTER_KEY, vol);
        UpdateButtonVisuals(masterButtons, muteMasterButton, vol);

        // Master変更時もSEで確認
        if (playTestSound && vol > 0.01f && testSE != null)
        {
            AudioManager.PlaySystemSE(testSE);
        }
    }
    void UpdateButtonVisuals(Button[] buttons, Button muteButton, float currentVol)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            float buttonVol = (i + 1) * 0.2f;
            // ボタンの画像色を変更（ボタン自体にImageがついている想定）→ 親オブジェクトに切り替え
            titleUIManager.UpdateAudioUI(buttons[i].transform.parent.gameObject, buttonVol <= currentVol + 0.01f);
        }
        // muteButtonのUI制御
        titleUIManager.UpdateAudioUI(muteButton.transform.parent.gameObject, currentVol == 0.0001f);
    }
}