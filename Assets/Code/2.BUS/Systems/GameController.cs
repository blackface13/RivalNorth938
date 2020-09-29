using Assets.Code._4.CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void LoadMap(int mapID, Vector2 movePos)
    {
        StartCoroutine(GameSystems.LoadMap(mapID, movePos));
    }
}
