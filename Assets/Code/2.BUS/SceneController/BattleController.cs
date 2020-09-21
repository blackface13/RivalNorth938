using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    #region Variables
    [TabGroup("Cấu hình")]
    [Title("Nhân vật chính")]
    public GameObject Player;
    [TabGroup("Cấu hình")]
    [Title("Số lượng object damage text khởi tạo ban đầu")]
    public int NumberObjectDmgTextCreate;


    [TabGroup("Misc")]

    private List<GameObject> DamageText;
    private List<DamageTextController> DamageTextControl;
    #endregion

    #region Initialize
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
        CreateDmgText();
    }

    /// <summary>
    /// Khởi tạo player
    /// </summary>
    private void SetupPlayer()
    {
        if (Player != null)
        {
            GameSettings.Player = Player;
            GameSettings.PlayerController = Player.GetComponent<HeroController>();
        }
    }

    /// <summary>
    /// Khởi tạo các object dmg text
    /// </summary>
    private void CreateDmgText()
    {
        //DamageText = new List<GameObject>();
        DamageText = GameSettings.ObjControl.CreateListObject("Prefabs/UI/DamageText", NumberObjectDmgTextCreate, GameSettings.DefaultPositionObjectSkill, Quaternion.identity);
        DamageTextControl = new List<DamageTextController>();
        for (int i = 0; i < NumberObjectDmgTextCreate; i++)
        {
            DamageTextControl.Add(DamageText[i].GetComponent<DamageTextController>());
        }
    }

    /// <summary>
    /// Hiển thị damage lên đối tượng trúng đòn
    /// </summary>
    public void ShowDmgText(Vector3 pos, string dmgText)
    {
        GameSettings.ObjControl.CheckExistAndCreateObject<DamageTextController>(pos, DamageText, Quaternion.identity, DamageTextControl);
    }
    #endregion

    // Update is called once per frame
    //void Update()
    //{

    //}
}
