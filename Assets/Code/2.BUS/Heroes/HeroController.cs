using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    #region Variables
    [Title("JoyStick di chuyển")]
    public Joystick JoystickController;
    [Title("Thời gian delay skill lướt")]
    public float SurfDelayTime;
    [Title("Lực nhảy")]
    public float JumpForce;
    [Title("Độ nặng của nhân vật")]
    public float HeroWeight;
    [Title("Tốc độ di chuyển")]
    public float MoveSpeed;
    [Title("Nút di chuyển")]
    public RectTransform JoystickHandle;

    [Title("Phần dưới này không cần bận tâm")]
    private float DelayTime2Atk = .3f;//Thời gian chờ đợi giữa combo atk khi ng chơi nghỉ nhấn
    public bool IsMoving = false;//Di chuyển
    public bool IsJumping = false;//Nhảy
    public bool IsSurfing = false;//Lướt
    public bool IsViewLeft = false;//Hướng nhìn
    public bool IsViewingLeft = false;//Hướng đang nhìn (dành cho skill)
    public bool IsAtking = false;//Có đang thực hiện tấn công hay ko
    private bool IsPressAtk = false;//Có đang nhấn atk hay ko
    private bool IsPressMove = false;//Có đang chạm nút di chuyển hay ko
    private float[] ControlTimeComboNormalAtk; //Điều khiển thời gian combo

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

    private void Awake()
    {
        ControlTimeComboNormalAtk = new float[2];
    }

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
        ComboAttackController();
        MoveController();
    }


    private void MoveController()
    {
        if (IsPressMove && JoystickHandle.localPosition.x != 0)
        {
            if (CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump))
                this.transform.Translate(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime);
            //HeroRigidBody2D.AddForce(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime,ForceMode2D.Impulse);
            if (CurrentAction.Equals(Actions.Idle))// && !IsJumping && !IsSurfing && !IsMoving && !IsAtking)
            {
                SetAnimation(HeroController.Actions.Move);
                IsMoving = true;
            }

            if (JoystickHandle.localPosition.x < 0)
            {
                IsViewLeft = true;
                if (CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump))
                    SetView();
            }
            else
            {
                IsViewLeft = false;
                if (CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump))
                    SetView();
            }

        }
        //HeroRigidBody2D.velocity = new Vector2(JoystickController.Horizontal * MoveSpeed, HeroRigidBody2D.velocity.y);
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
            IsViewingLeft = IsViewLeft;
        }
        if (!IsViewLeft && transform.localScale.x > 0)
        {
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                IsViewingLeft = IsViewLeft;
            }
        }
    }

    /// <summary>
    /// Lướt
    /// </summary>
    public void ActionSurf(BaseEventData eventData)
    {
        if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump))
        //if (!IsSurfing)
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
        if (!IsAtking)
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
        // if (!IsJumping && !IsAtking && !IsSurfing)
        if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Move))
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

                ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];
                if (IsJumping)
                    HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                IsAtking = true;
                Anim.SetTrigger(CurrentWeapon + action.ToString() + (CurrentCombo + 1).ToString());
                if (!IsJumping)
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
        ObjControl.CheckExistAndCreateEffectExtension(GetPositionSkillEffect(effectName), EffectWeaponAttack[effectName], EffectWeaponAttack[effectName][0].transform.rotation, IsViewingLeft);
    }

    /// <summary>
    /// Trả về tọa độ để hiển thị skill effect
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPositionSkillEffect(string effectName)
    {
        if (IsViewingLeft)
            return new Vector3(this.transform.position.x - GameSettings.SkillsPosition[effectName].x, this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
        else
            return new Vector3(this.transform.position.x + GameSettings.SkillsPosition[effectName].x, this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
    }

    /// <summary>
    /// Không freeze tọa độ Y với nhân vật nữa(hàm này gọi trong animation)
    /// </summary>
    public void DisableFreezeY()
    {
        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        HeroRigidBody2D.AddForce(new Vector2(.001f, 0), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Kết thúc anim tấn công
    /// </summary>
    public void EndAtk()
    {
        SetView();
        IsAtking = false;
        IsAlowAtk = true;
        if (!IsPressAtk && !IsPressMove)
            SetAnimation(Actions.Idle);
        if (IsPressMove && !IsPressAtk)
            SetAnimation(Actions.Move);
        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        HeroRigidBody2D.AddForce(new Vector2(.001f, 0), ForceMode2D.Impulse);

    }

    /// <summary>
    /// Cho phép nhảy sang animation khác (Dành cho atk liên tiếp)
    /// </summary>
    public void CanNextAnim()
    {
        SetView();
        IsAlowAtk = true;
        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        HeroRigidBody2D.AddForce(new Vector2(.001f, 0), ForceMode2D.Impulse);
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
                HeroRigidBody2D.velocity = Vector3.zero;
                HeroRigidBody2D.angularVelocity = 0;
                CurrentComboTmp = CurrentCombo;
                SetAnimation(Actions.Atk, true);
                if (CurrentCombo >= GameSettings.MaxAtkCombo)
                    CurrentCombo = 0;
                else CurrentCombo++;
            }
        }
    }

    /// <summary>
    /// Điều khiển combo của normal atk, ngắt combo sau 1 giây
    /// </summary>
    public void ComboAttackController()
    {
        if (CurrentCombo > GameSettings.MaxAtkCombo)
            CurrentCombo = 0;
        ControlTimeComboNormalAtk[1] += 1 * Time.deltaTime;
        if (CurrentCombo > 0)
        {
            if (ControlTimeComboNormalAtk[1] - ControlTimeComboNormalAtk[0] >= GameSettings.TimeDelayComboNormalAtk)
                CurrentCombo = 0;
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
            if (CurrentAction.Equals(Actions.Jump) || CurrentAction.Equals(Actions.Surf))
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
    public void BtnMoveDown(BaseEventData eventData)
    {
        IsPressMove = true;
    }

    public void BtnMoveUp(BaseEventData eventData)
    {
        IsPressMove = false;
        if (CurrentAction.Equals(Actions.Move))
        {
            SetAnimation(Actions.Idle);
            IsMoving = false;
        }
    }


    public void Test(BaseEventData eventData)
    {
        print("OK");
    }
    #endregion
}
