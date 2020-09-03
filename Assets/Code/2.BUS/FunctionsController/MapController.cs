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
    [Title("Độ dài mỗi background")]
    public Vector2 PositionBG2;//Tọa độ của hình background thứ 2, dựa vào đây sẽ tính toán tọa độ cho các bg khác, x = khoảng cách, y = tọa độ được set
    [Title("Background Scale")]
    public Vector3 BackgroundScale;
    [Title("List các object background")]
    public List<GameObject> BackgroundList;
    [Title("Player")]
    public GameObject Player;
    [Title("Sương mù của Map")]
    public GameObject ObjectFogBehind;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ObjectFogBehind.transform.SetParent(Camera.main.transform, false);//Sương mù
        //ObjectFogBehind.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y+2f, ObjectFogBehind.transform.position.z);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    BackgroundController();
    //}

    /// <summary>
    /// Điều khiển hoạt động của background
    /// </summary>
    private void BackgroundController()
    {

    }
}
