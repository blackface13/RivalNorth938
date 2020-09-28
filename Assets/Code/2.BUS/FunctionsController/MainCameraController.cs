using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    // Update is called once per frame
    //void Update()
    //{

    //}

    //private void Awake()
    //{
    //    this.GetComponent<BoxCollider2D>().size = Camera.main.orz
    //}
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
