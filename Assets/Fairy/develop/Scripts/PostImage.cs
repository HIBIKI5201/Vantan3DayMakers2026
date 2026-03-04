using UnityEngine;
using UnityEngine.UI;

public enum ImageType
{
    Hand,
    Character,
    Bow,
    Stamp
}
public class PostImage : MonoBehaviour
{
    public PostDatabase _postDatabase;
    public Image _targetImage;
    public ImageType _imageType;

    public void ChangePost(Post post)
    {
        PostData data = _postDatabase.Get(post);
        switch (_imageType)
        {
            case ImageType.Hand:
                _targetImage.sprite = data.HandImage;
                break;
            case ImageType.Character:
                _targetImage.sprite = data.CharacterImage;
                break;
            case ImageType.Bow:
                _targetImage.sprite = data.BowImage;
                break;
            case ImageType.Stamp:
                _targetImage.sprite = data.StampImage;
                break;
        }
    }
}
