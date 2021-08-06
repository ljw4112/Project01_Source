using System.Collections.Generic;

public struct NonSelect
{
    public int nodeNum;
    public char direction;
    public NonSelect(int nodeNum, char direction)
    {
        this.nodeNum = nodeNum;
        this.direction = direction;
    }
}

public struct EndingInfo
{
    public bool isClear;
    public List<int> clearRoot;
    public List<NonSelect> nonSelect;
    public EndingInfo(bool isClear, List<int> clearRoot, List<NonSelect> nonSelect)
    {
        this.isClear = isClear;
        this.clearRoot = clearRoot;
        this.nonSelect = nonSelect;
    }
}

public struct CutsceneInfo
{
    public string cutsceneName;
    public bool isView;
    public CutsceneInfo(string cutsceneName, bool isView)
    {
        this.cutsceneName = cutsceneName;
        this.isView = isView;
    }
}
public class UserInfo
{
    public string id;
    public List<int> saveChapterList;
    public List<NonSelect> saveChapterNonSelectList;
    public Dictionary<string, EndingInfo> arrEnding;
    public Dictionary<string, CutsceneInfo> arrCutScene;
    public Dictionary<int, int> profileData;
    public int keyIndex;
    public int deadCount;

    public UserInfo()
    {
        this.id = null;
        this.saveChapterList = new List<int>();
        this.saveChapterNonSelectList = new List<NonSelect>();
        this.arrEnding = new Dictionary<string, EndingInfo>();
        this.arrCutScene = new Dictionary<string, CutsceneInfo>();
        this.profileData = new Dictionary<int, int>();
    }
}