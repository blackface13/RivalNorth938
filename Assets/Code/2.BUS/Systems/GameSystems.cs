using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystems : MonoBehaviour
{
    public static GameObject CurrentMap;
    public static Color ImgTranslateColor;
    public static Image ImgTranslate;
    public static GameController GameControl;

    // Start is called before the first frame update
    void Start()
    {

    }

    public static IEnumerator LoadMap(int mapID, Vector2 movePos)
    {
        ImgTranslate.gameObject.SetActive(true);
        GameSettings.IsAllowActions = false;

        //Giữ player nguyên vị trí Y
        try
        {
            GameSettings.PlayerController.SetAnimation(HeroController.Actions.Idle);
        }
        catch { }
        var playerRigid = GameSettings.Player.GetComponent<Rigidbody2D>();
        playerRigid.velocity = Vector3.zero;
        playerRigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        var duration = .1f;
        var startColor = new Color(0, 0, 0, 0);
        var endColor = new Color(0, 0, 0, 1);
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            ImgTranslateColor.a = Mathf.Lerp(startColor.a, endColor.a, time);
            ImgTranslate.color = ImgTranslateColor;
            yield return null;
        }

        //Clear map khỏi bộ nhớ
        RemoveMap();

        //Load map
        CurrentMap = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Maps/Map" + mapID.ToString()), new Vector3(0, 0, 20), Quaternion.identity);

        //Di chuyển nhân vật tới vị trí cần thiết
        GameSettings.Player.transform.position = new Vector3(movePos.x, movePos.y, GameSettings.Player.transform.position.z);

        //Trả lại Y để player có thể rơi xuống
        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;


        yield return new WaitForSeconds(.5f);

        //Camera.main.transform.position = new Vector3(GameSettings.Player.transform.position.x, GameSettings.Player.transform.position.y, Camera.main.transform.position.z);

        time = 0;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            ImgTranslateColor.a = Mathf.Lerp(endColor.a, startColor.a, time);
            ImgTranslate.color = ImgTranslateColor;
            yield return null;
        }
        GameSettings.PlayerController.IsAllowAtk = true;
        GameSettings.PlayerController.IsAllowSurf = true;
        GameSettings.PlayerController.IsJumping = false;
        GameSettings.PlayerController.IsSurfing = false;
        GameSettings.PlayerController.IsAtking = false;
        GameSettings.PlayerController.IsMoving = false;
        GameSettings.IsAllowActions = true;
    }

    public static void RemoveMap()
    {
        try
        {
            var mapControl = CurrentMap.GetComponent<MapController>();
            if (mapControl.ObjectFogBehind != null)
                MonoBehaviour.Destroy(mapControl.ObjectFogBehind);
        }
        catch { }

        MonoBehaviour.Destroy(CurrentMap);
        Resources.UnloadUnusedAssets();
    }
}
