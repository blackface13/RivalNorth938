using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    #region Skill Object Controller
    /// <summary>
    /// Khởi tạo các object skill
    /// </summary>
    /// <param name="listObj"></param>
    /// <param name="prefabName"></param>
    /// <param name="quantity"></param>
    /// <param name="quater"></param>
    /// <param name="isViewLeft"></param>
    public List<GameObject> CreateListSkillObject(string prefabName, int quantity, Quaternion quater, bool isViewLeft = false)
    {
        var listObj = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            listObj.Add((GameObject)Instantiate(Resources.Load<GameObject>(GameSettings.PathSkillObjects + prefabName), GameSettings.DefaultPositionObjectSkill, quater));
            listObj[i].GetComponent<SkillController>().IsViewLeft = isViewLeft;
            listObj[i].SetActive(false);
        }
        return listObj;
    }

    /// <summary>
    /// Gọi ở các object kế thừa, enable skill của hero
    /// </summary>
    /// <param name="obj">Skill object</param>
    /// <param name="vec">Tọa độ xuât hiện</param>
    /// <param name="quater">Độ nghiêng, xoay tròn</param>
    public void ShowSkill(GameObject obj, Vector3 vec, Quaternion quater, bool isViewLeft)
    {
        obj.GetComponent<SkillController>().IsViewLeft = isViewLeft;
        obj.transform.position = vec;
        obj.transform.rotation = quater;
        obj.SetActive(true);
    }

    /// <summary>
    /// Trả về object skill đang ko hoạt động để xuất hiện
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public GameObject GetObjectNonActive(List<GameObject> obj)
    {
        int count = obj.Count;
        for (int i = 0; i < count; i++)
        {
            if (!obj[i].activeSelf)
                return obj[i];
        }
        return null;
    }

    /// <summary>
    /// Check khởi tạo và hiển thị hiệu ứng trúng đòn lên đối phương
    /// </summary>
    /// <param name="col"></param>
    public bool CheckExistAndCreateEffectExtension(Vector3 col, List<GameObject> gobject, Quaternion quater, bool isViewLeft, bool isMoving)
    {
        var a = GetObjectNonActive(gobject);
        if (a == null)
        {
            gobject.Add(Instantiate(gobject[0], new Vector3(col.x, col.y, col.z), quater));
            var controller = gobject[gobject.Count - 1].GetComponent<SkillController>();
            controller.IsViewLeft = isViewLeft;
            controller.ForceToVictimBonus = isMoving ? Random.Range(10f, 15f) : 0;
            return true;
        }
        else
        {
            var controller = a.GetComponent<SkillController>();
            controller.IsViewLeft = isViewLeft;
            controller.ForceToVictimBonus = isMoving ? Random.Range(10f, 15f) : 0;
            ShowSkill(a, new Vector3(col.x, col.y, col.z), quater, isViewLeft);
        }
        return false;
    }
    #endregion

    #region Normal Object
    /// <summary>
    /// Khởi tạo danh sách GameObject ban đầu
    /// </summary>
    /// <param name="prefabName">Đường dẫn tới prefab</param>
    /// <param name="quantity">Số lượng object</param>
    /// <param name="pos">Tọa độ</param>
    /// <param name="quater">Quay</param>
    /// <returns></returns>
    public List<GameObject> CreateListObject(string prefab, int quantity, Vector3 pos, Quaternion quater, GameObject parent = null)
    {
        var listObj = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            listObj.Add((GameObject)Instantiate(Resources.Load<GameObject>(prefab), pos, quater));
            listObj[i].SetActive(false);
            if (parent != null)
                listObj[i].transform.SetParent(parent.transform, false);
        }
        return listObj;
    }

    /// <summary>
    /// Kiểm tra và clone game object nếu thiếu
    /// </summary>
    /// <param name="col"></param>
    /// <param name="gobject"></param>
    /// <param name="quater"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public bool CheckExistAndCreateObject(Vector3 col, List<GameObject> gobject, Quaternion quater, GameObject parent = null)
    {
        var a = GetObjectNonActive(gobject);
        if (a == null)
        {
            gobject.Add(Instantiate(gobject[0], new Vector3(col.x, col.y, col.z), quater));
            if (parent != null)
                gobject[gobject.Count - 1].transform.SetParent(parent.transform, false);
            return true;
        }
        else
            ShowObject(a, new Vector3(col.x, col.y, col.z), quater);
        return false;
    }

    /// <summary>
    /// Kiểm tra và clone game object nếu thiếu kèm theo 1 list để getcomponent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="col"></param>
    /// <param name="gobject"></param>
    /// <param name="quater"></param>
    /// <param name="input"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public bool CheckExistAndCreateObject<T>(Vector3 col, List<GameObject> gobject, Quaternion quater, List<T> input, GameObject parent = null)
    {
        var a = GetObjectNonActive(gobject);
        if (a == null)
        {
            gobject.Add(Instantiate(gobject[0], new Vector3(col.x, col.y, col.z), quater));
            input.Add(gobject[gobject.Count - 1].GetComponent<T>());
            if (parent != null)
                gobject[gobject.Count - 1].transform.SetParent(parent.transform, false);
            return true;
        }
        else
            ShowObject(a, new Vector3(col.x, col.y, col.z), quater);
        return false;
    }

    /// <summary>
    /// Hiển thị object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="vec"></param>
    /// <param name="quater"></param>
    public void ShowObject(GameObject obj, Vector3 vec, Quaternion quater)
    {
        obj.transform.position = vec;
        obj.transform.rotation = quater;
        obj.SetActive(true);
    }
    #endregion
}
