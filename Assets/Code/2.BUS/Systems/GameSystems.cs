using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystems : MonoBehaviour
{
    public static GameObject CurrentMap;
    public static Color ImgTranslateColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    public static IEnumerator LoadMap(int mapID, Vector2 movePos)
    {
        GameSettings.IsAllowActions = false;

        //Giữ player nguyên vị trí Y
        var playerRigid = GameSettings.Player.GetComponent<Rigidbody2D>();
        playerRigid.velocity = Vector3.zero;
        playerRigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        //var duration = .5f;
        //var startColor = new Color(255, 255, 255, 0);
        //var endColor = new Color(255, 255, 255, 255);
        //float time = 0;
        //float rate = 1 / duration;
        //while (time < 1)
        //{
        //    time += rate * Time.deltaTime;
        //    Color.Lerp(startColor, endColor, Time.time);
        //    yield return null;
        //}

        //Clear map khỏi bộ nhớ
        RemoveMap();

        //Load map
        CurrentMap = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Maps/Map" + mapID.ToString()), new Vector3(0, 0, 20), Quaternion.identity);

        //Di chuyển nhân vật tới vị trí cần thiết
        GameSettings.Player.transform.position = new Vector3(movePos.x, movePos.y, GameSettings.Player.transform.position.z);

        //Trả lại Y để player có thể rơi xuống
        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameSettings.IsAllowActions = true;

        //time = 0;
        //while (time < 1)
        //{
        //    time += rate * Time.deltaTime;
        //    Color.Lerp(endColor, startColor, Time.time);
        //    yield return null;
        //}
        yield return null;
    }

    public static void RemoveMap()
    {
        MonoBehaviour.Destroy(CurrentMap);
        Resources.UnloadUnusedAssets();
    }

    public IEnumerator FadeIn(float duration)
    {
        var startColor = new Color32(255, 255, 255, 0);
        var endColor = new Color32(255, 255, 255, 255);
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            Color32.Lerp(startColor, endColor, Time.time);
            yield return null;
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        var startColor = new Color32(255, 255, 255, 255);
        var endColor =  new Color32(255, 255, 255, 0);
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            Color32.Lerp(startColor, endColor, Time.time);
            yield return null;
        }
    }
}
