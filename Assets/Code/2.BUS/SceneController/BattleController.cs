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
        GameSettings.BattleControl = this;
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
    }

    /// <summary>
    /// Hiển thị damage lên đối tượng trúng đòn
    /// </summary>
    public void ShowDmgText(Vector3 pos, string dmgValue)
    {
        var obj = GameSettings.ObjControl.GetObjectNonActive(DamageText);
        if (obj == null)
        {
            DamageText.Add(Instantiate(DamageText[0], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity));
            var tmp = DamageText[DamageText.Count - 1].GetComponent<DamageTextController>();
            tmp.DamageValue = dmgValue;
        }
        else
        {
            var tmp = DamageText[DamageText.Count - 1].GetComponent<DamageTextController>();
            tmp.DamageValue = dmgValue;
            GameSettings.ObjControl.ShowObject(obj, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        }
    }
    #endregion

    #region Functions


    /// <summary>
    /// Đẩy lùi đối phương
    /// </summary>
    /// <returns></returns>
    public IEnumerator RepelVictim(Rigidbody2D victimRigid2D, Vector3 skillPos, Vector3 victimPos, float forceVictim, float originalGravity)
    {
        victimRigid2D.velocity = Vector3.zero;
        victimRigid2D.gravityScale = 0;
        victimRigid2D.AddForce(new Vector2(skillPos.x < victimPos.x ? forceVictim : -forceVictim, 0) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.05f);
        victimRigid2D.gravityScale = originalGravity;
    }
    #endregion

    // Update is called once per frame
    //void Update()
    //{

    //}
}
