using UnityEngine;

public class CorsorChange : MonoBehaviour
{
    private void Awake()
    {
        Cursor.SetCursor(CreateBlackCursor(), Vector2.zero, CursorMode.Auto); //カーソル画像を変更
    }

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

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
