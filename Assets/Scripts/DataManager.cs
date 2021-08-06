using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public enum eStatus
{
    NULL, DEAD, CLEAR
}

public class DataManager
{
    public List<Tree> arrTrees;

    //각 챕터의 스크립트 데이터를 담고 있음
    //프롤로그
    public Dictionary<int, Prologue> arrChapter001;
    public Dictionary<int, Prologue> arrChapter002;
    public Dictionary<int, Prologue> arrChapter003;
    public Dictionary<int, Prologue> arrChapter004;
    public Dictionary<int, Prologue> arrChapter005;
    public Dictionary<int, Prologue> arrChapter006;
    public Dictionary<int, Prologue> arrChapter007;
    public Dictionary<int, Prologue> arrChapter008;
    public Dictionary<int, Prologue> arrChapter009;
    public Dictionary<int, Prologue> arrChapter010;
    public Dictionary<int, Prologue> arrChapter011;
    public Dictionary<int, Prologue> arrChapter012;
    public List<Dictionary<int, Prologue>> arrChapter0;

    //챕터 1
    public Dictionary<int, Prologue> arrChapter100;
    public Dictionary<int, Prologue> arrChapter101;
    public Dictionary<int, Prologue> arrChapter102;
    public Dictionary<int, Prologue> arrChapter103;
    public Dictionary<int, Prologue> arrChapter104;
    public Dictionary<int, Prologue> arrChapter105;
    public Dictionary<int, Prologue> arrChapter106;
    public Dictionary<int, Prologue> arrChapter107;
    public Dictionary<int, Prologue> arrChapter108;
    public Dictionary<int, Prologue> arrChapter109;
    public Dictionary<int, Prologue> arrChapter110;
    public Dictionary<int, Prologue> arrChapter111;
    public Dictionary<int, Prologue> arrChapter112;
    public Dictionary<int, Prologue> arrChapter113;
    public Dictionary<int, Prologue> arrChapter114;
    public Dictionary<int, Prologue> arrChapter115;
    public Dictionary<int, Prologue> arrChapter116;
    public Dictionary<int, Prologue> arrChapter117;
    public Dictionary<int, Prologue> arrChapter118;
    public Dictionary<int, Prologue> arrChapter119;
    public Dictionary<int, Prologue> arrChapter120;
    public Dictionary<int, Prologue> arrChapter121;
    public Dictionary<int, Prologue> arrChapter122;
    public Dictionary<int, Prologue> arrChapter123;
    public Dictionary<int, Prologue> arrChapter124;
    public Dictionary<int, Prologue> arrChapter125;
    public Dictionary<int, Prologue> arrChapter126;
    public Dictionary<int, Prologue> arrChapter127;
    public Dictionary<int, Prologue> arrChapter128;
    public Dictionary<int, Prologue> arrChapter129;
    public Dictionary<int, Prologue> arrChapter130;
    public List<Dictionary<int, Prologue>> arrChapter1;

    //챕터 2
    public Dictionary<int, Prologue> arrChapter200;
    public Dictionary<int, Prologue> arrChapter201;
    public Dictionary<int, Prologue> arrChapter202;
    public Dictionary<int, Prologue> arrChapter203;
    public Dictionary<int, Prologue> arrChapter204;
    public Dictionary<int, Prologue> arrChapter205;
    public Dictionary<int, Prologue> arrChapter206;
    public Dictionary<int, Prologue> arrChapter207;
    public Dictionary<int, Prologue> arrChapter208;
    public Dictionary<int, Prologue> arrChapter209;
    public Dictionary<int, Prologue> arrChapter210;
    public Dictionary<int, Prologue> arrChapter211;
    public Dictionary<int, Prologue> arrChapter212;
    public Dictionary<int, Prologue> arrChapter213;
    public List<Dictionary<int, Prologue>> arrChapter2;

    //캐릭터 데이터
    public Dictionary<int, CharData> arrCharData;
    //캐릭터 썸네일 데이터
    public Dictionary<int, CharacterThumbnail> arrCharacterThumbnail;
    //컷씬 데이터
    public Dictionary<int, CutScene> arrCutScene;
    //엔딩 데이터
    public Dictionary<int, EndingData> arrEndingData;

