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


    [TabGroup("Misc")]
    #endregion

    #region Initialize
    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
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
    #endregion

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
