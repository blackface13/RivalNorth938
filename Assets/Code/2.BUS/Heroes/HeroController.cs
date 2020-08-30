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

    private Animator Anim;
    private SpriteRenderer HeroSpriteRenderer;
    private Rigidbody2D HeroRigidBody2D;

    public enum Weapons
    {
        Blade,
        Staff
    }
    public Weapons CurentWeapon = Weapons.Blade;
    public enum Actions
    {
        Idle,
        Move,
        Jump,
        Surf,
    }
    public Actions CurentAction = Actions.Idle;
    //private bool IsStay = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        HeroSpriteRenderer = this.GetComponent<SpriteRenderer>();
        HeroRigidBody2D = this.GetComponent<Rigidbody2D>();
        HeroRigidBody2D.gravityScale = HeroWeight;
        Anim = this.GetComponent<Animator>();
    }

    #region Functions
    // Update is called once per frame
    void Update()
    {
        // print(IsStay);
    }

    /// <summary>
    /// Set vũ khí cho nhân vật
    /// </summary>
    /// <param name="weapon"></param>
    private void ChangeWeapon(Weapons weapon)
    {
        CurentWeapon = weapon;
        SetAnimation(CurentAction);
        //switch (weapon)
        //{
        //    case Weapons.Blade:
        //        CurentWeapon = Weapons.Blade;
        //        SetAnimation(CurentAction);
        //        break;
        //    case Weapons.Staff:
        //        CurentWeapon = Weapons.Staff;
        //        SetAnimation(CurentAction);
        //        break;
        //    default: break;
        //}
    }

    public void ChangeWeaponTmp(BaseEventData eventData)
    {
        CurentWeapon = CurentWeapon.Equals(Weapons.Blade) ? Weapons.Staff: Weapons.Blade;
        SetAnimation(CurentAction);
    }

    /// <summary>
    /// Set hướng nhìn trái phải
    /// </summary>
    /// <returns></returns>
    public void SetView()
    {
        if (IsViewLeft && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (!IsViewLeft && transform.localScale.x > 0)
        {
            { transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); }
        }
    }

    /// <summary>
    /// Lướt
    /// </summary>
    public void ActionSurf(BaseEventData eventData)
    {
        if (!IsSurfing)
        {
            HeroRigidBody2D.velocity = Vector3.zero;
            HeroRigidBody2D.gravityScale = 0;

            if (IsViewLeft)
                HeroRigidBody2D.AddForce(new Vector2(0 - JumpForce, 0), ForceMode2D.Impulse);
            else
                HeroRigidBody2D.AddForce(new Vector2(JumpForce, 0), ForceMode2D.Impulse);
            //HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
            IsSurfing = true;
            IsJumping = true;
            SetAnimation(Actions.Surf);
            StartCoroutine(WaitSurf());
        }
    }

    private IEnumerator WaitSurf()
    {
        IsMoving = false;
        yield return new WaitForSeconds(.2f);
            SetAnimation(Actions.Idle);
        IsJumping = false;
        HeroRigidBody2D.gravityScale = HeroWeight;
        //HeroRigidBody2D.constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(SurfDelayTime - .2f);
        IsSurfing = false;
    }

    /// <summary>
    /// Nhảy
    /// </summary>
    public void ActionJump(BaseEventData eventData)
    {
        if (!IsJumping)
        {
            IsJumping = true;
            IsMoving = false;
            HeroRigidBody2D.AddForce(new Vector2(0f, 100f), ForceMode2D.Impulse);
            SetAnimation(Actions.Jump);
        }
    }

    /// <summary>
    /// Gán animation cho nhân vật
    /// </summary>
    public void SetAnimation(Actions action)
    {
        switch (action)
        {
            case Actions.Move:
                Anim.SetTrigger(CurentWeapon + "Move");
                CurentAction = action;
                break;
            case Actions.Idle:
                Anim.SetTrigger(CurentWeapon + "Idle");
                CurentAction = action;
                break;
            case Actions.Jump:
                Anim.SetTrigger(CurentWeapon + "Jump");
                CurentAction = action;
                break;
            case Actions.Surf:
                Anim.SetTrigger(CurentWeapon + "Surf");
                CurentAction = action;
                break;
            default: break;
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
            SetAnimation(Actions.Idle);
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