    //각 챕터를 가리키는 트리
    public Tree prologueTree;
    public Tree chapter1Tree;
    public Tree chapter2Tree;
    public Tree chapter3Tree;
    public Tree finaleTree;

    //모든 엔딩번호를 저장하고 있는 리스트
    public List<int> endingNum;

    //캐릭터의 인덱스를 저장하고 있는 리스트
    public List<int> charNum;

    //유저의 결제정보를 담고있는 객체
    private PurchaseData userPurchase;
    public PurchaseData Purchase
    {
        set
        {
            this.userPurchase = value;
        }
        get
        {
            return this.userPurchase;
        }
    }

    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }
    private UserInfo user;

    private DataManager()
    {
        this.arrTrees = new List<Tree>();
        this.arrChapter0 = new List<Dictionary<int, Prologue>>();
        this.arrChapter1 = new List<Dictionary<int, Prologue>>();
        this.arrChapter2 = new List<Dictionary<int, Prologue>>();
        this.arrEndingData = new Dictionary<int, EndingData>();
        this.arrCharData = new Dictionary<int, CharData>();
        this.arrCharacterThumbnail = new Dictionary<int, CharacterThumbnail>();

        //캐릭터의 정보를 저장 (클라이언트는 미리 다 알고 있어야 되므로)
        //0. 이인, 1: 보라, 2: 권찬훈, 3: 한유성, 4: 김서율
        this.charNum = new List<int>();
        for (int i = 1; i <= 6; i++)
        {
            this.charNum.Add(i);
        }

        this.arrTrees.Add(this.prologueTree);
        this.arrTrees.Add(this.chapter1Tree);
        this.arrTrees.Add(this.chapter2Tree);
        this.arrTrees.Add(this.chapter3Tree);
        this.arrTrees.Add(this.finaleTree);
    }

    public void Init()
    {
        //초기화
        this.endingNum = new List<int>();

        //엔딩노드 번호 삽입
        this.endingNum.Add(12);
        this.endingNum.Add(205);
        this.endingNum.Add(206);
        this.endingNum.Add(211);
        this.endingNum.Add(212);

        //리스트에 챕터 삽입
        //프롤로그
        this.arrChapter0.Add(this.arrChapter001);
        this.arrChapter0.Add(this.arrChapter002);
        this.arrChapter0.Add(this.arrChapter003);
        this.arrChapter0.Add(this.arrChapter004);
        this.arrChapter0.Add(this.arrChapter005);
        this.arrChapter0.Add(this.arrChapter006);
        this.arrChapter0.Add(this.arrChapter007);
        this.arrChapter0.Add(this.arrChapter008);
        this.arrChapter0.Add(this.arrChapter009);
        this.arrChapter0.Add(this.arrChapter010);
        this.arrChapter0.Add(this.arrChapter011);
        this.arrChapter0.Add(this.arrChapter012);

        //챕터1
        this.arrChapter1.Add(this.arrChapter100);
        this.arrChapter1.Add(this.arrChapter101);
        this.arrChapter1.Add(this.arrChapter102);
        this.arrChapter1.Add(this.arrChapter103);
        this.arrChapter1.Add(this.arrChapter104);
        this.arrChapter1.Add(this.arrChapter105);
        this.arrChapter1.Add(this.arrChapter106);
        this.arrChapter1.Add(this.arrChapter107);
        this.arrChapter1.Add(this.arrChapter108);
        this.arrChapter1.Add(this.arrChapter109);
        this.arrChapter1.Add(this.arrChapter110);
        this.arrChapter1.Add(this.arrChapter111);
        this.arrChapter1.Add(this.arrChapter112);
        this.arrChapter1.Add(this.arrChapter113);
        this.arrChapter1.Add(this.arrChapter114);
        this.arrChapter1.Add(this.arrChapter115);
        this.arrChapter1.Add(this.arrChapter116);
        this.arrChapter1.Add(this.arrChapter117);
        this.arrChapter1.Add(this.arrChapter118);
        this.arrChapter1.Add(this.arrChapter119);
        this.arrChapter1.Add(this.arrChapter120);
        this.arrChapter1.Add(this.arrChapter121);
        this.arrChapter1.Add(this.arrChapter122);
        this.arrChapter1.Add(this.arrChapter123);
        this.arrChapter1.Add(this.arrChapter124);
        this.arrChapter1.Add(this.arrChapter125);
        this.arrChapter1.Add(this.arrChapter126);
        this.arrChapter1.Add(this.arrChapter127);
        this.arrChapter1.Add(this.arrChapter128);
        this.arrChapter1.Add(this.arrChapter129);
        this.arrChapter1.Add(this.arrChapter130);

        //챕터2
        this.arrChapter2.Add(this.arrChapter200);
        this.arrChapter2.Add(this.arrChapter201);
        this.arrChapter2.Add(this.arrChapter202);
        this.arrChapter2.Add(this.arrChapter203);
        this.arrChapter2.Add(this.arrChapter204);
        this.arrChapter2.Add(this.arrChapter205);
        this.arrChapter2.Add(this.arrChapter206);
        this.arrChapter2.Add(this.arrChapter207);
        this.arrChapter2.Add(this.arrChapter208);
        this.arrChapter2.Add(this.arrChapter209);
        this.arrChapter2.Add(this.arrChapter210);
        this.arrChapter2.Add(this.arrChapter211);
        this.arrChapter2.Add(this.arrChapter212);
        this.arrChapter2.Add(this.arrChapter213);

        //엔딩 데이터
    }

    public void GetData()       //초기 데이터 로딩
    {
        this.Init();

        //프롤로그 트리
        this.prologueTree = new Tree(this.arrChapter0[0], 1);
        this.prologueTree.AddChild('L', this.arrChapter0[0], this.arrChapter0[1], 2);
        this.prologueTree.AddChild('R', this.arrChapter0[0], this.arrChapter0[2], 3);
        this.prologueTree.AddChild('L', this.arrChapter0[1], this.arrChapter0[3], 4);
        this.prologueTree.AddChild('R', this.arrChapter0[1], this.arrChapter0[3], 4);
        this.prologueTree.AddChild('L', this.arrChapter0[2], this.arrChapter0[4], 5);
        this.prologueTree.AddChild('R', this.arrChapter0[2], this.arrChapter0[4], 5);
        this.prologueTree.AddChild('L', this.arrChapter0[3], this.arrChapter0[5], 6);
        this.prologueTree.AddChild('R', this.arrChapter0[3], this.arrChapter0[5], 6);
        this.prologueTree.AddChild('L', this.arrChapter0[4], this.arrChapter0[6], 7);
        this.prologueTree.AddChild('R', this.arrChapter0[4], this.arrChapter0[6], 7);
        this.prologueTree.AddChild('L', this.arrChapter0[5], this.arrChapter0[7], 8);
        this.prologueTree.AddChild('R', this.arrChapter0[5], this.arrChapter0[8], 9);
        this.prologueTree.AddChild('L', this.arrChapter0[6], this.arrChapter0[9], 10);
        this.prologueTree.AddChild('R', this.arrChapter0[6], this.arrChapter0[10], 11);
        this.prologueTree.AddChild('L', this.arrChapter0[7], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('R', this.arrChapter0[7], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('L', this.arrChapter0[8], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('R', this.arrChapter0[8], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('L', this.arrChapter0[9], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('R', this.arrChapter0[9], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('L', this.arrChapter0[10], this.arrChapter0[11], 12);
        this.prologueTree.AddChild('R', this.arrChapter0[10], this.arrChapter0[11], 12);

        //챕터1 트리==================================================================================
        this.chapter1Tree = new Tree(this.arrChapter1[0], 100);
        
        //100번 왼쪽 서브트리
        this.chapter1Tree.AddChild('L', this.arrChapter1[0], this.arrChapter1[1], 101);
        this.chapter1Tree.AddChild('L', this.arrChapter1[1], this.arrChapter1[2], 102);
        this.chapter1Tree.AddChild('R', this.arrChapter1[1], this.arrChapter1[5], 105);
        this.chapter1Tree.AddChild('L', this.arrChapter1[2], this.arrChapter1[3], 103);
        this.chapter1Tree.AddChild('R', this.arrChapter1[2], this.arrChapter1[4], 104);

        //105번이 루트인 서브트리
        this.chapter1Tree.AddChild('L', this.arrChapter1[5], this.arrChapter1[6], 106);
        this.chapter1Tree.AddChild('R', this.arrChapter1[5], this.arrChapter1[7], 107);
        this.chapter1Tree.AddChild('L', this.arrChapter1[6], this.arrChapter1[9], 109);
        this.chapter1Tree.AddChild('L', this.arrChapter1[7], this.arrChapter1[8], 108);
        this.chapter1Tree.AddChild('R', this.arrChapter1[7], this.arrChapter1[8], 108);
        this.chapter1Tree.AddChild('L', this.arrChapter1[8], this.arrChapter1[6], 106);
        this.chapter1Tree.AddChild('R', this.arrChapter1[8], this.arrChapter1[7], 107);
        this.chapter1Tree.AddChild('R', this.arrChapter1[6], this.arrChapter1[10], 110);
        this.chapter1Tree.AddChild('L', this.arrChapter1[10], this.arrChapter1[11], 111);
        this.chapter1Tree.AddChild('R', this.arrChapter1[10], this.arrChapter1[11], 111);
        this.chapter1Tree.AddChild('L', this.arrChapter1[11], this.arrChapter1[9], 109);
        this.chapter1Tree.AddChild('R', this.arrChapter1[11], this.arrChapter1[10], 110);

        //109번이 루트인 서브트리
        this.chapter1Tree.AddChild('L', this.arrChapter1[9], this.arrChapter1[12], 112);
        this.chapter1Tree.AddChild('R', this.arrChapter1[9], this.arrChapter1[13], 113);
        this.chapter1Tree.AddChild('L', this.arrChapter1[13], this.arrChapter1[14], 114);
        this.chapter1Tree.AddChild('R', this.arrChapter1[13], this.arrChapter1[14], 114);
        this.chapter1Tree.AddChild('L', this.arrChapter1[14], this.arrChapter1[12], 112);
        this.chapter1Tree.AddChild('R', this.arrChapter1[14], this.arrChapter1[13], 113);

        //112번이 루트인 서브트리
        this.chapter1Tree.AddChild('L', this.arrChapter1[12], this.arrChapter1[15], 115);
        this.chapter1Tree.AddChild('R', this.arrChapter1[12], this.arrChapter1[16], 116);
        this.chapter1Tree.AddChild('L', this.arrChapter1[16], this.arrChapter1[17], 117);
        this.chapter1Tree.AddChild('R', this.arrChapter1[16], this.arrChapter1[17], 117);
        this.chapter1Tree.AddChild('L', this.arrChapter1[17], this.arrChapter1[15], 115);
        this.chapter1Tree.AddChild('R', this.arrChapter1[17], this.arrChapter1[16], 116);

        //100번 오른쪽 서브트리
        this.chapter1Tree.AddChild('R', this.arrChapter1[0], this.arrChapter1[18], 118);
        this.chapter1Tree.AddChild('L', this.arrChapter1[18], this.arrChapter1[6], 106);
        this.chapter1Tree.AddChild('R', this.arrChapter1[18], this.arrChapter1[20], 120);
        this.chapter1Tree.AddChild('L', this.arrChapter1[20], this.arrChapter1[21], 121);
        this.chapter1Tree.AddChild('R', this.arrChapter1[20], this.arrChapter1[21], 121);
        this.chapter1Tree.AddChild('L', this.arrChapter1[21], this.arrChapter1[6], 106);
        this.chapter1Tree.AddChild('L', this.arrChapter1[21], this.arrChapter1[20], 120);

        //챕터2 트리==================================================================================
        this.chapter2Tree = new Tree(this.arrChapter2[0], 200);

        //200번 왼쪽 서브트리
        this.chapter2Tree.AddChild('L', this.arrChapter2[0], this.arrChapter2[1], 201);
        this.chapter2Tree.AddChild('L', this.arrChapter2[1], this.arrChapter2[3], 203);
        this.chapter2Tree.AddChild('R', this.arrChapter2[1], this.arrChapter2[2], 202);
        this.chapter2Tree.AddChild('L', this.arrChapter2[3], this.arrChapter2[4], 204);
        this.chapter2Tree.AddChild('R', this.arrChapter2[3], this.arrChapter2[4], 204);
        this.chapter2Tree.AddChild('L', this.arrChapter2[2], this.arrChapter2[4], 204);
        this.chapter2Tree.AddChild('R', this.arrChapter2[2], this.arrChapter2[4], 204);
        this.chapter2Tree.AddChild('L', this.arrChapter2[4], this.arrChapter2[5], 205);
        this.chapter2Tree.AddChild('R', this.arrChapter2[4], this.arrChapter2[6], 206);
        this.chapter2Tree.AddChild('L', this.arrChapter2[6], this.arrChapter2[13], 213);
        this.chapter2Tree.AddChild('R', this.arrChapter2[6], this.arrChapter2[13], 213);

        //200번 오른쪽 서브트리
        this.chapter2Tree.AddChild('R', this.arrChapter2[0], this.arrChapter2[7], 207);
        this.chapter2Tree.AddChild('L', this.arrChapter2[7], this.arrChapter2[9], 209);
        this.chapter2Tree.AddChild('R', this.arrChapter2[7], this.arrChapter2[8], 208);
        this.chapter2Tree.AddChild('L', this.arrChapter2[9], this.arrChapter2[10], 210);
        this.chapter2Tree.AddChild('R', this.arrChapter2[9], this.arrChapter2[10], 210);
        this.chapter2Tree.AddChild('L', this.arrChapter2[8], this.arrChapter2[10], 210);
        this.chapter2Tree.AddChild('R', this.arrChapter2[8], this.arrChapter2[10], 210);
        this.chapter2Tree.AddChild('L', this.arrChapter2[10], this.arrChapter2[11], 211);
        this.chapter2Tree.AddChild('R', this.arrChapter2[10], this.arrChapter2[12], 212);
        this.chapter2Tree.AddChild('L', this.arrChapter2[12], this.arrChapter2[13], 213);
        this.chapter2Tree.AddChild('R', this.arrChapter2[12], this.arrChapter2[13], 213);
    }

    public void SaveData(string id, List<int> node, List<NonSelect> nonSelectList, int keyIndex, int deadCount, Action callback)      //저장하기
    {
        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);

        if (!File.Exists(path))
        {
            UserInfo newUser = new UserInfo();
            newUser.saveChapterList = node;
            newUser.saveChapterNonSelectList = nonSelectList;
            newUser.keyIndex = keyIndex;
            newUser.deadCount = deadCount;
            this.user = newUser;
        }
        else
        {
            var tmp = File.ReadAllText(path);
            this.user = JsonConvert.DeserializeObject<UserInfo>(tmp);
            this.user.saveChapterList = node;
            this.user.saveChapterNonSelectList = nonSelectList;
            this.user.keyIndex = keyIndex;
            this.user.deadCount = deadCount;
        }
        this.user.id = id;
        var json = JsonConvert.SerializeObject(this.user);
        File.WriteAllText(path, json);
        callback();
    }

    //챕터 종료 후 엔딩 저장
    public void SaveEndingData(List<int> nodes, List<NonSelect> nonSelectList, int deadCount)
    {
        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
        this.user = this.GetUser();
        if (this.user == null)
        {
            if (File.Exists(path))
            {
                var tmp = File.ReadAllText(path);
                this.user = JsonConvert.DeserializeObject<UserInfo>(tmp);
            }
            else
            {
                this.user = new UserInfo();
            }
        }

        var index = "e" + nodes[nodes.Count - 1];
        var endingData = new EndingInfo();
        endingData.isClear = true;
        endingData.nonSelect = nonSelectList;
        endingData.clearRoot = nodes;
        this.user.deadCount = deadCount;
        if (this.user.arrEnding.ContainsKey(index))
        {
            //이미 해당엔딩에 관한 내용이 저장되어 있으면 덮어쓰기.
            this.user.arrEnding[index] = endingData;
        }
        else
        {
            //없으면 추가하기
            this.user.arrEnding.Add(index, endingData);
        }

        this.user = this.SaveNeighborProfile(this.user, nodes[nodes.Count - 1]);

        var json = JsonConvert.SerializeObject(this.user);
        File.WriteAllText(path, json);
    }

    public void SaveCutScene(string cutsceneName)
    {
        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
        this.user = this.GetUser();

        if (this.user == null)
        {
            if (File.Exists(path))
            {
                var tmp = File.ReadAllText(path);
                this.user = JsonConvert.DeserializeObject<UserInfo>(tmp);
            }
            else
            {
                this.user = new UserInfo();
            }
        }

        //cutsceneName이란 이름을 처음 조우했다면
        if (!this.user.arrCutScene.ContainsKey(cutsceneName))
        {
            this.user.arrCutScene.Add(cutsceneName, new CutsceneInfo(cutsceneName, true));
        }

        var json = JsonConvert.SerializeObject(this.user);
        File.WriteAllText(path, json);
    }

    public UserInfo LoadData()      //불러오기
    {
        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            this.user = JsonConvert.DeserializeObject<UserInfo>(json);
        }
        else
        {
            this.user = null;
        }

        return user;
    }

    public Node FindNode(int chapterNum, int nodeNum)
    {
        Node find = null;
        switch (chapterNum)
        {
            case 0:
                {
                    find = this.prologueTree.FindData(this.prologueTree.root, nodeNum);
                }
                break;
            case 1:
                {
                    find = this.prologueTree.FindData(this.chapter1Tree.root, nodeNum);
                }
                break;
            case 2:
                {
                    find = this.prologueTree.FindData(this.chapter2Tree.root, nodeNum);
                }
                break;
            case 3:
                {
                    find = this.prologueTree.FindData(this.chapter2Tree.root, nodeNum);
                }
                break;
            case 4:
                {
                    find = this.prologueTree.FindData(this.finaleTree.root, nodeNum);
                }
                break;
        }
        return find;
    }

    private UserInfo SaveNeighborProfile(UserInfo user, int endingNum)
    {
        if (endingNum / 100 == 0)
        {
            //보라, 권찬훈 프로필 저장
            if (endingNum == 12)
            {
                if (!user.profileData.ContainsKey(1))
                {
                    user.profileData.Add(1, (int)eStatus.CLEAR);
                }
                else if (user.profileData[1] != (int)eStatus.CLEAR)
                {
                    user.profileData[1] = (int)eStatus.CLEAR;
                }

                if (!user.profileData.ContainsKey(2))
                {
                    user.profileData.Add(2, (int)eStatus.CLEAR);
                }
                else if (user.profileData[2] != (int)eStatus.CLEAR)
                {
                    user.profileData[2] = (int)eStatus.CLEAR;
                }
            }
        }
        else if(endingNum / 100 == 2)
        {
            //유성, 서율 프로필 저장
            if(endingNum == 205 || endingNum == 206)
            {
                if (!user.profileData.ContainsKey(3))
                {
                    user.profileData.Add(3, (int)eStatus.CLEAR);
                }
                else if (user.profileData[3] != (int)eStatus.CLEAR)
                {
                    user.profileData[3] = (int)eStatus.CLEAR;
                }
            }
            else if(endingNum == 211 || endingNum == 212)
            {
                if (!user.profileData.ContainsKey(4))
                {
                    user.profileData.Add(4, (int)eStatus.CLEAR);
                }
                else if (user.profileData[4] != (int)eStatus.CLEAR)
                {
                    user.profileData[4] = (int)eStatus.CLEAR;
                }
            }
        }
        return user;
    }

    public UserInfo GetUser()
    {
        this.LoadData();
        UserInfo tmpUser = this.user ?? null;
        return tmpUser;
    }

    public int CalculationEnding()
    {
        if (this.GetUser() != null)
        {
            var total = this.endingNum.Count;
            var progress = this.GetUser().arrEnding.Count;
            var presentProgress = progress * 100 / total;
            return presentProgress;
        }

        return 0;
    }
}