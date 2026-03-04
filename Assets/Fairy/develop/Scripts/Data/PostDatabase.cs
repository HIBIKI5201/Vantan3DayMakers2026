using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PostDatabase")]
public class PostDatabase : ScriptableObject
{
  public PostData[] PostDatas;
    private Dictionary<Post, PostData> _cache;

    public void Initialize()
    {
        _cache = new Dictionary<Post, PostData>();
        foreach (var p in PostDatas)
            _cache[p.PostType] = p;
    }

    public PostData Get(Post post)
    {
        if(_cache == null)
        {
            Initialize();
        }

        if (_cache.TryGetValue(post, out var data))
        {
            return data;
        }
        Debug.LogError("PostData Null");
        return null;
    }
}
