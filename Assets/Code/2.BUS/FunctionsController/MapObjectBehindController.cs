using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectBehindController : MonoBehaviour
{
    [Title("Object phía bên trái hoặc phải")]
    public bool IsLeft = true;
    [Title("Object cần điều khiểu")]
    public GameObject MainObject;
    [Title("Ẩn khi khởi tạo")]
    public bool IsShowOnCreate = false;

    private void Awake()
    {
        if (IsShowOnCreate)
            MainObject.SetActive(true);
        else MainObject.SetActive(false);
    }

    /// <summary>
    /// Va chạm với player
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.MainCamera))
        {
            if (IsLeft && col.gameObject.transform.position.x < gameObject.transform.position.x)
                MainObject.SetActive(true);
            if (!IsLeft && col.gameObject.transform.position.x > gameObject.transform.position.x)
                MainObject.SetActive(true);
        }
    }
    /// <summary>
    /// Ra khỏi vùng va chạm
    /// </summary>
    /// <param name="col"></param>
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.MainCamera))
        {
            if (IsLeft && col.gameObject.transform.position.x < gameObject.transform.position.x)
                MainObject.SetActive(false);
            if (!IsLeft && col.gameObject.transform.position.x > gameObject.transform.position.x)
                MainObject.SetActive(false);
        }
    }
}
