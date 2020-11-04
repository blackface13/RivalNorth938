using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string AnimString;
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = this.GetComponent<Animator>();
        Anim.SetTrigger(AnimString);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
