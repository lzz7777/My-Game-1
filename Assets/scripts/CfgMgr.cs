using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class AnimCfg
{
    public string Id;
    public int SpriteCount;
    public string SpriteName;
    public string FolderPath;
    public int AnimatorRate;
}

[System.Serializable]
public class RoleCfg
{
    public string Id;
    public List<string> AnimationId;
    public int IsPlayer;
}

public class AnimCfgs
{
    public List<AnimCfg> AnimCfg;
}

public class RoleCfgs
{
    public List<RoleCfg> RoleCfg;
}

public enum CfgEnum
{
    RoleCfg,
    AnimCfg,
}

public class CfgMgr : MonoBehaviour
{
    private static GameObject _cfgObject;
    private static CfgMgr _cfgMgr = null;
    public static CfgMgr GetInstance()
    {
        if (_cfgMgr == null)
        {
            _cfgMgr = _cfgObject.GetComponent<CfgMgr>();
        }
        return _cfgMgr;
    }

    public Dictionary<string, AnimCfg> AnimCfgDic = new Dictionary<string, AnimCfg>();
    public Dictionary<string, RoleCfg> RoleCfgDic = new Dictionary<string, RoleCfg>();

    private void Awake()
    {
        InitData();

        foreach (AnimCfg item in JsonConvert.DeserializeObject<AnimCfgs>(ReadData("AnimCfg")).AnimCfg)
        {
            AnimCfgDic.Add(item.Id, item);
        }
        Dump(AnimCfgDic);

        foreach (RoleCfg item in JsonConvert.DeserializeObject<RoleCfgs>(ReadData("RoleCfg")).RoleCfg)
        {
            RoleCfgDic.Add(item.Id, item);
        }
        Dump(RoleCfgDic);
    }

    //读取文件
    public string ReadData(string fileName)
    {
        string readData;
        string fileUrl = string.Format("{0}\\{1}.json", Application.streamingAssetsPath, fileName);
        readData = File.ReadAllText(fileUrl);
        return readData;
    }

    private void InitData()
    {
        CfgMgr._cfgObject = gameObject;
    }

    public static void Dump(object obj)
    {
        Debug.Log(ConvertJsonString(JsonConvert.SerializeObject(obj)));
    }

    private static string ConvertJsonString(string str)
    {
        //格式化json字符串
        JsonSerializer serializer = new JsonSerializer();
        TextReader tr = new StringReader(str);
        JsonTextReader jtr = new JsonTextReader(tr);
        object obj = serializer.Deserialize(jtr);
        if (obj != null)
        {
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }
        else
        {
            return str;
        }
    }
}