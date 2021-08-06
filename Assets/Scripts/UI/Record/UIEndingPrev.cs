using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIEndingPrev : MonoBehaviour
{
    public GameObject branchBtnPrefab;
    public GameObject mainDialogPrefab2;
    public GameObject mainDialogPrefab;
    public GameObject dialogPrefab2;
    public GameObject dialogPrefab;
    public GameObject narrationPrefab;
    public GameObject cutScenePrefab;
    public Transform branchBtnGrid;
    public Transform grid;
    public ScrollRect scrollView;
    public Button btnClose;
    public Sprite prevBg;
    public Sprite originBg;
    public Sprite branchBtnImage;
    public Sprite branchBtnSelectImage;
    public Image bg;
    public RectTransform endingPrevRect;

    private Node node;
    private UserInfo user;
    private List<Button> createdButtons;
    private int endingNum;
    private int keyIndex;
    private int nonSelectIndex;
    private List<char> arrNonSelect;

    public void Init(UserInfo user, int endingNum)
    {
        if (Screen.height > 1920)
        {
            this.endingPrevRect.sizeDelta = new Vector2(Screen.width, Screen.height / 1.5f);
        }

        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        this.gameObject.SetActive(true);

        this.createdButtons = new List<Button>();
        //this.bg.sprite = prevBg;
        this.user = user;
        this.endingNum = endingNum;
        this.arrNonSelect = new List<char>();

        foreach(Transform grid in this.branchBtnGrid)
        {
            Destroy(grid.gameObject);
        }

        foreach (Transform child in this.grid)
        {
            Destroy(child.gameObject);
        }

        this.InitBranchButton();

        this.btnClose.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.gameObject.SetActive(false);
        });
    }

    private void InitBranchButton()
    {
        int index = 1;
        foreach (var nodeNum in this.user.arrEnding["e" + this.endingNum].clearRoot)
        {
            //노드안의 다이얼로그 구조체 크기가 2 이하일 경우 생략하는 과정
            Node tmpFind = DataManager.Instance.FindNode(nodeNum / 100, nodeNum);
            if(tmpFind.dialogData.Count <= 2)
            {
                continue;
            }

            var btnGo = Instantiate(this.branchBtnPrefab, this.branchBtnGrid);
            var btnInit = btnGo.GetComponent<UIBranch>();
            btnInit.Init((index++).ToString());
            btnInit.btn.gameObject.GetComponent<Image>().sprite = branchBtnImage;
            this.createdButtons.Add(btnInit.btn);
            btnInit.btn.onClick.AddListener(() =>
            {
                InGame.Instance.onPlayClickSound();
                scrollView.verticalNormalizedPosition = 1;
                foreach(var i in this.createdButtons)
                {
                    i.gameObject.GetComponent<Image>().sprite = branchBtnImage;
                }
                btnInit.btn.gameObject.GetComponent<Image>().sprite = branchBtnSelectImage;

                foreach(Transform child in this.grid)
                {
                    Destroy(child.gameObject);
                }

                this.InitializeDialog(nodeNum);
                while(this.node.dialogData[this.keyIndex].char_id / 100 != 27)
                {
                    this.DisplayDialog();
                }
            });
        }
    }

    private void InitializeDialog(int btnNum)
    {
        //해당구간의 시작점 설정
        this.node = DataManager.Instance.FindNode(this.endingNum / 100, btnNum);
        this.keyIndex = 1;

        //비분기 선택지 정보 가져오기
        var tmp = user.arrEnding["e" + endingNum].nonSelect;
        this.nonSelectIndex = 0;
        foreach (var i in tmp)
        {
            if (i.nodeNum == btnNum)
            {
                this.arrNonSelect.Add(i.direction);
            }
        }
    }

    private void DisplayDialog()
    {
        var tmp = node.dialogData[this.keyIndex];
        Prologue tmp2 = null;
        int cnt2 = 0;

        //"\n" 개수 체크
        MatchCollection matches = Regex.Matches(tmp.dialog, "\n");
        int cnt = matches.Count;

        if (tmp.char_id / 100 != 27)
        {
            tmp2 = this.node.dialogData[this.keyIndex + 1];

            MatchCollection matches2 = Regex.Matches(tmp2.dialog, "\n");
            cnt2 = matches2.Count;
        }

        if (tmp.char_id < 2300 || (tmp.char_id >= 2800 && tmp.char_id <= 2900))
        {
            GameObject diaGo;

            //주인공 대화일때
            if (tmp.char_id == 100)
            {
                //그 전의 Dialog와 주체가 동일한 경우 (썸네일 아이디가 같을 경우)
                if (this.keyIndex != 1 &&
                    this.node.dialogData[this.keyIndex - 1].thumbnail_id == this.node.dialogData[this.keyIndex].thumbnail_id)
                {
                    diaGo = Instantiate(this.mainDialogPrefab2, this.grid);
                    diaGo.GetComponent<UIDialog>().Init(tmp.dialog);

                    if (cnt >= 1)
                    {
                        float multi = 1;
                        for (int i = 0; i < cnt; i++)
                        {
                            multi += 0.5f;
                        }

                        var rec = diaGo.GetComponent<RectTransform>();
                        rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                    }
                }
                else
                {
                    diaGo = Instantiate(this.mainDialogPrefab, this.grid);
                    if (tmp.thumbnail_id != 0)
                    {
                        diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[tmp.char_id].name, tmp.dialog, DataManager.Instance.arrCharacterThumbnail[tmp.thumbnail_id].thumbnail_name);
                    }
                    else
                    {
                        diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[tmp.char_id].name, tmp.dialog, "0");
                    }

                    if (cnt >= 1)
                    {
                        float multi = 1;
                        for (int i = 0; i < cnt; i++)
                        {
                            multi += 0.27f;
                        }

                        var rec = diaGo.GetComponent<RectTransform>();
                        rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                    }
                }
            }
            else
            {
                if (this.keyIndex != 1 &&
                   this.node.dialogData[this.keyIndex - 1].thumbnail_id == this.node.dialogData[this.keyIndex].thumbnail_id)
                {
                    diaGo = Instantiate(this.dialogPrefab2, this.grid);
                    diaGo.GetComponent<UIDialog>().Init(tmp.dialog);

                    if (cnt >= 1)
                    {
                        float multi = 1;
                        for (int i = 0; i < cnt; i++)
                        {
                            multi += 0.5f;
                        }

                        var rec = diaGo.GetComponent<RectTransform>();
                        rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                    }
                }
                else
                {
                    diaGo = Instantiate(this.dialogPrefab, this.grid);
                    if (tmp.thumbnail_id != 0)
                    {
                        diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[tmp.char_id].name, tmp.dialog, DataManager.Instance.arrCharacterThumbnail[tmp.thumbnail_id].thumbnail_name);
                    }
                    else
                    {
                        diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[tmp.char_id].name, tmp.dialog, "0");
                    }

                    if (cnt >= 1)
                    {
                        float multi = 1;
                        for (int i = 0; i < cnt; i++)
                        {
                            multi += 0.27f;
                        }

                        var rec = diaGo.GetComponent<RectTransform>();
                        rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                    }
                }
            }
            
        }

        var dataIndex = tmp.char_id / 100;
        //대사 이외의 것들
        switch (dataIndex)
        {
            //나레이션 (배경 有)
            case 23:
                {
                    var narGo = Instantiate(this.narrationPrefab, this.grid);
                    narGo.GetComponent<UINarration>().Init(tmp.dialog, true);
                }
                break;
            //나레이션 (배경 無)
            case 24:
                {
                    var narGo = Instantiate(this.narrationPrefab, this.grid);
                    narGo.GetComponent<UINarration>().Init(tmp.dialog, false);
                }
                break;
            //선택지
            case 25:
                {
                    if(tmp.char_id != 2500)
                    {
                        var diaGo = Instantiate(this.mainDialogPrefab, this.grid);
                        if (this.arrNonSelect[this.nonSelectIndex++] == 'L')
                        {
                            if (DataManager.Instance.arrCharacterThumbnail.ContainsKey(tmp2.thumbnail_id))
                            {
                                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[100].name, tmp.dialog, DataManager.Instance.arrCharacterThumbnail[tmp.thumbnail_id].thumbnail_name);
                            }
                            else
                            {
                                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[100].name, tmp.dialog, "0");
                            }
                            
                            if (cnt >= 1)
                            {
                                float multi = 1;
                                for (int i = 0; i < cnt; i++)
                                {
                                    multi += 0.5f;
                                }

                                var rec = diaGo.GetComponent<RectTransform>();
                                rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                            }
                        }
                        else
                        {
                            if (DataManager.Instance.arrCharacterThumbnail.ContainsKey(tmp2.thumbnail_id))
                            {
                                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[100].name, tmp2.dialog, DataManager.Instance.arrCharacterThumbnail[tmp2.thumbnail_id].thumbnail_name);
                            }
                            else
                            {
                                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[100].name, tmp2.dialog, "0");
                            }

                            if (cnt2 >= 1)
                            {
                                float multi = 1;
                                for (int i = 0; i < cnt2; i++)
                                {
                                    multi += 0.5f;
                                }

                                var rec = diaGo.GetComponent<RectTransform>();
                                rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                            }
                        }
                        this.keyIndex++;
                    }
                }
                break;
            //컷씬
            case 26:
                {
                    var cutGo = Instantiate(this.cutScenePrefab, this.grid);

                    if (tmp.dialog != "0")
                    {
                        cutGo.GetComponent<UICutscene>().Init(tmp.dialog, this.node.num / 100);
                    }
                }
                break;
        }
        this.keyIndex++;
    }
}
