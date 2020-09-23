using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [TabGroup("Cấu hình")]
    [Title("Hướng sang trái khi khởi tạo")]
    public bool IsViewLeft;
    [TabGroup("Cấu hình")]
    [Title("Tự động ẩn")]
    public bool IsAutoHide = true;
    [TabGroup("Cấu hình")]
    [Title("Tự động ẩn sau khoảng thời gian")]
    public float DelayTimeHide;
    [TabGroup("Cấu hình")]
    [Title("Gây sát thương ngay khi hiển thị")]
    public bool HitWhenShow;
    [TabGroup("Cấu hình")]
    [Title("Thời gian delay trước khi gây sát thương")]
    public float DelayTimeHitWhenShow;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy đối phương về sau")]
    public float ForceToVictim;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy đối phương cộng thêm (dành cho di chuyển + đánh)")]
    public float ForceToVictimBonus;
    [TabGroup("Cấu hình")]

    [TabGroup("Misc")]
    private Collider2D ThisCollider;

    private float OriginalScaleX;//Scale ban đầu của object

    #region Initialize

    public virtual void Awake()
    {
        OriginalScaleX = this.transform.localScale.x;
        ThisCollider = this.GetComponent<Collider2D>();
    }

    public virtual void OnEnable()
    {
        //Gây sát thương khi hiển thị skill
        if (HitWhenShow)
            ThisCollider.enabled = true;
        else
        {
            StartCoroutine(AutoEnableCollider());
            ThisCollider.enabled = false;
        }

        //Hướng skill
        if (IsViewLeft)
            this.transform.localScale = new Vector3(-OriginalScaleX, this.transform.localScale.y, this.transform.localScale.z);
        else
            this.transform.localScale = new Vector3(OriginalScaleX, this.transform.localScale.y, this.transform.localScale.z);

        //Tự động ẩn sau 1 khoảng time
        if (IsAutoHide)
            StartCoroutine(AutoHide(DelayTimeHide));
    }
    #endregion

    #region Functions

    /// <summary>
    /// Tự động ẩn Object sau 1 khoảng time
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public virtual IEnumerator AutoHide(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Tự động enable collider sau 1 khoảng time
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator AutoEnableCollider()
    {
        yield return new WaitForSeconds(DelayTimeHitWhenShow);
        ThisCollider.enabled = true;
    }

    #endregion

    #region Physics

    /// <summary>
    /// Va chạm không xuyên qua
    /// </summary>
    /// <param name="col"></param>
    //public void OnCollisionEnter2D(Collision2D col)
    //{
    //    //Va chạm với mặt đất
    //    //if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
    //    //{
    //    //    if (CurrentAction.Equals(Actions.Jump) || CurrentAction.Equals(Actions.Surf))
    //    //        SetAnimation(Actions.Idle);
    //    //    IsJumping = false;
    //    //    IsAlowAtk = true;
    //    //}
    //}

    /// <summary>
    /// Va chạm không xuyên qua
    /// </summary>
    /// <param name="col"></param>
    //public void OnCollisionExit2D(Collision2D col)
    //{
    //    //Đổi animation khi rơi từ map xuống
    //    //if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
    //    //{
    //    //    if (!CurrentAction.Equals(Actions.Atk) && !CurrentAction.Equals(Actions.Surf))
    //    //    {
    //    //        if (CurrentAction.Equals(Actions.Jump) || CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Move))
    //    //            SetAnimation(Actions.Jump);
    //    //        IsJumping = true;
    //    //    }
    //    //}
    //}

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillPlayer))
            || (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Hero) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillEnemy)))
        {
            if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy))
            {
                var enemy = col.GetComponent<EnemyController>();
                StartCoroutine(GameSettings.BattleControl.RepelVictim(enemy.ThisRigid2D, this.transform.position, col.gameObject.transform.position, (ForceToVictim + ForceToVictimBonus), enemy.Weight, enemy.IsViewLeft));
                GameSettings.BattleControl.ShowDmgText(col.transform.position, Random.Range(0001, 5000).ToString());

                enemy.SetAnimation(EnemyController.Actions.Hited);
            }
        }
    }
    #endregion
}
