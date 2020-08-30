using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    #region Variables
    [Title("Object Player")]
    public Transform Player;
    [Title("Tốc độ di chuyển, Z của canvas")]
    public float MoveSpeed = 15.0f;
    public float CanvasZ;

    [Title("Vùng giao diện move")]
    public Transform circle;
    public Transform outerCircle;

    private Vector2 startingPoint;
    Vector2 direction;
    private int leftTouch = 99;

    private bool IsTouchMove = false;
    private float LimitRangeCircle = 1.5f;//Độ kéo lớn tối đa của vòng di chuyển
    private Vector2 OutCirclePosXOriginal;//Tọa độ ban đầu của control move 

    private HeroController Hero;

    private Vector2 posTmp;
    public Text Infor;
    #endregion

    #region Inityalize
    void Awake()
    {
        OutCirclePosXOriginal = outerCircle.position;
        Hero = Player.GetComponent<HeroController>();
    }
    private void Start()
    {

    }
    #endregion

    #region Functions
    // Update is called once per frame
    void Update()
    {
        TouchController();
        KeyPressController();
        if (IsTouchMove)
            moveCharacter(direction);
    }

    /// <summary>
    /// Điều khiển nhân vật với chạm vuốt
    /// </summary>
    private void TouchController()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector2 touchPos = getTouchPosition(t.position); // * -1 for perspective cameras
            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x < Screen.width / 3 && t.position.y < Screen.height / 2)
                {
                    leftTouch = t.fingerId;
                    startingPoint = touchPos;
                    posTmp = new Vector2(Camera.main.transform.position.x, 0);
                }
            }
            else if ((t.phase == TouchPhase.Moved) && leftTouch == t.fingerId)
            {
                Vector2 offset = touchPos - (startingPoint + new Vector2(Camera.main.transform.position.x, 0) - posTmp);
                Infor.text = touchPos + ":" + (startingPoint + new Vector2(Camera.main.transform.position.x, 0) - posTmp) + ":" + posTmp;
                direction = Vector2.ClampMagnitude(offset, LimitRangeCircle);
                circle.transform.position = new Vector3(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y, CanvasZ);
                IsTouchMove = true;

                //Set hướng nhìn trái phải
                if (offset.x < 0)
                {
                    Hero.IsViewLeft = true;
                    Hero.SetView();
                }
                else
                {
                    Hero.IsViewLeft = false;
                    Hero.SetView();
                }
            }
            else if (t.phase == TouchPhase.Ended && leftTouch == t.fingerId)
            {
                if (Hero.IsMoving && !Hero.IsAtking)
                {
                    Hero.SetAnimation(HeroController.Actions.Idle);
                    Hero.IsMoving = false;
                }
                IsTouchMove = false;
                leftTouch = 99;
                circle.transform.position = new Vector3(outerCircle.transform.position.x, outerCircle.transform.position.y, CanvasZ);
            }
            ++i;
        }
    }

    /// <summary>
    /// Điều khiển nhân vật với bàn phím
    /// </summary>
    private void KeyPressController()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Hero.IsViewLeft = true;
            Hero.SetView();
            Vector2 offset = Vector2.zero - new Vector2(LimitRangeCircle, 0);
            direction = Vector2.ClampMagnitude(offset, LimitRangeCircle);
            circle.transform.position = new Vector3(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y, CanvasZ);
            IsTouchMove = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Hero.IsViewLeft = false;
            Hero.SetView();
            Vector2 offset = Vector2.zero + new Vector2(LimitRangeCircle, 0);
            direction = Vector2.ClampMagnitude(offset, LimitRangeCircle);
            circle.transform.position = new Vector3(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y, CanvasZ);
            IsTouchMove = true;
        }

        //Nhảy
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hero.ActionJump(null);
        }

        //Lướt
        if (Input.GetKeyDown(KeyCode.J))
        {
            Hero.ActionSurf(null);
        }

        //Nhả phím
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
            {
                IsTouchMove = false;

                if (Hero.IsMoving && !Hero.IsAtking)
                    Hero.SetAnimation(HeroController.Actions.Idle);
                Hero.IsMoving = false;
            }
            circle.transform.position = new Vector3(outerCircle.transform.position.x, outerCircle.transform.position.y, CanvasZ);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.A))
            {
                IsTouchMove = false;
                if (Hero.IsMoving && !Hero.IsAtking)
                    Hero.SetAnimation(HeroController.Actions.Idle);
                Hero.IsMoving = false;
            }
            circle.transform.position = new Vector3(outerCircle.transform.position.x, outerCircle.transform.position.y, CanvasZ);
        }
    }

    /// <summary>
    /// Lấy tọa độ khi chạm trên màn hình
    /// </summary>
    /// <param name="touchPosition"></param>
    /// <returns></returns>
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }

    /// <summary>
    /// Di chuyển nhân vật
    /// </summary>
    /// <param name="direction"></param>
    void moveCharacter(Vector2 direction)
    {
        if (!Hero.IsAtking)
            Player.Translate(new Vector2(direction.x, 0) * MoveSpeed * Time.deltaTime);
        if (Hero.CurrentAction == HeroController.Actions.Idle && !Hero.IsJumping && !Hero.IsSurfing && !Hero.IsMoving && !Hero.IsAtking)
        {
            Hero.SetAnimation(HeroController.Actions.Move);
            Hero.IsMoving = true;
        }
        //Camera.main.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Camera.main.transform.position.z);
    }
    #endregion
}