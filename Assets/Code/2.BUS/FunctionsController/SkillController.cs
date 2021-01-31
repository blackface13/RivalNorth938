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
    [Title("Là object con")]
    public bool IsChild;
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
    [Title("Disable sát thương sau 1 khoảng time")]
    public bool DisableColliderRealTime;
    [TabGroup("Cấu hình")]
    [Title("Thời gian disable gây sát thương")]
    public float DelayTimeDisableCollider;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy đối phương về sau")]
    public Vector2 ForceToVictim;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy đối phương cộng thêm (dành cho di chuyển + đánh)")]
    public float ForceToVictimBonus;
    [TabGroup("Cấu hình")]
    [Title("Hất tung đối thủ hay ko")]
    public bool IsPushUp;
    [TabGroup("Cấu hình")]
    [Title("Lực hất tung")]
    public float ForcePushUp;
    [TabGroup("Cấu hình")]
    [Title("Đẩy đối thủ trên xuống hay ko")]
    public bool IsPushDown;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy trên xuống")]
    public float ForcePushDown;
    [TabGroup("Cấu hình")]
    [Title("Đẩy đối thủ trên xuống khi Player đang nhảy hay ko")]
    public bool IsPushDownOnJump;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy trên xuống khi đang nhảy")]
    public float ForcePushDownOnJump;
    [TabGroup("Cấu hình")]
    [Title("Đẩy lùi đối thủ khi đạp đối thủ xuống chạm đất hay ko")]
    public bool IsRepelWhenTouchLane;
    [TabGroup("Cấu hình")]
    [Title("Lực đẩy lùi đối thủ khi đạp đối thủ xuống chạm đất")]
    public float ForceRepelWhenTouchLane;
    [TabGroup("Misc")]
    public float ForceKeepPushUp = 22f;//Lực đẩy vừa đủ giữ đối phương trên không
    [TabGroup("Misc")]
    private Collider2D ThisCollider;
    private SkillController ControlParent;
    private float OriginalScaleX;//Scale ban đầu của object

    #region Initialize

    public virtual void Awake()
    {
        OriginalScaleX =  this.transform.localScale.x;
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

        //Dừng gây sát thương sau 1 khoảng time
        if(DisableColliderRealTime)
            StartCoroutine(AutoDisableCollider());

        //IsViewLeft = IsChild ? ControlParent.IsViewLeft : IsViewLeft;

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

    /// <summary>
    /// Tự động disable collider sau 1 khoảng time
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator AutoDisableCollider()
    {
        yield return new WaitForSeconds(DelayTimeDisableCollider);
        ThisCollider.enabled = false;
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
        
    }
    #endregion
}
