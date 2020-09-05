using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectController : MonoBehaviour
{
    [Title("Ẩn khi khởi tạo")]
    public bool IsShowOnCreate = false;

    private void Awake()
    {
        if (IsShowOnCreate)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
}
