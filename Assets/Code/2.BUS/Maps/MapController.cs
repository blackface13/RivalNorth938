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
    #endregion

    public virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (BackgroundObject != null)
            BackgroundPositionOriginal = BackgroundObject.transform.localPosition;
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
            BackgroundObject.transform.localPosition = new Vector3(Camera.main.transform.position.x * ObjectMoveSpeed, BackgroundPositionOriginal.y, BackgroundPositionOriginal.z);
        }
    }
}
