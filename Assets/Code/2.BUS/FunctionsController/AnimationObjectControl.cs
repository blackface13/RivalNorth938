using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjectControl : MonoBehaviour {
    [Header("Tên animation")]
    public string AnimationName;
    private  Animator Anim;
    private void Awake()
    {
            Anim = this.GetComponent<Animator>();
        if (AnimationName != "" && AnimationName != null)
        {
            Anim.SetTrigger(AnimationName);
        }
    }
    private void OnEnable()
    {
        if (AnimationName != "" && AnimationName != null)
        {
            Anim.speed = Random.Range(0.3f, 1f);
            Anim.SetTrigger(AnimationName);
        }
    }
}
