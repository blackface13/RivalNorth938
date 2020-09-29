using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2 : MapController
{
    [Title("Object nước phía trước")]
    public GameObject WaterFront;
    [Title("Object nước phía sau")]
    public GameObject WaterBehind;

    private Vector3 WaterFrontPositionOriginal;
    private Vector3 WaterBehindPositionOriginal;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        //if (WaterFront != null)
        //    WaterFrontPositionOriginal = WaterFront.transform.localPosition;
        //if (WaterBehind != null)
        //    WaterBehindPositionOriginal = WaterBehind.transform.localPosition;
    }

    public override void BackgroundController()
    {
        base.BackgroundController();
        //if (WaterFront != null)
        //{
        //    WaterFront.transform.localPosition = new Vector3(Camera.main.transform.position.x * ObjectMoveSpeed, WaterFrontPositionOriginal.y, WaterFrontPositionOriginal.z);
        //}
        //if (WaterBehind != null)
        //{
        //    WaterBehind.transform.localPosition = new Vector3(Camera.main.transform.position.x * ObjectMoveSpeed, WaterBehindPositionOriginal.y, WaterBehindPositionOriginal.z);
        //}
    }
}
