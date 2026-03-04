using UnityEngine;
public class TitleCursorController : TitleUIObjects
{
    [SerializeField] protected Texture2D centerCursorImage;
    [SerializeField] protected Texture2D editNameCursorImage;
    [SerializeField] protected Texture2D gameStartCursorImage;
    [SerializeField] protected Texture2D creditCursorImage;
    [SerializeField] protected Texture2D audioCursorImage;
    private void Awake()
    {
        Cursor.SetCursor(centerCursorImage, Vector2.zero, CursorMode.Auto); //カーソル画像を変更
    }
}