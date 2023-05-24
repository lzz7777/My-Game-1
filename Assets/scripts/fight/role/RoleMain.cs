using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleMain : MonoBehaviour
{
    public TransformMgr transformMgr;
    public AnimatorMgr animatorMgr;

    public string RoleId;
    protected RoleCfg RoleCfg;

    private void Awake()
    {
        transformMgr = new TransformMgr(this);
        animatorMgr = new AnimatorMgr(this);

        CfgMgr.GetInstance().RoleCfgDic.TryGetValue(RoleId, out RoleCfg);
        if (RoleCfg.IsPlayer == 1)
        {

        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
