using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjectControl : MonoBehaviour
{
    [Header("Tên animation")]
    public string AnimationName;
    [Header("Ẩn khi khởi tạo")]
    public bool HideOnCreate;
    [Header("Chạy animation khi khởi tạo")]
    public bool PlayAnimOnCreate = true;
    [Header("Tốc độ ngẫu nhiên")]
    public bool RandomSpeed = true;
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
        //if (!PlayAnimOnCreate)
        //    Anim.enabled = false;
    }

    private void OnEnable()
    {
        if (AnimationName != "" && AnimationName != null)
        {
            if (RandomSpeed)
                Anim.speed = Random.Range(0.3f, 1f);
            Anim.Play(AnimationName, 0, RandomSpeed ? Random.Range(0, AnimTime) : AnimTime);
        }
    }

    public void PlayAnim()
    {
        Anim.enabled = true;
        Anim.ResetTrigger(AnimationName);
    }

    public void ResetAnim()
    {
        Anim.Rebind();
    }
}
