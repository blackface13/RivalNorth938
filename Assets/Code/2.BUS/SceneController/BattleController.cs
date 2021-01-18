using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    #region Variables
    [TabGroup("Cấu hình")]
    [Title("Nhân vật chính")]
    public GameObject Player;
    [TabGroup("Cấu hình")]
    [Title("Số lượng object damage text khởi tạo ban đầu")]
    public int NumberObjectDmgTextCreate;
    [TabGroup("Cấu hình")]
    [Title("Hình ảnh làm tối khi chuyển cảnh")]
    public Image ImgTranslate;
    [TabGroup("Cấu hình")]
    [Title("Object combo text")]
    public Text ComboText;


    [TabGroup("Misc")]
    public bool IsShowCombo;
    [TabGroup("Misc")]
    public int ComboCount;
    private List<GameObject> DamageText;
    #endregion

    #region Initialize
    private void Awake()
    {
        GameSystems.GameControl = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameController>();
        SetupPlayer();
        GameSystems.ImgTranslate = ImgTranslate;
        GameSystems.GameControl.LoadMap(3, new Vector2(0, 0));
        //StartCoroutine( GameSystems.LoadMap(2, new Vector2(0, 0)));
    }

    // Start is called before the first frame update
    void Start()
    {
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
            var tmp = obj.GetComponent<DamageTextController>();
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
    public IEnumerator RepelVictim(Rigidbody2D victimRigid2D, Vector3 skillPos, Vector3 victimPos, float forceVictim, bool repelLeft)
    {
        victimRigid2D.velocity = Vector3.zero;
        victimRigid2D.velocity += (repelLeft ? Vector2.right : Vector2.left) * forceVictim;
        //victimRigid2D.AddForce(new Vector2(skillPos.x < victimPos.x ? forceVictim : -forceVictim, 0) * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.05f);
    }

    /// <summary>
    /// Show combo khi đánh liên tục
    /// </summary>
    public void ShowCombo()
    {
        if (!IsShowCombo)
        {
            IsShowCombo = true;
            StartCoroutine(GameSettings.ObjControl.MoveObjectCurve(true, ComboText.gameObject, ComboText.gameObject.transform.localPosition, new Vector2(-885.61f, ComboText.gameObject.transform.localPosition.y), .5f, GameSystems.GameControl.MoveAnim));
        }
        else
            ComboText.text = string.Format("Combo {0}", ComboCount);
    }
    #endregion

    // Update is called once per frame
    //void Update()
    //{

    //}
}
