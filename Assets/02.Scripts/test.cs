using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AnimatorPro;

public class test : MonoBehaviour
{
    public Animator _animator;
    public AnimatorPro _aniPro;

    private static readonly int idAttack = Animator.StringToHash("Attack");
    private static readonly int idMove = Animator.StringToHash("Move");

    private void Awake()
    {
        _aniPro?.Init(_animator);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W)) { _aniPro.SetParam(idMove, 1.0f); }
        if(Input.GetKey(KeyCode.S)) { _aniPro.SetParam(idMove, -1.0f); }
        if(Input.GetKey(KeyCode.A)) { _aniPro.SetParam(idMove, 0.0f); }

        if (Input.GetMouseButtonDown(0))
        {
            _aniPro.SetParam(idAttack, true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _aniPro.SetParam(idAttack, false);
        }
    }
}
