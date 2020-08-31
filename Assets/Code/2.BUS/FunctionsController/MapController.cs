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
    [Title("")]

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Điều khiển hoạt động của background
    /// </summary>
    private void BackgroundController()
    {

    }
}
