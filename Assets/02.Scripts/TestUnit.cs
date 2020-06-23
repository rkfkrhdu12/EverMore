using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    public int AniType = 5;
    public int prevAniType;

    Animator ani;

    public ParticleSystem particle;
   
    private void Awake()
    {
        ani = GetComponent<Animator>();

        prevAniType = AniType;

        ani.SetInteger("WeaponType", AniType);

        float atkSpd = ani.GetFloat("AttackSpeed");

        particle.playbackSpeed = 1 * atkSpd;
        for (int i = 0; i < particle.transform.childCount; ++i)
        {
            particle.transform.GetChild(i).GetComponent<ParticleSystem>().playbackSpeed = 1 * atkSpd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnAttack();
        }

        if(AniType != prevAniType)
        {
            prevAniType = AniType;
            ani.SetInteger("WeaponType", AniType);
        }
    }

    public void OnAttack()
    {
        ani.SetBool("Attack", true);

    }

    public void OnEffect()
    {
        Debug.Log("A");

        particle.Play();
    }
}
