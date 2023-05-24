using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreateRoleMgr : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // 生成主角
        Dictionary<string, RoleCfg> cfg = CfgMgr.GetInstance().RoleCfgDic;
        CfgMgr.Dump(cfg);
        foreach (var item in cfg)
        {
            if (item.Value.IsPlayer == 1)
            {
                CreateRole(item.Value.Id);
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateRole(string rid)
    {
        Debug.Log("CreateRole: " + rid);

        GameObject prefab = Resources.Load<GameObject>("Prefabs/role_unit");
        GameObject roleUnit = Instantiate(prefab) as GameObject;
        RoleMain roleMain = roleUnit.GetComponent<RoleMain>();
        roleMain.RoleId = rid;
    }
}
