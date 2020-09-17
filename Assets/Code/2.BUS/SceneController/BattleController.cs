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
        DamageText = new List<GameObject>();
        DamageTextControl = new List<DamageTextController>();
        for (int i = 0; i < NumberObjectDmgTextCreate; i++)
        {
            DamageText.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/UI/DamageText"), new Vector3(-1000, -1000, 0), Quaternion.identity));
            DamageTextControl.Add(DamageText[i].GetComponent<DamageTextController>());
            DamageText[i].SetActive(false);
        }
    }
    #endregion

    // Update is called once per frame
    //void Update()
    //{

    //}
}
