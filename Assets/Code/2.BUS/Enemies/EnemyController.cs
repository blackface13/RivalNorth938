using Assets.Code._4.CORE;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    [TabGroup("Cấu hình thuộc tính")]
    [Title("ID")]
    public int EnemyID;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Tốc độ di chuyển")]
    public float MoveSpeed;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Khoảng cách xuất hiện")]
    public float ActiveRange;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Khoảng cách phát hiện player")]
    public float DetectRange;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Khoảng cách tấn công")]
    public float AttackRange;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Cân nặng")]
    public float Weight;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Bao nhiêu đòn đánh thường")]
    public float TotalAtkAnim;

    [TabGroup("Misc")]
    public Actions CurrentAction = Actions.Idle;
    [TabGroup("Misc")]
    public bool IsViewLeft;//Hướng nhìn
    [TabGroup("Misc")]
    public bool IsMoving;//Di chuyển
    [TabGroup("Misc")]
    public bool IsAtking;
    [TabGroup("Misc")]
    public bool IsHited;
    [TabGroup("Misc")]
    public Rigidbody2D ThisRigid2D;

    private Animator Anim;
    private bool AllowMove = true;//Cho phép nhân vật di chuyển hay ko
    private bool IsActive;
    private int HitedCount;//Bộ đếm số lần bị đánh
    public enum Actions//Hành động hiện tại
    {
        Idle,
        Move,
        Jump,
        Surf,
        Atk,
        Hited
    }
    private GameObject ThisBody;
    private bool IsViewingLeft;//Hướng đang nhìn (dành cho skill)
    private float DistanceToPlayer;//Khoảng cách từ enemy tới player
    #endregion

    #region Initialize
    private void Awake()
    {
        ThisBody = this.transform.GetChild(0).gameObject;
        Anim = GetComponent<Animator>();
        ThisRigid2D = GetComponent<Rigidbody2D>();
        ThisRigid2D.gravityScale = Weight;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        IsActive = true;
    }
    #endregion

    #region Functions
    // Update is called once per frame
    void Update()
    {
        ActiveController();
        if (IsActive)
        {
            MoveController();
        }
        //print(CurrentAction);
    }

    /// <summary>
    /// Điều khiển ẩn hiện khi gần player
    /// </summary>
    private void ActiveController()
    {
        DistanceToPlayer = Vector2.Distance(GameSettings.Player.transform.position, this.transform.position);
        if (DistanceToPlayer < ActiveRange)
        {
            if (!ThisBody.activeSelf)
            {
                ThisBody.SetActive(true);
                IsActive = true;
            }
        }
        else
        {
            if (ThisBody.activeSelf)
            {
                ThisBody.SetActive(false);
                IsActive = false;
            }
        }
    }

    /// <summary>
    /// Điều khiển di chuyển
    /// </summary>
    private void MoveController()
    {
        //Trong vùng di chuyển
        if (AllowMove)
        {
            if (DistanceToPlayer < DetectRange && DistanceToPlayer > AttackRange)
            {
                if (CurrentAction.Equals(Actions.Move) || CurrentAction.Equals(Actions.Idle))
                {
                    if (transform.position.x < GameSettings.Player.transform.position.x)
                    {
                        IsViewLeft = false;
                        SetView();
                    }
                    else
                    {
                        IsViewLeft = true;
                        SetView();
                    }

                    if ((transform.position.x < GameSettings.Player.transform.position.x && GameSettings.Player.transform.position.x - transform.position.x > 1f) ||
                        (transform.position.x > GameSettings.Player.transform.position.x && transform.position.x - GameSettings.Player.transform.position.x > 1f))
                    {
                        ThisRigid2D.velocity = new Vector2((IsViewLeft ? -1 : 1) * MoveSpeed, ThisRigid2D.velocity.y);
                        if (!CurrentAction.Equals(Actions.Move))
                            SetAnimation(Actions.Move);
                    }
                    //this.transform.Translate(new Vector2(IsViewLeft ? -1 : 1, 0) * MoveSpeed * Time.deltaTime);
                }
            }
            else
            {
                //Trong phạm vi tấn công
                if (DistanceToPlayer <= AttackRange)
                {
                    if (!CurrentAction.Equals(Actions.Atk))
                        SetAnimation(Actions.Atk);
                }
                else //Ngoài phạm vi phát hiện player
                {
                    if (!CurrentAction.Equals(Actions.Idle))
                    {
                        SetAnimation(Actions.Idle);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set hướng nhìn trái phải
    /// </summary>
    /// <returns></returns>
    private void SetView()
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
    /// Gán animation cho nhân vật
    /// </summary>
    public virtual void SetAnimation(Actions action)
    {
        if (action.Equals(Actions.Move))
            IsMoving = true;
        if (action.Equals(Actions.Atk))
            IsAtking = true;
        if (action.Equals(Actions.Hited))
            IsHited = true;
        Anim.SetTrigger(action.ToString() + (action.Equals(Actions.Atk) ? ((int)Random.Range(1, TotalAtkAnim + 1)).ToString() : ""));
        CurrentAction = action;
    }

    /// <summary>
    /// Kết thúc anim tấn công
    /// </summary>
    public void EndAtk()
    {
        AllowMove = false;
        if (transform.position.x < GameSettings.Player.transform.position.x)
            IsViewLeft = false;
        else
            IsViewLeft = true;
        SetView();
        IsAtking = false;
        IsHited = false;
        SetAnimation(Actions.Idle);
        StartCoroutine(WaitForEndHited(HitedCount));
    }

    /// <summary>
    /// Khoảng thời gian bị choáng sau khi bị đánh trúng
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForEndHited(int hitedCount)
    {
        yield return new WaitForSeconds(Random.Range(.1f, 1.5f));
        if (hitedCount.Equals(HitedCount))
            AllowMove = true;
    }

    #endregion

    #region Physics

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillPlayer))
        {
            HitedCount++;
        }
    }
    #endregion
}
