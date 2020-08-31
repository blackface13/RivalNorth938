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
    private float DelayTime2Atk = .3f;//Thời gian chờ đợi giữa combo atk khi ng chơi nghỉ nhấn
    public bool IsMoving = false;//Di chuyển
    public bool IsJumping = false;//Nhảy
    public bool IsSurfing = false;//Lướt
    public bool IsViewLeft = false;//Hướng nhìn
    public bool IsAtking = false;//Có đang thực hiện tấn công hay ko
    private bool IsPressAtk = false;//Có đang nhấn atk hay ko

    private Animator Anim;
    private SpriteRenderer HeroSpriteRenderer;
    private Rigidbody2D HeroRigidBody2D;
    private int CurrentCombo, CurrentComboTmp = 0;
    ObjectController ObjControl;

    public enum Weapons//Vũ khí nhân vật có thể sử dụng
    {
        Blade,
        Staff
    }
    public Weapons CurrentWeapon = Weapons.Blade;
    public enum Actions//Hành động hiện tại
    {
        Idle,
        Move,
        Jump,
        Surf,
        Atk
    }
    public Actions CurrentAction = Actions.Idle;

    private bool IsAlowAtk = true;//Xác định cho phép thực hiện anim atk hay ko
    //private bool IsStay = false;
    private Dictionary<string, List<GameObject>> EffectWeaponAttack;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        EffectWeaponAttack = new Dictionary<string, List<GameObject>>();
        ObjControl = new ObjectController();
        HeroSpriteRenderer = this.GetComponent<SpriteRenderer>();
        HeroRigidBody2D = this.GetComponent<Rigidbody2D>();
        HeroRigidBody2D.gravityScale = HeroWeight;
        Anim = this.GetComponent<Animator>();
        GameSettings.CreateSkillsPosition();
        CreateEffectWeapons(Weapons.Blade);
        CreateEffectWeapons(Weapons.Staff);
    }

    /// <summary>
    /// Khởi tạo các object hiệu ứng đòn đánh
    /// </summary>
    private void CreateEffectWeapons(Weapons wp)
    {
        switch (wp)
        {
            case Weapons.Blade:
                EffectWeaponAttack.Add("BladeAtk1", ObjControl.CreateListSkillObject("BladeAtk1", 1, Quaternion.Euler(70f, 0, 50f)));
                EffectWeaponAttack.Add("BladeAtk2", ObjControl.CreateListSkillObject("BladeAtk2", 1, Quaternion.Euler(80f, 0, 0)));
                EffectWeaponAttack.Add("BladeAtk3", ObjControl.CreateListSkillObject("BladeAtk3", 1, Quaternion.Euler(0, 0, -153f)));
                break;
            case Weapons.Staff:
                //EffectWeaponAttack.Add("BladeAtk1", ObjControl.CreateListSkillObject("BladeAtk1", 1, Quaternion.Euler(70f, 0, 50f)));
                EffectWeaponAttack.Add("StaffAtk2", ObjControl.CreateListSkillObject("StaffAtk2", 1, Quaternion.Euler(0, 0, 190f)));
                EffectWeaponAttack.Add("StaffAtk3", ObjControl.CreateListSkillObject("StaffAtk3", 1, Quaternion.Euler(0, 0, 190f)));
                break;
            default: break;
        }
        //ObjControl.CheckExistAndCreateEffectExtension(new Vector3(0, 0, 0), EffectWeaponAttack[0][Weapons.Blade], EffectWeaponAttack[0][Weapons.Blade][0].transform.rotation, IsViewLeft);
    }

    #region Functions
    // Update is called once per frame
    void Update()
    {
        // print(IsStay);
        AttackActionController();
    }

    /// <summary>
    /// Set vũ khí cho nhân vật
    /// </summary>
    /// <param name="weapon"></param>
    private void ChangeWeapon(Weapons weapon)
    {
        CurrentWeapon = weapon;
        SetAnimation(CurrentAction);
        //switch (weapon)
        //{
        //    case Weapons.Blade:
        //        CurrentWeapon = Weapons.Blade;
        //        SetAnimation(CurrentAction);
        //        break;
        //    case Weapons.Staff:
        //        CurrentWeapon = Weapons.Staff;
        //        SetAnimation(CurrentAction);
        //        break;
        //    default: break;
        //}
    }

    public void ChangeWeaponTmp(BaseEventData eventData)
    {
        CurrentWeapon = CurrentWeapon.Equals(Weapons.Blade) ? Weapons.Staff : Weapons.Blade;
        SetAnimation(CurrentAction);
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
    public void SetAnimation(Actions action, bool isAtk = false)
    {
        if (isAtk)
        {
            if (IsPressAtk)
            {
                IsAtking = true;
                Anim.SetTrigger(CurrentWeapon + action.ToString() + (CurrentCombo + 1).ToString());
                HeroRigidBody2D.AddForce(new Vector2(IsViewLeft ? -40f : 40f, 0f), ForceMode2D.Impulse);
                CurrentAction = action;
                IsAlowAtk = false;
            }
            else goto End;
        }
    Begin:
        {
            Anim.SetTrigger(CurrentWeapon + action.ToString());
            CurrentAction = action;
        }
    End: { }
        //switch (action)
        //{
        //    case Actions.Move:
        //        Anim.SetTrigger(CurrentWeapon + "Move");
        //        CurrentAction = action;
        //        break;
        //    case Actions.Idle:
        //        Anim.SetTrigger(CurrentWeapon + "Idle");
        //        CurrentAction = action;
        //        break;
        //    case Actions.Jump:
        //        Anim.SetTrigger(CurrentWeapon + "Jump");
        //        CurrentAction = action;
        //        break;
        //    case Actions.Surf:
        //        Anim.SetTrigger(CurrentWeapon + "Surf");
        //        CurrentAction = action;
        //        break;
        //    default: break;
        //}
    }

    /// <summary>
    /// Hàm này gọi từ animation
    /// </summary>
    public void ShowEffect(string effectName)
    {
        ObjControl.CheckExistAndCreateEffectExtension(GetPositionSkillEffect(effectName), EffectWeaponAttack[effectName], EffectWeaponAttack[effectName][0].transform.rotation, IsViewLeft);
    }

    /// <summary>
    /// Trả về tọa độ để hiển thị skill effect
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPositionSkillEffect(string effectName)
    {
        if (IsViewLeft)
            return new Vector3(this.transform.position.x - GameSettings.SkillsPosition[effectName].x, this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
        else
            return new Vector3(this.transform.position.x + GameSettings.SkillsPosition[effectName].x, this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
    }

    /// <summary>
    /// Kết thúc anim tấn công
    /// </summary>
    public void EndAtk()
    {
        IsAtking = false;
        IsAlowAtk = true;
        if (!IsPressAtk)
            SetAnimation(Actions.Idle);
        if (IsMoving)
            SetAnimation(Actions.Move);

    }

    /// <summary>
    /// Cho phép nhảy sang animation khác (Dành cho atk liên tiếp)
    /// </summary>
    public void CanNextAnim()
    {
        IsAlowAtk = true;
    }

    /// <summary>
    /// Điều khiển atk
    /// </summary>
    private void AttackActionController()
    {
        if (IsPressAtk)
        {
            if (IsAlowAtk)
            {
                CurrentComboTmp = CurrentCombo;
                SetAnimation(Actions.Atk, true);
                if (CurrentCombo >= GameSettings.MaxAtkCombo)
                    CurrentCombo = 0;
                else CurrentCombo++;
            }
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
            if (IsJumping)
                SetAnimation(Actions.Idle);
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

    #region Events
    /// <summary>
    /// Nhấn nút atk
    /// </summary>
    /// <param name="eventData"></param>
    public void BtnAtkDown(BaseEventData eventData)
    {
        IsPressAtk = true;
    }

    /// <summary>
    /// Nhả nút atk
    /// </summary>
    /// <param name="eventData"></param>
    public void BtnAtkUp(BaseEventData eventData)
    {
        IsPressAtk = false;
    }
    #endregion
}
