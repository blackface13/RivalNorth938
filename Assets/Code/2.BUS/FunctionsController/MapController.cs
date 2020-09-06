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
    [Title("Độ rộng mỗi background")]
    public float BackgroundXSize;
    [Title("Tọa độ Y của background")]
    public float BackgroundPositionY;
    [Title("Background Scale")]
    public Vector3 BackgroundScale;
    [Title("List các object background")]
    public List<GameObject> BackgroundList;
    [Title("Sương mù của Map")]
    public GameObject ObjectFogBehind;
    [Title("Màu background main Camera")]
    public Color MainCameraBackgroundColor;
    private GameObject Player;
    #endregion

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        //CreateBackground();
    }

    /// <summary>
    /// Khởi tạo background cho map
    /// </summary>
    private void CreateBackground()
    {
        if (BackgroundList.Count > 0)
        {
            BackgroundList[0].transform.position = new Vector3(Player.transform.position.x - BackgroundXSize, BackgroundPositionY, BackgroundList[0].transform.position.z);
            for (int i = 0; i < BackgroundQuantity - 1; i++)
            {
                BackgroundList.Add(Instantiate(BackgroundList[0], new Vector3(BackgroundList[0].transform.position.x + ((i + 1) * BackgroundXSize), BackgroundPositionY, BackgroundList[0].transform.position.z), Quaternion.identity));
            }

            //Set parent và sửa lại tọa độ
            foreach(var item in BackgroundList)
            {
                item.transform.SetParent(BackgroundList[0].transform.parent, false);
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, BackgroundList[0].transform.position.z);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //ObjectFogBehind.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y+2f, ObjectFogBehind.transform.position.z);
    }
    private void OnEnable()
    {
        if (ObjectFogBehind != null)
        {
            ObjectFogBehind.SetActive(true);
            ObjectFogBehind.transform.SetParent(Camera.main.transform, false);//Sương mù
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera.main.backgroundColor = MainCameraBackgroundColor;
    }

    private void OnDisable()
    {
        if (ObjectFogBehind != null)
        {
            ObjectFogBehind.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //BackgroundController();
    }

    /// <summary>
    /// Điều khiển hoạt động của background
    /// </summary>
    private void BackgroundController()
    {
        if(BackgroundQuantity > 0)
        {
            foreach (var item in BackgroundList)
            {
                item.transform.SetParent(BackgroundList[0].transform.parent, false);
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, BackgroundList[0].transform.position.z);
            }
        }
    }
}
