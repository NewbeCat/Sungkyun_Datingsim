using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpriteEntry
{
    public string key;
    public Sprite value;
}

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private List<SpriteEntry> spriteEntries;
    private Dictionary<string, Sprite> _facePool;

    private void Awake()
    {
        _facePool = new Dictionary<string, Sprite>();

        foreach (var entry in spriteEntries)
        {
            _facePool[entry.key] = entry.value;
        }
    }
    public Sprite profileCall(string name, int faceNum)
    {
        string spriteName = name + faceNum.ToString();

        if (_facePool.ContainsKey(spriteName))
        {
            return _facePool[spriteName];
        }
        else
        {
            Debug.Log("해당 프로필은 없습니다 : " + name + ", " + faceNum);
            return null;
        }
    }
}
