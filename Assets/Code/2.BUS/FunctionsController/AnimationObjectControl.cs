using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjectControl : MonoBehaviour
{
    [Header("Tên animation")]
    public string AnimationName;
    public bool HideOnCreate;
    private Animator Anim;
    float AnimTime;
    private void Awake()
    {
        if (HideOnCreate)
            gameObject.SetActive(false);
        Anim = this.GetComponent<Animator>();
        AnimationClip[] clips = Anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(AnimationName))
            {
                AnimTime = clip.length;
                break;
            }

        }
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
            Anim.Play(AnimationName, 0, Random.Range(0, AnimTime));
            //Anim.SetTrigger(AnimationName);
        }
    }
}
