using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum CursorType
{
    OnEditName,
    OnGameStart,
    OnCredit,
    OnAudioSettings,
    None
}
public class TitleCursorController : MonoBehaviour
{
    CursorType _cursorType;
    CursorType _CursorType{get => _cursorType;set{_cursorType = value;UpdateActiveObj();}}
    // [SerializeField] Sprite gameStartPrediction;
    // [SerializeField] Sprite audioCursorPrediction;
    [Header("GameObject")]
    [SerializeField] GameObject editNameDecorationObject;
    [SerializeField] GameObject gameStartDecorationObject;
    [SerializeField] GameObject creditCursorDecorationObject;
    [SerializeField] GameObject audioCursorDecorationObject;
    Dictionary<CursorType, GameObject> decorationObjDict;
    [Header("CurrentParam")]
    [SerializeField] Image _currentDecorationImage; //直接代入中
    
    //パラメータ関連
    [SerializeField] Vector2 editNameDecorationPosDuration;
    [SerializeField] Vector2 gameStartDecorationPosDuration;
    [SerializeField] Vector2 creditCursorDecorationPosDuration;
    [SerializeField] Vector2 audioCursorDecorationPosDuration;
    private void Awake()
    {
        Cursor.SetCursor(CreateBlackCursor(), Vector2.zero, CursorMode.Auto); //カーソル画像を変更
        
        decorationObjDict = new Dictionary<CursorType, GameObject>()
        {
            { CursorType.OnEditName,       editNameDecorationObject      },
            { CursorType.OnGameStart,      gameStartDecorationObject     },
            { CursorType.OnCredit,         creditCursorDecorationObject  },
            { CursorType.OnAudioSettings,  audioCursorDecorationObject   }
        };
    }

    private void Update()
    {
        Vector2 posDuration = Vector2.zero;
        switch (_cursorType)
        {
            case CursorType.OnEditName:
                posDuration = editNameDecorationPosDuration;
                break;
            case CursorType.OnGameStart:
                posDuration = gameStartDecorationPosDuration;
                break;
            case CursorType.OnCredit:
                posDuration = creditCursorDecorationPosDuration;
                break;
            case CursorType.OnAudioSettings:
                posDuration = audioCursorDecorationPosDuration;
                break;
        }
        Move(posDuration);
    }
    private void Move(Vector2 posDuration) => _currentDecorationImage.rectTransform.position = (Vector2)Input.mousePosition + posDuration;
    /// <summary> _CursorTypeの変更時に一度だけ呼び出される </summary>
    private void UpdateActiveObj()
    {
        decorationObjDict.ToList().ForEach(kvp => kvp.Value.SetActive(kvp.Key == _cursorType));
        if (_cursorType != CursorType.None){
            _currentDecorationImage = decorationObjDict[_cursorType].GetComponent<Image>();
        }
    }
    
    /// <summary>PointerEnterから呼び出し</summary><param name="cursorTypeIndex"></param>
    public void SetCursorType(int cursorTypeIndex) => _CursorType = (CursorType)cursorTypeIndex;
    private Texture2D CreateBlackCursor()
    {
        // 黒点生成
        int size = 16;
        Texture2D tex = new Texture2D(size, size);
        // 全部透明にする
        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
            tex.SetPixel(x, y, Color.clear);
        // 中心だけ黒
        tex.SetPixel(size / 2, size / 2, Color.black);
        tex.Apply();
        return tex;
    }
}