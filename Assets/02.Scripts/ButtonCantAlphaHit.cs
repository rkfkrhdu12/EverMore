using UnityEngine;
using UnityEngine.UI;

public class ButtonCantAlphaHit
{
    /// <summary>
    /// Scene 넘어갈때 작동되는 함수 (ScenesManager)
    /// </summary>
    public void Start()
    {
        _image = null;
        _image = GameObject.Find("Canvas").GetComponentsInChildren<Image>(); ;

        for (int i = 0; i < _image.Length; ++i)
        {
            if(_image[i].CompareTag("UI")) { continue; }

            _image[i].alphaHitTestMinimumThreshold = .1f;
        }
    }

    #region Variable

    Image[] _image;

    #endregion
}
