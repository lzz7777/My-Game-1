using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPTextureManage : MonoBehaviour
{
    private static GameObject _pMainObject;
    private static PPTextureManage _pContainer = null;
    public static PPTextureManage GetInstance()
    {
        if (_pContainer == null)
        {
            _pContainer = _pMainObject.GetComponent<PPTextureManage>();
        }
        return _pContainer;
    }
    private Dictionary<string, Object[]> m_pAtlasDic; //图集的集合

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        PPTextureManage._pMainObject = gameObject;
        m_pAtlasDic = new Dictionary<string, Object[]>();
    }
    
    public Sprite LoadAtlasSprite(string _spriteAtlasPath, string _spriteName)
    {
        //Debug.Log(_spriteAtlasPath);
        //Debug.Log(_spriteName);
        Sprite _sprite = FindSpriteFormBuffer(_spriteAtlasPath, _spriteName);
        if (_sprite == null)
        {
            Object[] _atlas = Resources.LoadAll(_spriteAtlasPath);
            m_pAtlasDic.Add(_spriteAtlasPath, _atlas);
            _sprite = SpriteFormAtlas(_atlas, _spriteName);
        }
        return _sprite;
    }

    public void DeleteAtlas(string _spriteAtlasPath)
    {
        if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
        {
            m_pAtlasDic.Remove(_spriteAtlasPath);
        }
    }

    public Sprite FindSpriteFormBuffer(string _spriteAtlasPath, string _spriteName)
    {
        if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
        {
            Object[] _atlas = m_pAtlasDic[_spriteAtlasPath];
            Sprite _sprite = SpriteFormAtlas(_atlas, _spriteName);
            return _sprite;
        }
        return null;
    }

    private Sprite SpriteFormAtlas(Object[] _atlas, string _spriteName)
    {
        for (int i = 0; i < _atlas.Length; i++)
        {
            if (_atlas[i].GetType() == typeof(Sprite))
            {
                if(_atlas[i].name == _spriteName)
                {
                    return (Sprite)_atlas[i];
                }
            }
        }

        Debug.LogError("图集中没有这张图片");
        return null;
    }

}
