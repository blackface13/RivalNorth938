using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    #region Variables
    [TabGroup("Cấu hình thuộc tính")]
    [Title("ID của Map sẽ move tới")]
    public int MoveMapID;
    [TabGroup("Cấu hình thuộc tính")]
    [Title("Tọa độ move tới")]
    public Vector2 MovePos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Va chạm xuyên qua
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter2D(Collider2D col)
    {
        //Va chạm với Player
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Hero))
        {
           StartCoroutine( GameSystems.LoadMap(MoveMapID, MovePos));
        }
    }
}
