using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
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
    public bool CheckExistAndCreateEffectExtension(Vector3 col, List<GameObject> gobject, Quaternion quater, bool isViewLeft)
    {
        var a = GetObjectNonActive(gobject);
        if (a == null)
        {
            gobject.Add(Instantiate(gobject[0], new Vector3(col.x, col.y, col.z), quater));
            gobject[gobject.Count - 1].GetComponent<SkillController>().IsViewLeft = isViewLeft;
            return true;
        }
        else
            ShowSkill(a, new Vector3(col.x, col.y, col.z), quater, isViewLeft);
        return false;
    }
}
