using Assets.Code._2.BUS.NPCs;
using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    #region Variables
    [Required]
    [TabGroup("Cấu hình")]
    [Title("Object button thao tác với NPC")]
    public GameObject BtnActionNPC;
    [Required]
    [TabGroup("Cấu hình")]
    [Title("Object hiển thị message")]
    public GameObject MessageContentObject;
    [Required]
    [TabGroup("Cấu hình")]
    [Title("Các obj UI cần ẩn để show message")]
    public GameObject ObjectUIHideForShowMessage;
    [TabGroup("Cấu hình")]
    [Title("Biểu đồ move")]
    public AnimationCurve MoveAnim;


    [TabGroup("Misc")]
    public NPCBase CurrentNPC;//NPC hiện tại đang 
    #endregion

    #region Functions

    public void Awake()
    {
        GameSystems.InitializeLanguage();
    }

    /// <summary>
    /// Load map
    /// </summary>
    /// <param name="mapID"></param>
    /// <param name="movePos"></param>
    public void LoadMap(int mapID, Vector2 movePos)
    {
        StartCoroutine(GameSystems.LoadMap(mapID, movePos));
    } 

    /// <summary>
    /// 
    /// </summary>
    public void EndMessageNPC()
    {
        ObjectUIHideForShowMessage.SetActive(true);
        MessageContentObject.SetActive(false);
    }

    /// <summary>
    /// Thao tác với NPC
    /// </summary>
    /// <param name="eventData"></param>
    public void ActionNPCEvent(BaseEventData eventData)
    {
        ObjectUIHideForShowMessage.SetActive(false);
        MessageContentObject.SetActive(true);
    }
    #endregion
}
