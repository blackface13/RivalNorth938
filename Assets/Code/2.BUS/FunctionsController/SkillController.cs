using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public bool IsViewLeft;

    public bool IsAutoHide = true;
    public float DelayTimeHide;
    private float OriginalScaleX;//Scale ban đầu của object

    private void Awake()
    {
        OriginalScaleX = this.transform.localScale.x;
    }

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnEnable()
    {
        if (IsViewLeft)
            this.transform.localScale = new Vector3(-OriginalScaleX, this.transform.localScale.y, this.transform.localScale.z);
        else
            this.transform.localScale = new Vector3(OriginalScaleX, this.transform.localScale.y, this.transform.localScale.z);

        if (IsAutoHide)
            StartCoroutine(AutoHide(DelayTimeHide));
    }


    /// <summary>
    /// Tự động ẩn Object sau 1 khoảng time
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator AutoHide(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
