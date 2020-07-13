using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitAbilityIconManager : MonoBehaviour
{
    public Sprite[] _spriteList;

    static List<string> _abilNames = new List<string>();

    static Dictionary<string, Sprite> _stateSprites = new Dictionary<string, Sprite>();

    private void Awake()
    {
        StartCoroutine(InitAbilIcon());
    }

    WaitForSeconds _waitTime = new WaitForSeconds(1.0f);
    IEnumerator InitAbilIcon()
    {
        while (!_isAddNameEnd)
            yield return _waitTime;

        int count = _spriteList.Length > _abilNames.Count ? _abilNames.Count : _spriteList.Length;

        for (int i = 0; i < count; ++i)
        {
            if(_stateSprites.ContainsKey(_abilNames[i])) { continue; }
            if(_stateSprites.ContainsValue(_spriteList[i])) { continue; }

            _stateSprites.Add(_abilNames[i], _spriteList[i]);
        }
    }

    public static bool _isAddNameEnd = false;
    public static void AddName(string abilName)
    {
        if (_isAddNameEnd) { return; }
        if (_abilNames == null) { return; }
        if (_abilNames.Contains(abilName)) { return; }

        _abilNames.Add(abilName);
    }

    public static void Update(Image image, string abilName)
    {
        if (_abilNames == null || _stateSprites == null) { return; } 

        if(_stateSprites.ContainsKey(abilName))
        {
            image.sprite = _stateSprites[abilName];
        }
    }
}
