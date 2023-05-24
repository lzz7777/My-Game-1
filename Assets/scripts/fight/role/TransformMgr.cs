using UnityEngine;
using System.Collections;

public class TransformMgr
{

    private RoleMain _roleMain;

    public TransformMgr(RoleMain roleMain)
    {
        _roleMain = roleMain;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoMove(Vector2 offset)
    {
        if (offset.x < 0)
        {
            _roleMain.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(offset.x > 0)
        {
            _roleMain.transform.localScale = Vector3.one;
        }

        _roleMain.transform.Translate(offset);
    }
}
