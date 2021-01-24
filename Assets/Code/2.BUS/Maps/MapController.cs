using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    #region Variables
    [Title("ID của Map")]
    public int MapID;
    [Title("Số lượng Background tối thiểu được fill")]
    public int BackgroundQuantity;
    [Title("Sương mù của Map")]
    public GameObject ObjectFogBehind;
    [Title("Màu background main Camera")]
    public Color MainCameraBackgroundColor;
    [Title("Object background")]
    public GameObject BackgroundObject;
    private Vector3 BackgroundPositionOriginal;
    public GameObject Player;
    public float ObjectMoveSpeed = -0.005f;
    public float ObjectMoveSpeedY;
    #endregion

    public virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (BackgroundObject != null)
            BackgroundPositionOriginal = BackgroundObject.transform.localPosition;

        //Clone background image
        if (BackgroundQuantity > 1 && BackgroundObject.transform.childCount > 0)
        {
            var imgBG = BackgroundObject.transform.GetChild(0).gameObject;//Get first child
            var size = imgBG.GetComponent<SpriteRenderer>().bounds.size;//Get size image

            //Clone image
            for (int i = 0; i < BackgroundQuantity - 1; i++)
            {
                Instantiate(imgBG, BackgroundObject.transform);
            }

            //Tạo và gán tọa độ của image đầu tiên
            var firstImgPosX = (BackgroundQuantity % 2).Equals(0) ? -(BackgroundQuantity / 2 * size.x - size.x / 2) : -(BackgroundQuantity / 2 * size.x);
            imgBG.transform.localPosition = new Vector3(firstImgPosX, imgBG.transform.localPosition.y, 0);
            print(firstImgPosX);
            //Các img tiếp theo set theo X của image đầu tiên
            for (int i = 0; i < BackgroundQuantity; i++)
            {
                BackgroundObject.transform.GetChild(i).transform.localPosition = new Vector3((firstImgPosX + size.x * i), imgBG.transform.localPosition.y, 0);
            }
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        //ObjectFogBehind.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y+2f, ObjectFogBehind.transform.position.z);
    }
    public virtual void OnEnable()
    {
        if (ObjectFogBehind != null)
        {
            ObjectFogBehind.SetActive(true);
            ObjectFogBehind.transform.SetParent(Camera.main.transform, false);//Sương mù
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera.main.backgroundColor = MainCameraBackgroundColor;
    }

    public virtual void OnDisable()
    {
        if (ObjectFogBehind != null)
        {
            ObjectFogBehind.SetActive(false);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //BackgroundController();
        BackgroundController();
    }

    /// <summary>
    /// Điều khiển hoạt động của background
    /// </summary>
    public virtual void BackgroundController()
    {
        if (BackgroundObject != null)
        {
            BackgroundObject.transform.localPosition = new Vector3(Camera.main.transform.position.x * ObjectMoveSpeed, ObjectMoveSpeedY != 0 ? Camera.main.transform.position.y * ObjectMoveSpeedY : BackgroundPositionOriginal.y, BackgroundPositionOriginal.z);
        }
    }
}
