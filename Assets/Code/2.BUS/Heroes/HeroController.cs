using Assets.Code._2.BUS.NPCs;
using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    #region Variables
    [TabGroup("Cấu hình thuộc tính")]
    [Title("JoyStick di chuyển")]
    public Joystick JoystickController;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Thời gian delay skill lướt")]
    public float SurfDelayTime;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Lực lướt")]
    public float SurfForce;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Image delay lướt")]
    public Image ImgSurfDelay;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Lực nhảy")]
    public float JumpForce;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Lực lướt khi atk")]
    public float AtkForce;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Độ nặng của nhân vật")]
    public float HeroWeight;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Tốc độ di chuyển")]
    public float MoveSpeed;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Nút di chuyển")]
    public RectTransform JoystickHandle;
    [TabGroup("Cấu hình thuộc tính")]
    public List<GameObject> Light;
    [TabGroup("Cấu hình thuộc tính")]
    public List<GameObject> Torch;
    [TabGroup("Cấu hình thuộc tính")]
    public List<GameObject> Shader;
    [TabGroup("Cấu hình thuộc tính")]
    public List<GameObject> Map;

    [TabGroup("Misc")]
    private float DelayTime2Atk = .3f;//Thời gian chờ đợi giữa combo atk khi ng chơi nghỉ nhấn
    [TabGroup("Misc")]
    public bool IsMoving = false;//Di chuyển
    [TabGroup("Misc")]
    public bool IsJumping = false;//Nhảy
    [TabGroup("Misc")]
    public bool IsSurfing = false;//Lướt
    [TabGroup("Misc")]
    public bool IsViewLeft = false;//Hướng nhìn
    [TabGroup("Misc")]
    public bool IsViewingLeft = false;//Hướng đang nhìn (dành cho skill)
    [TabGroup("Misc")]
    public bool IsAtking = false;//Có đang thực hiện tấn công hay ko
    [TabGroup("Misc")]
    public bool IsPressAtk = false;//Có đang nhấn atk hay ko
    [TabGroup("Misc")]
    private bool IsPressMove = false;//Có đang chạm nút di chuyển hay ko
    [TabGroup("Misc")]
    public Rigidbody2D HeroRigidBody2D;
    [TabGroup("Misc")]
    public Weapons CurrentWeapon = Weapons.Blade;
    [TabGroup("Misc")]
    public Actions CurrentAction = Actions.Idle;
    [TabGroup("Misc")]
    public bool IsAllowAtk = true;//Xác định cho phép thực hiện anim atk hay ko
    [TabGroup("Misc")]
    public bool IsAllowSurf = true;//Xác định cho phép thực hiện lướt hay ko
    [TabGroup("Misc")]
    public bool IsAutoJumping;//Nhảy bị động bởi các object hỗ trợ
    [TabGroup("Misc")]
    public bool IsAtkPushUp;//Đang thực hiện đòn đánh hất tung

    ContactPoint2D[] points = new ContactPoint2D[20];
    Vector3 wallNormal; //this will store the vector that points out from the wall
    bool IsTouchingWall;//Chạm tường
    bool IsTouchingLane;//Chạm đất
    private bool? IsTouchWallLeft = null;
    private bool IsAtkMoving;//Đòn đánh thường có di chuyển nhân vật hay ko

    private float[] ControlTimeComboNormalAtk; //Điều khiển thời gian combo
    private Animator Anim;
    private SpriteRenderer HeroSpriteRenderer;
    private int CurrentCombo, CurrentComboTmp = 0;
    private bool IsKeyboardPress;

    public enum Weapons//Vũ khí nhân vật có thể sử dụng
    {
        Blade,
        Staff,
        Katana,
        Sword
    }
    public enum Actions//Hành động hiện tại
    {
        Idle,
        Stand,
        Move,
        Jump,
        Surf,
        Atk,
        AtkPushUp,
        AtkPushDown
    }

    //private bool IsStay = false;
    private Dictionary<string, List<GameObject>> EffectWeaponAttack;
    #endregion

    #region Initialize

    private void Awake()
    {
        ControlTimeComboNormalAtk = new float[2];
    }

    // Start is called before the first frame update
    void Start()
    {
        EffectWeaponAttack = new Dictionary<string, List<GameObject>>();
        GameSettings.ObjControl = new ObjectController();
        HeroSpriteRenderer = this.GetComponent<SpriteRenderer>();
        HeroRigidBody2D = this.GetComponent<Rigidbody2D>();
        HeroRigidBody2D.gravityScale = HeroWeight;
        Anim = this.GetComponent<Animator>();
        GameSettings.CreateSkillsPosition();
        CreateEffectWeapons(Weapons.Blade);
        CreateEffectWeapons(Weapons.Staff);
        CreateEffectWeapons(Weapons.Katana);
        CreateEffectWeapons(Weapons.Sword);
        ChangeWeaponTmp(null);
    }

    /// <summary>
    /// Khởi tạo các object hiệu ứng đòn đánh
    /// </summary>
    private void CreateEffectWeapons(Weapons wp)
    {
        switch (wp)
        {
            case Weapons.Blade:
                EffectWeaponAttack.Add("BladeAtk1", GameSettings.ObjControl.CreateListSkillObject("BladeAtk1", 1, Quaternion.identity));
                EffectWeaponAttack.Add("BladeAtk2", GameSettings.ObjControl.CreateListSkillObject("BladeAtk2", 1, Quaternion.Euler(80f, 0, 0)));
                EffectWeaponAttack.Add("BladeAtk3", GameSettings.ObjControl.CreateListSkillObject("BladeAtk3", 1, Quaternion.identity));
                EffectWeaponAttack.Add("BladeAtkPushUp", GameSettings.ObjControl.CreateListSkillObject("BladeAtkPushUp", 1, Quaternion.identity));
                break;
            case Weapons.Staff:
                EffectWeaponAttack.Add("StaffAtk1", GameSettings.ObjControl.CreateListSkillObject("StaffAtk1", 1, Quaternion.identity));
                EffectWeaponAttack.Add("StaffAtk2", GameSettings.ObjControl.CreateListSkillObject("StaffAtk2", 1, Quaternion.identity));
                EffectWeaponAttack.Add("StaffAtk3", GameSettings.ObjControl.CreateListSkillObject("StaffAtk3", 1, Quaternion.identity));
                EffectWeaponAttack.Add("StaffAtkPushUp", GameSettings.ObjControl.CreateListSkillObject("StaffAtkPushUp", 1, Quaternion.identity));
                break;
            case Weapons.Katana:
                EffectWeaponAttack.Add("KatanaAtk1", GameSettings.ObjControl.CreateListSkillObject("KatanaAtk1", 1, Quaternion.identity));
                EffectWeaponAttack.Add("KatanaAtk2", GameSettings.ObjControl.CreateListSkillObject("KatanaAtk2", 1, Quaternion.identity));
                EffectWeaponAttack.Add("KatanaAtk3_1", GameSettings.ObjControl.CreateListSkillObject("KatanaAtk3_1", 1, Quaternion.identity));
                EffectWeaponAttack.Add("KatanaAtk3_2", GameSettings.ObjControl.CreateListSkillObject("KatanaAtk3_2", 1, Quaternion.identity));
                EffectWeaponAttack.Add("KatanaAtkPushUp", GameSettings.ObjControl.CreateListSkillObject("KatanaAtkPushUp", 1, Quaternion.identity));
                break;
            case Weapons.Sword:
                EffectWeaponAttack.Add("SwordAtk1", GameSettings.ObjControl.CreateListSkillObject("SwordAtk1", 1, Quaternion.identity));
                EffectWeaponAttack.Add("SwordAtk2", GameSettings.ObjControl.CreateListSkillObject("SwordAtk2", 1, Quaternion.identity));
                EffectWeaponAttack.Add("SwordAtk3", GameSettings.ObjControl.CreateListSkillObject("SwordAtk3", 1, Quaternion.identity));
                EffectWeaponAttack.Add("SwordAtkPushUp", GameSettings.ObjControl.CreateListSkillObject("SwordAtkPushUp", 1, Quaternion.identity));
                //EffectWeaponAttack.Add("StaffAtk2", GameSettings.ObjControl.CreateListSkillObject("StaffAtk2", 1, Quaternion.Euler(0, 0, 190f)));
                //EffectWeaponAttack.Add("StaffAtk3", GameSettings.ObjControl.CreateListSkillObject("StaffAtk3", 1, Quaternion.Euler(0, 0, 190f)));
                break;
            default: break;
        }
        //GameSettings.ObjControl.CheckExistAndCreateEffectExtension(new Vector3(0, 0, 0), EffectWeaponAttack[0][Weapons.Blade], EffectWeaponAttack[0][Weapons.Blade][0].transform.rotation, IsViewLeft);
    }

    #endregion

    #region Functions
    // Update is called once per frame
    void Update()
    {
        if (GameSettings.IsAllowActions)
        {
            AttackActionController();
            ComboAttackController();
            MoveController();
            RespawnController();
            KeyPressController();
        }
        //print(CurrentAction);
    }

    /// <summary>
    /// Điều khiển hồi chiêu
    /// </summary>
    private void RespawnController()
    {
        //Hồi kỹ  năng lướt
        if (!IsAllowSurf)
        {
            if (ImgSurfDelay.fillAmount <= 0)
            {
                ImgSurfDelay.fillAmount = 0;
                IsAllowSurf = true;
            }
            else
            {
                ImgSurfDelay.fillAmount -= 1000f / (SurfDelayTime * 1000f) * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Nhân vật di chuyển
    /// </summary>
    private void MoveController()
    {
        if (IsPressMove && JoystickHandle.localPosition.x != 0)
        {
            if (CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump))
                HeroRigidBody2D.velocity = new Vector2((JoystickController.Horizontal < 0 ? -1 : 1) * (IsTouchingWall ? JoystickController.Horizontal < 0 && IsTouchWallLeft == true ? 0 : JoystickController.Horizontal >= 0 && IsTouchWallLeft == false ? 0 : MoveSpeed : MoveSpeed), HeroRigidBody2D.velocity.y);
            //HeroRigidBody2D.velocity = new Vector2((JoystickController.Horizontal < 0 ? -1 : 1) *  MoveSpeed, HeroRigidBody2D.velocity.y);
            //this.transform.Translate(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime);
            //HeroRigidBody2D.AddForce(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime,ForceMode2D.Impulse);
            if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Stand))// && !IsJumping && !IsSurfing && !IsMoving && !IsAtking)
            {
                SetAnimation(Actions.Move);
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
        StartCoroutine(DelayAlowAtk());
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
        CurrentWeapon = CurrentWeapon.Equals(Weapons.Blade) ? Weapons.Staff : CurrentWeapon.Equals(Weapons.Staff) ? Weapons.Katana : CurrentWeapon.Equals(Weapons.Katana) ? Weapons.Sword : Weapons.Blade;
        if (!IsAtking)
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
        if ((CurrentAction.Equals(Actions.Idle) || (CurrentAction.Equals(Actions.Stand)) || CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Jump)) && IsAllowSurf)
        //if (!IsSurfing)
        {
            StartCoroutine(DelayAlowAtk());
            HeroRigidBody2D.velocity = Vector3.zero;
            if (IsAutoJumping)
                HeroRigidBody2D.gravityScale = 0;
            HeroRigidBody2D.velocity += (IsViewLeft ? Vector2.left : Vector2.right) * SurfForce;
            //HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
            IsSurfing = true;
            //IsJumping = true;
            SetAnimation(Actions.Surf);
            StartCoroutine(WaitSurf());
            IsAllowSurf = false;
            ImgSurfDelay.fillAmount = 1f;
        }
    }

    private IEnumerator WaitSurf()
    {
        IsMoving = false;
        yield return new WaitForSeconds(.2f);
        if (!IsAtking)
        {
            if (IsTouchingLane)
                SetAnimNormal();
            else if (IsJumping)
                SetAnimation(Actions.Jump);
            else
                SetAnimNormal();
        }
        if (IsTouchingLane)
            IsJumping = false;
        HeroRigidBody2D.velocity = Vector3.zero;
        HeroRigidBody2D.gravityScale = HeroWeight;
        //HeroRigidBody2D.constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(SurfDelayTime - .2f);
        IsSurfing = false;
        SetView();
    }

    /// <summary>
    /// Nhảy
    /// </summary>
    public void ActionJump(BaseEventData eventData)
    {
        // if (!IsJumping && !IsAtking && !IsSurfing)
        if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Stand) || CurrentAction.Equals(Actions.Move))
        {
            StartCoroutine(DelayAlowAtk());
            IsJumping = true;
            IsMoving = false;
            HeroRigidBody2D.velocity += Vector2.up * JumpForce;
            //HeroRigidBody2D.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            SetAnimation(Actions.Jump);
        }
    }

    /// <summary>
    /// Khóa atk trong 1 khoảng time ngắn, tránh lỗi nhảy và đánh hoặc lướt và đánh cùng lúc
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayAlowAtk()
    {
        IsAllowAtk = false;
        yield return new WaitForSeconds(.1f);
        IsAllowAtk = true;
    }

    /// <summary>
    /// Gán animation cho nhân vật
    /// </summary>
    public void SetAnimation(Actions action, bool isAtk = false)
    {
        if (Anim == null)
            Anim = this.GetComponent<Animator>();
        try
        {
            if (isAtk)
            {
                if (IsPressAtk)
                {

                    ControlTimeComboNormalAtk[0] = ControlTimeComboNormalAtk[1];

                    //Ko chạm đất => khóa Y
                    if (!IsTouchingLane)
                    {
                        HeroRigidBody2D.velocity = Vector3.zero;
                        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }
                    IsAtking = true;

                    //Thực hiện animation
                    Anim.SetTrigger(CurrentWeapon + action.ToString() + (IsAtkPushUp ? "" : (CurrentCombo + 1).ToString()));

                    if (!IsJumping && IsPressMove && !IsAtkPushUp)
                    {
                        HeroRigidBody2D.velocity = Vector3.zero;
                        HeroRigidBody2D.velocity += (IsViewLeft ? Vector2.left : Vector2.right) * AtkForce;
                        IsAtkMoving = true;
                    }
                    else
                        IsAtkMoving = false;
                    CurrentAction = action;
                    IsAllowAtk = false;
                    IsAtkPushUp = false;
                }
                else goto End;
            }
        Begin:
            {
                //Anim.Rebind();
                Anim.SetTrigger(CurrentWeapon + action.ToString());
                CurrentAction = action;
            }
        End: { }
        }
        catch (Exception ex) { print(ex.Message.ToString()); }

    }

    /// <summary>
    /// Hàm này gọi từ animation
    /// </summary>
    public void ShowEffect(string effectName)
    {
        GameSettings.ObjControl.CheckExistAndCreateEffectExtension(GetPositionSkillEffect(effectName), EffectWeaponAttack[effectName], EffectWeaponAttack[effectName][0].transform.rotation, IsViewingLeft, IsAtkMoving);
    }

    /// <summary>
    /// Trả về tọa độ để hiển thị skill effect
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPositionSkillEffect(string effectName)
    {
        if (IsViewingLeft)
            return new Vector3(this.transform.position.x - GameSettings.SkillsPosition[effectName].x - (IsAtkMoving ? 3f : 0), this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
        else
            return new Vector3(this.transform.position.x + GameSettings.SkillsPosition[effectName].x + (IsAtkMoving ? 3f : 0), this.transform.position.y + GameSettings.SkillsPosition[effectName].y, GameSettings.PositionZDefaultInMap);
    }

    /// <summary>
    /// Không freeze tọa độ Y với nhân vật nữa(hàm này gọi trong animation)
    /// </summary>
    public void DisableFreezeY()
    {
        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        HeroRigidBody2D.AddForce(new Vector2(.001f, IsJumping ? -30f : 0), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Kết thúc anim tấn công
    /// </summary>
    public void EndAtk()
    {
        SetView();
        IsAtking = false;
        IsAllowAtk = true;
        if (!IsPressAtk && !IsPressMove && !IsJumping)
            SetAnimNormal();
        else if (IsPressMove && !IsPressAtk && !IsJumping)
            SetAnimation(Actions.Move);
        else if (IsJumping)
            SetAnimation(Actions.Jump);
        HeroRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        HeroRigidBody2D.AddForce(new Vector2(.001f, 0), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Cho phép nhảy sang animation khác (Dành cho atk liên tiếp)
    /// </summary>
    public void CanNextAnim()
    {
        SetView();
        IsAllowAtk = true;
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
            if (IsAllowAtk)
            {
                //if (IsJumping)
                //    HeroRigidBody2D.velocity = Vector3.zero;
                CurrentComboTmp = CurrentCombo;
                if (JoystickHandle.localPosition.y > GameSettings.JoystickPosYLimitDetect && !IsJumping)
                {
                    CurrentCombo = 0;
                    IsAtkPushUp = true;
                    HeroRigidBody2D.velocity += Vector2.up * JumpForce;
                    SetAnimation(Actions.AtkPushUp, true);
                }
                else
                {
                    SetAnimation(Actions.Atk, true);
                    if (CurrentCombo >= GameSettings.MaxAtkCombo)
                        CurrentCombo = 0;
                    else CurrentCombo++;
                }
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

    /// <summary>
    /// Đưa trạng thái nhân vật về bình thường
    /// </summary>
    public void ResetAction()
    {
        IsAtking = false;
        IsJumping = false;
        IsPressAtk = false;
        IsPressMove = false;
        IsSurfing = false;
        IsAtkPushUp = false;
        IsAtkMoving = false;
    }

    /// <summary>
    /// Đưa player về trạng thái bình thường khi kết thúc anim
    /// </summary>
    public void SetAnimNormal()
    {
        if (!IsAtking && !IsPressAtk)
            SetAnimation(GameSystems.Location.Equals(GameSettings.LocationSettings.CombatRegion) ? Actions.Idle : Actions.Stand);
    }
    #endregion

    #region Physics

    /// <summary>
    /// Va chạm không xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionEnter2D(Collision2D col)
    {
        //Va chạm với mặt đất
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
        {
            if (CurrentAction.Equals(Actions.Jump) || CurrentAction.Equals(Actions.Surf))
            {
                if (IsPressMove)
                    SetAnimation(Actions.Move);
                else
                    SetAnimNormal();
            }
            IsJumping = false;
            if (IsAutoJumping)
            {
                IsAllowAtk = true;
                IsAutoJumping = false;
            }
        }
    }

    /// <summary>
    /// Va chạm không xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionExit2D(Collision2D col)
    {
        //Đổi animation khi rơi từ map xuống
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
        {
            if (!CurrentAction.Equals(Actions.Atk) && !CurrentAction.Equals(Actions.Surf))
            {
                //if (CurrentAction.Equals(Actions.Jump) || CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Stand) || CurrentAction.Equals(Actions.Move))
                //{
                //    SetAnimation(Actions.Jump);
                //}
                IsJumping = true;
            }
            IsTouchingWall = false;
            IsTouchingLane = false;
        }
    }

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.NPC))
        {
            var npcController = col.gameObject.GetComponent<NPCBase>();
            if (npcController.Choices != null)
            {
                GameSystems.GameControl.BtnActionNPC.SetActive(true);
                GameSystems.GameControl.CurrentNPC = npcController;
            }
        }
    }

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.NPC))
        {
            GameSystems.GameControl.BtnActionNPC.SetActive(false);
        }
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Lane))
        {
            int pointsAmount = col.GetContacts(points);
            IsTouchingWall = false; //don't know if we're near a wall yet
            for (int i = 0; i < pointsAmount; i++)
            {
                //if angle between the touched surface and the plane ground is more than 80 degrees, than it means this surface is kind of a wall
                if (Vector3.Angle(points[i].normal, Vector3.up) > -10 && Vector3.Angle(points[i].normal, Vector3.up) < 10)
                {
                    IsTouchingWall = false;
                    IsTouchingLane = true;
                    IsTouchWallLeft = null;
                    break;
                }
                else if (Vector3.Angle(points[i].normal, Vector3.up) > 80f)
                {
                    IsTouchWallLeft = IsViewLeft;
                    wallNormal = points[i].normal;
                    wallNormal.y = 0f; // eliminate all possible slopes of the wall touched so the vector will be ideally horizontal.
                    IsTouchingWall = true;
                    break; //quit the iteration as we already got some wall
                }
            }
        }
    }
    #endregion

    #region Events
    /// <summary>
    /// Nhấn nút atk
    /// </summary>
    /// <param name="eventData"></param>
    public void BtnAtkDown(BaseEventData eventData)
    {
        if (GameSettings.IsAllowActions && GameSystems.Location.Equals(GameSettings.LocationSettings.CombatRegion))
        {
            IsPressAtk = true;
        }
    }

    /// <summary>
    /// Nhả nút atk
    /// </summary>
    /// <param name="eventData"></param>
    public void BtnAtkUp(BaseEventData eventData)
    {
        if (GameSettings.IsAllowActions && GameSystems.Location.Equals(GameSettings.LocationSettings.CombatRegion))
        {
            IsPressAtk = false;
        }
    }

    public void BtnMoveDown(BaseEventData eventData)
    {
        if (GameSettings.IsAllowActions)
        {
            IsPressMove = true;
        }
    }

    public void BtnMoveUp(BaseEventData eventData)
    {
        if (GameSettings.IsAllowActions)
        {
            IsPressMove = false;
            if (CurrentAction.Equals(Actions.Move))
            {
                SetAnimNormal();
                IsMoving = false;
            }
        }
    }


    public void TestAnim(BaseEventData eventData)
    {
        ////Anim.Play(CurrentWeapon + Actions.Stand.ToString());
        //Anim.Play(CurrentWeapon + Actions.Move.ToString());
        //print((CurrentWeapon + Actions.Stand.ToString()).ToString());
        //SetAnimation(Actions.Stand);
        //SetAnimation(Actions.Move);

        Anim.SetTrigger(CurrentWeapon + Actions.Stand.ToString());
        Anim.Rebind();
        Anim.SetTrigger(CurrentWeapon + Actions.Move.ToString());
        print(CurrentAction);
    }
    public void DisableTest2()
    {
        try
        {
            var count = Torch.Count;
            var boolset = Torch[0].activeSelf;
            for (int i = 0; i < count; i++)
            {
                Torch[i].SetActive(!boolset);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    public void DisableTest3()
    {
        try
        {
            var count = Shader.Count;
            var boolset = Shader[0].activeSelf;
            for (int i = 0; i < count; i++)
            {
                Shader[i].SetActive(!boolset);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    public void DisableTest4()
    {
        try
        {
            gameObject.transform.position = new Vector3(0, 0, gameObject.transform.position.z);
            if (Map[0].activeSelf)
            {
                Map[0].SetActive(false);
                Map[1].SetActive(true);
            }
            else
            {
                Map[1].SetActive(false);
                Map[0].SetActive(true);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    #endregion

    #region Input


    /// <summary>
    /// Điều khiển nhân vật với bàn phím
    /// </summary>
    private void KeyPressController()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsViewLeft = IsKeyboardPress = true;
            SetView();
            //HeroRigidBody2D.AddForce(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime,ForceMode2D.Impulse);
            if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Stand))// && !IsJumping && !IsSurfing && !IsMoving && !IsAtking)
            {
                SetAnimation(HeroController.Actions.Move);
                IsMoving = true;
                IsPressMove = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            IsViewLeft = false;
            IsKeyboardPress = true;
            SetView();
            //HeroRigidBody2D.AddForce(new Vector2(JoystickController.Horizontal < 0 ? -1 : 1, 0) * MoveSpeed * Time.deltaTime,ForceMode2D.Impulse);
            if (CurrentAction.Equals(Actions.Idle) || CurrentAction.Equals(Actions.Stand))// && !IsJumping && !IsSurfing && !IsMoving && !IsAtking)
            {
                SetAnimation(HeroController.Actions.Move);
                IsMoving = true;
                IsPressMove = true;
            }
        }
        if (IsPressMove && IsKeyboardPress)
            HeroRigidBody2D.velocity = new Vector2((JoystickController.Horizontal < 0 ? -1 : 1) * (IsTouchingWall ? 0 : MoveSpeed), HeroRigidBody2D.velocity.y);
        //this.transform.Translate(new Vector2(IsViewLeft ? -1 : 1, 0) * MoveSpeed * Time.deltaTime);

        //Nhảy
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActionJump(null);
        }

        //Lướt
        if (Input.GetKeyDown(KeyCode.K))
        {
            ActionSurf(null);
        }

        //Nhả phím
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
            {
                IsPressMove = IsKeyboardPress = false;

                if (IsMoving && !IsAtking)
                    SetAnimNormal();
                IsMoving = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.A))
            {
                IsPressMove = IsKeyboardPress = false;
                if (IsMoving && !IsAtking)
                    SetAnimNormal();
                IsMoving = false;
            }
        }
    }
    #endregion
}
