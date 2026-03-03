using UnityEngine;

public class StageCreate : MonoBehaviour
{
    [System.Serializable] 
    public class FrameData
    {
        public Post PostType;
        public Transform MainFrame;
    }
    public FrameData[] _frameDatas;
    public GameObject _stampPrefab;
    public void Create(float rotation, Post post)
    {
        foreach(var frameData in _frameDatas)
        {
            if(frameData.PostType != post)
            {
                Quaternion quaternion = Quaternion.Euler(0, 0, rotation);
                Instantiate(_stampPrefab,frameData.MainFrame.position, quaternion);
            }
        }
    }
}
