using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
// Json文件
    {
        "infoList":
        [
            {
                "panelTypeString":"Skill",
                "path":"Assets/UI/icon.prefab"
            },
            {
              "panelTypeString":"Task",
              "path":"Assets/UI/icon.prefab"
            }
        ]
    }
*/

//这里需要加载UI面板的类型
public enum UIPanelType
{
    MainMenu,
    Task,
    Shop,
    Bag,
    Skill,
    System,
    ItemMessage
}

// 进行反序列化和序列化
[Serializable]//一定要写，而且不能错
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]//一定要写，而且不能错
                   //UI的类型
    public UIPanelType uiPanelType;
    //这里要和你的Json里命名一样，顺序也要一样，例如："panelTypeString":"Skill"的panelTypeString
    public string panelTypeString;
    //UI的路径
    public string path;
    //反序列之前调用
    public void OnAfterDeserialize()
    {
        uiPanelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), panelTypeString);//将反序列化的字符串类型转化为对应的枚举
    }
    //序列化之前调用该方法
    public void OnBeforeSerialize()
    {
    }
}

[Serializable]
class UIPanelTypeJson
{
    public List<UIPanelInfo> infoList;//C#中集合和json的集合名字要一致，且需要标记上上序列化
}

public class UIManager
{
    //单例模式
    private static UIManager instance;
    public static UIManager GetInstacne
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
    //存储加载进来数据的字典
    private Dictionary<UIPanelType, string> pannelPathDict;
    //构造私有化
    private UIManager()
    {
        ParseUIPanelTypeJson();//加载Json文件等操作
    }
    void ParseUIPanelTypeJson()
    {

        if (pannelPathDict == null)
        {
            pannelPathDict = new Dictionary<UIPanelType, string>();
        }
        //加载json文件，json文件在Resources文件中
        TextAsset info = Resources.Load<TextAsset>("Json/UIPanelTypeJson");

        UIPanelTypeJson jsonObj = JsonUtility.FromJson<UIPanelTypeJson>(info.text);
        //将每个数据存储在字典中
        foreach (UIPanelInfo item in jsonObj.infoList)
        {
            pannelPathDict.Add(item.uiPanelType, item.path);
        }
    }
    //测试是否成功把数据加载到unity中
    public void Test()
    {
        string path;
        pannelPathDict.TryGetValue(UIPanelType.Skill, out path);
        Debug.Log(path);
    }

}