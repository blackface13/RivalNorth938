using Assets.Code._4.CORE;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    // Update is called once per frame
    //void Update()
    //{

    //}
    public GameObject CMVcam;

    private void Awake()
    {
        //Tính toán và set rigid body cho camera
        //var virtualCam = CMVcam.GetComponent<CinemachineVirtualCamera>();
        //var fieldOfView = virtualCam.m_Lens.FieldOfView;//Chỉ số này lấy ở fieldOfView của camera
        //var depth = -(((CinemachineFramingTransposer)virtualCam.GetCinemachineComponent(CinemachineCore.Stage.Body)).m_CameraDistance + fieldOfView + 5);//Độ sâu trường ảnh, khoảng cách camera chiếu tới map

        //float halfFieldOfView = Camera.main.fieldOfView * fieldOfView * Mathf.Deg2Rad;
        //float halfHeightAtDepth = (float)Math.Round(depth * Mathf.Tan(halfFieldOfView), 2);
        //float halfWidthAtDepth = Camera.main.aspect * halfHeightAtDepth;

        //this.GetComponent<BoxCollider2D>().size = new Vector2(halfWidthAtDepth, halfHeightAtDepth);
        this.GetComponent<BoxCollider2D>().size = new Vector2(NominalScreenWidthAt(Camera.main, GameSettings.Player.transform), NominalScreenHeightAt(Camera.main, GameSettings.Player.transform));
    }

    public float NominalScreenWidthAt(Camera c, Transform t)
    {
        float yFromCamera = t.transform.position.z - c.transform.position.z;

        return
            c.ViewportToWorldPoint(new Vector3(1f, 1f, yFromCamera)).x
            - c.ViewportToWorldPoint(new Vector3(0f, 1f, yFromCamera)).x;
    }

    public float NominalScreenHeightAt(Camera c, Transform t)
    {
        float yFromCamera = t.transform.position.z - c.transform.position.z;

        return
            c.ViewportToWorldPoint(new Vector3(0f, 1f, yFromCamera)).y
            - c.ViewportToWorldPoint(new Vector3(0f, 0f, yFromCamera)).y;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.MapObject) && col.gameObject.tag.Equals("Untagged"))
        {
            col.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.MapObject) && col.gameObject.tag.Equals("Untagged"))
        {
            col.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
