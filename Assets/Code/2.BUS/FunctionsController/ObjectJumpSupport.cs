using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object hỗ trợ player nhảy lên cao trong map
/// </summary>
public class ObjectJumpSupport : MonoBehaviour
{
    [Title("Tên animation sẽ thực hiện khi va chạm với Player")]
    public string AnimName;
    [Title("Lực đẩy nhân vật lên")]
    public Vector2 JumpForce;
    private Animator Anim;
    private void OnEnable()
    {
        Anim = GetComponent<Animator>();
        Anim.SetTrigger(AnimName);
        Anim.enabled = false;
    }

    /// <summary>
    /// Va chạm với player
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Hero))
        {
            Anim.enabled = true;
            Anim.SetTrigger(AnimName);
            var hero = col.GetComponent<HeroController>();
            if(hero.IsAtking)
            {
                hero.IsAtking = false;
                hero.IsAllowAtk = true;
            }
            hero.SetAnimation(HeroController.Actions.Jump);
            hero.IsJumping = true;
            hero.IsMoving = false;
            hero.IsAllowAtk = false;
            hero.IsAutoJumping = true;
            hero.HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            hero.HeroRigidBody2D.velocity = Vector3.zero;
            hero.HeroRigidBody2D.velocity += JumpForce;
            //hero.HeroRigidBody2D.AddForce(JumpForce, ForceMode2D.Impulse);
        }
    }
}
