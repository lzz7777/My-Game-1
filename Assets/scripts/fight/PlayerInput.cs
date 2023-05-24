using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public RoleMain _roleMain;

    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical") * speed;
        float h = Input.GetAxis("Horizontal") * speed;
        v *= Time.deltaTime;
        h *= Time.deltaTime;

        Vector2 offset = new Vector2(h, v);
        if (_roleMain)
        {
            _roleMain.transformMgr.DoMove(offset);
        }
    }
}
