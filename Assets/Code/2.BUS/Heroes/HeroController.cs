using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    #region Variables
    [Title("Thời gian delay skill lướt")]
    public float SurfDelayTime;
    [Title("Lực nhảy")]
    public float JumpForce;
    [Title("Độ nặng của nhân vật")]
    public float HeroWeight;

    [Title("Phần dưới này không cần bận tâm")]
    public bool IsMoving = false;//Di chuyển
    public bool IsJumping = false;//Nhảy
    public bool IsSurfing = false;//Lướt
    public bool IsViewLeft = false;//Hướng nhìn
    private SpriteRenderer HeroSpriteRenderer;
    private Rigidbody2D HeroRigidBody2D;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        HeroSpriteRenderer = this.GetComponent<SpriteRenderer>();
        HeroRigidBody2D = this.GetComponent<Rigidbody2D>();
        HeroRigidBody2D.gravityScale = HeroWeight;
    }

    #region Functions
    // Update is called once per frame
    void Update()
    {
        //print(IsJumping);
    }

    /// <summary>
    /// Set hướng nhìn trái phải
    /// </summary>
    /// <returns></returns>
    public void SetView()
    {
        if (IsViewLeft && !HeroSpriteRenderer.flipX)
            HeroSpriteRenderer.flipX = true;
        if (!IsViewLeft && HeroSpriteRenderer.flipX)
            HeroSpriteRenderer.flipX = false;
    }

    /// <summary>
    /// Lướt
    /// </summary>
    public void ActionSurf(BaseEventData eventData)
    {
        if (!IsSurfing)
        {
            if (IsViewLeft)
                HeroRigidBody2D.AddForce(new Vector2(0- JumpForce, 0), ForceMode2D.Impulse);
            else
                HeroRigidBody2D.AddForce(new Vector2(JumpForce, 0), ForceMode2D.Impulse);

            if (IsJumping) HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
            IsSurfing = true;
            StartCoroutine(WaitSurf());
        }
    }

    private IEnumerator WaitSurf()
    {
        yield return new WaitForSeconds(.2f);
        if (IsJumping) HeroRigidBody2D.constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(SurfDelayTime - .2f);
        IsSurfing = false;
    }

    /// <summary>
    /// Nhảy
    /// </summary>
    public void ActionJump(BaseEventData  eventData)
    {
        if (!IsJumping)
        {
            IsJumping = true;
            HeroRigidBody2D.AddForce(new Vector2(0f, 100f), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Physics

    /// <summary>
    /// Va chạm không xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
        {
            IsJumping = false;
        }
    }

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter2D(Collider2D col)
    {
    }
    #endregion
}
