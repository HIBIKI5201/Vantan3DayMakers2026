using UnityEngine;
using UnityEngine.UI;

public class VolumeSelector : MonoBehaviour
{
    [Header("BGM Settings")]
    [SerializeField] private Button[] bgmButtons;
    [SerializeField] private Button muteBgmButton; // 追加

    [Header("SE Settings")]
    [SerializeField] private Button[] seButtons;
    [SerializeField] private Button muteSeButton; // 追加

    [Header("Colors")]
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color inactiveColor = Color.gray;

    [Header("Test SE")]
    [SerializeField] private AudioClip testSE; // 確認用に鳴らす音をインスペクターで入れる

    private const string BGM_KEY = "BGM_VOL";
    private const string SE_KEY = "SE_VOL";

    void Start()
    {
        // 保存された音量をロード（初期値は1.0 = 100%）
        float b = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float s = PlayerPrefs.GetFloat(SE_KEY, 1f);

        // 初期音量を適用
        ApplyVolume(true, b, false);
        ApplyVolume(false, s, false);

        // ボタンクリック時は true にして音が鳴るようにする
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
            UpdateButtonVisuals(bgmButtons, vol);
        }
        else
        {
            AudioManager.SetSEVolume(vol);
            PlayerPrefs.SetFloat(SE_KEY, vol);
            UpdateButtonVisuals(seButtons, vol);

            // playTestSound が true の時だけ音を鳴らす
            if (playTestSound && !isBgm && vol > 0.01f && testSE != null)
            {
                // 他の汎用SE（スタンプ音など）を止めずに、確認音だけを鳴らし直す
                AudioManager.PlaySystemSE(testSE);
            }
        }
    }
    void UpdateButtonVisuals(Button[] buttons, float currentVol)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            float buttonVol = (i + 1) * 0.2f;
            // ボタンの画像色を変更（ボタン自体にImageがついている想定）
            buttons[i].GetComponent<Image>().color = (buttonVol <= currentVol + 0.01f) ? activeColor : inactiveColor;
        }
    }
}