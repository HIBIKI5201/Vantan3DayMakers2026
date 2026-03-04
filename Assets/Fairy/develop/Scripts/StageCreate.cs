using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class StageCreate : MonoBehaviour
{
    [System.Serializable] 
    public class FrameData
    {
        public Post PostType;
        public float BowAmount;
        public RectTransform MainFrame;
    }
    public FrameData[] _frameDatas;
    public GameObject _stampPrefab;
    public Sprite[] _otherStampImage;
    public PostImage _postImage;
    public RectTransform _aimingImage;
    public RectTransform SstampFrame {  get; private set; }
    public void Create(float rotation, Post post)
    {
        Debug.Log(post.ToString());
        foreach(var frameData in _frameDatas)
        {
            if(frameData.PostType != post)
            {
                Quaternion quaternion = Quaternion.Euler(0, 0, frameData.BowAmount);
                GameObject newStamo = Instantiate(_stampPrefab, this.transform);
                newStamo.transform.position = frameData.MainFrame.position;
                newStamo.transform.rotation = quaternion;
                if(newStamo.TryGetComponent(out StampData stampData))
                {
                    stampData.SetImage(_otherStampImage[Random.Range(0,_otherStampImage.Length)]);
                }
            }
            else
            {
                SstampFrame = frameData.MainFrame;
                _aimingImage.anchoredPosition = frameData.MainFrame.anchoredPosition;
            }
        }
        _postImage.ChangePost(post);
    }
    
}
