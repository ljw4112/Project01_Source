using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogScript : MonoBehaviour
{
    public Action onLoad;
    public Button pass;         //스크립트 넘기는 버튼(이미지 안에 내장)

    public GameObject mainDialogPrefab;
    public GameObject mainDialogPrefab2;
    public GameObject dialogPrefab;
    public GameObject dialogPrefab2;
    public GameObject narrationPrefab;
    public GameObject cutScenePrefab;
    public GameObject selectPrefab;
    public GameObject findingCluePrefab;
    public GameObject limitedSelectPrefab;

    public Transform grid;
    public Transform canvasTransform;

    public Image fadeDim;
    public Image tutorial;

    public bool pause;

    private int keyIndex;

    private bool ended;
    private bool loading;
    private bool isEnterTimeAttackZone;

    private Coroutine dialogCoroutine;
    private List<int> observUserSelect;
    private List<NonSelect> arrNonSelect;
    private Node node;

    private void Start()
    {
        //테스트 할 때의 진입점.
        //서버 켜져있을 때.
        //this.Init(1);
    }

    public void Init(int chapterNum, bool isFirst, int keyIndex = 1)
    {
        if (Screen.height > 1920)
        {
            this.pass.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height / 1.2f);
        }

        DataManager.Instance.GetData();
        this.tutorial.gameObject.SetActive(false);
        this.pause = true;
        if (isFirst)
        {
            this.fadeDim.color = Color.black;
        }
        StartCoroutine(this.FadeAndInit(chapterNum, isFirst, keyIndex));
    }

    private IEnumerator FadeAndInit(int chapterNum, bool isFirst, int keyIndex = 1)
    {
        for (float i = 1; i >= 0; i -= 0.03f)
        {
            Color tColor = this.fadeDim.color;
            tColor.a = i;
            this.fadeDim.color = tColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);
        this.InitImpl(chapterNum, isFirst, keyIndex);
    }

    private void InitImpl(int chapterNum, bool isFirst, int keyIndex = 1)
    {
        this.pause = true;
        this.keyIndex = 1;
        this.observUserSelect = new List<int>();
        this.arrNonSelect = new List<NonSelect>();

        if (keyIndex > 1)
        {
            this.OnLoad(InGame.Instance.user);
        }

        DialogAction.Instance.onComplete = () =>
        {
            if (this.node.dialogData[this.keyIndex].char_id / 100 != 25)
            {
                this.pause = false;
            }
            if (this.dialogCoroutine != null)
            {
                StopCoroutine(this.dialogCoroutine);
                this.dialogCoroutine = null;
            }
            this.dialogCoroutine = StartCoroutine(this.PassDialog());
        };

        InGame.Instance.onStopCoroutine = () =>
        {
            if (this.dialogCoroutine != null)
            {
                StopCoroutine(this.dialogCoroutine);
            }
            this.pause = true;
        };

        if (keyIndex == 1)
        {
            switch (chapterNum)
            {
                case 0:
                    {
                        this.node = DataManager.Instance.prologueTree.root;
                    }
                    break;
                case 1:
                    {
                        this.node = DataManager.Instance.chapter1Tree.root;
                    }
                    break;
                case 2:
                    {
                        this.node = DataManager.Instance.chapter2Tree.root;
                    }
                    break;
                case 3:
                    {
                        this.node = DataManager.Instance.chapter3Tree.root;
                    }
                    break;
                case 4:
                    {
                        this.node = DataManager.Instance.finaleTree.root;
                    }
                    break;
            }
        }


        this.observUserSelect.Add(this.node.num);
        this.dialogCoroutine = StartCoroutine(this.PassDialog());

        InGame.Instance.onDialogStart = () =>
        {
            if (this.dialogCoroutine == null)
            {
                this.dialogCoroutine = StartCoroutine(this.PassDialog());
            }
        };

        InGame.Instance.onDialogStop = () =>
        {
            if (this.dialogCoroutine != null)
            {
                StopCoroutine(this.dialogCoroutine);
            }
            this.dialogCoroutine = null;
        };

        InGame.Instance.onDialogLoadComplete();

        if (isFirst)
        {
            StartCoroutine(PrintTutorialPage());

        }
        else
        {
            this.pause = false;
        }
    }

    private IEnumerator PrintTutorialPage()
    {
        this.tutorial.gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        this.tutorial.gameObject.SetActive(false);
        this.pause = false;
    }

    public void OnLoad(UserInfo user)
    {
        string bgmName = "";
        string bgmVolume = "";

        if (user != null)
        {
            this.observUserSelect = user.saveChapterList;
            this.arrNonSelect = user.saveChapterNonSelectList;
            this.loading = true;

            //기존 오브젝트 모두 삭제
            foreach (Transform child in this.grid)
            {
                Destroy(child.gameObject);
            }

            int chapterNum = this.observUserSelect[this.observUserSelect.Count - 1] / 100;
            int nodeNum = this.observUserSelect[this.observUserSelect.Count - 1];
            this.node = DataManager.Instance.FindNode(chapterNum, nodeNum);
            while (this.keyIndex < user.keyIndex)
            {
                if (this.node.dialogData[this.keyIndex].action.Contains("BGAudio") ||
                    this.node.dialogData[this.keyIndex].action.Contains("BGAudioVolume"))
                {
                    string[] actions = this.node.dialogData[this.keyIndex].action.Split(',');
                    bgmName = Array.Find(actions, data => data.Contains("BGAudio"));
                    bgmVolume = Array.Find(actions, data => data.Contains("BGAudioVolume"));
                }
                this.DialogImpl();
            }

            if (bgmName == "" && bgmVolume == "")
            {
                //전 노드를 탐색하며 BGM이 계속 깔려있었는지 확인
                int prevNodeNum = this.observUserSelect[this.observUserSelect.Count - 2];
                Node prevNode = DataManager.Instance.FindNode(prevNodeNum / 100, prevNodeNum);
                int size = prevNode.dialogData.Count;
                for (int i = size; i > 0; i--)
                {
                    string actionData = prevNode.dialogData[i].action;
                    if (actionData.Contains("BGAudio") ||
                        actionData.Contains("BGAudioVolume"))
                    {
                        string[] actions = actionData.Split(',');
                        bgmName = Array.Find(actions, data => data.Contains("BGAudio"));
                        bgmVolume = Array.Find(actions, data => data.Contains("BGAudioVolume"));
                    }

                    if (bgmName != "" && bgmVolume != "")
                    {
                        break;
                    }
                }
            }
            this.loading = false;
            this.pause = false;
            if (bgmName != "" || bgmVolume != "")
            {
                DialogAction.Instance.Divide(bgmName + "," + bgmVolume);
            }
        }
    }

    //마우스를 클릭했을 때
    public void mouseUp()
    {
        if (!this.ended)
        {
            if (this.pause && this.dialogCoroutine != null)
            {
                StopCoroutine(this.dialogCoroutine);
            }
            else
            {
                if (!this.pause)
                {
                    this.DialogImpl();
                    StopCoroutine(this.dialogCoroutine);
                    this.dialogCoroutine = StartCoroutine(this.PassDialog());
                }
            }
        }
    }

    private IEnumerator PassDialog()
    {
        while (!this.ended)
        {
            if (!this.pause)
            {
                yield return new WaitForSeconds(InGame.Instance.dialogSpeed);
                if (!this.pause)
                {
                    this.DialogImpl();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void DialogImpl()
    {
        var nowDialog = this.node.dialogData[this.keyIndex];
        Prologue nextDialog = null;
        int cnt2 = 0;

        //"\n" 개수 체크
        int cnt = Regex.Matches(nowDialog.dialog, "\n").Count;

        if (nowDialog.char_id / 100 != 27)
        {
            nextDialog = this.node.dialogData[this.keyIndex + 1];
            cnt2 = Regex.Matches(nextDialog.dialog, "\n").Count;
        }

        if (!this.loading && nowDialog.action != "null")
        {
            this.pause = true;
            DialogAction.Instance.Divide(nowDialog.action);
        }


        if (nowDialog.char_id < 2300 || (nowDialog.char_id >= 2800 && nowDialog.char_id <= 2900))
        {
            GameObject diaGo = null;

            //주인공 대화일때
            if (nowDialog.char_id == 100)
            {
                //그 전의 Dialog와 주체가 동일한 경우 (썸네일 아이디가 같을 경우)
                this.DisplayMainCharDialog(nowDialog, nowDialog.dialog, cnt, diaGo);
            }
            //주인공이 아닌 사람이 대화할 때
            else
            {
                this.DisplayOtherCharDialog(nowDialog, nowDialog.dialog, cnt, diaGo);
            }
        }

        var dataIndex = nowDialog.char_id / 100;
        //대사 이외의 것들
        switch (dataIndex)
        {
            //나레이션 (배경 有)
            case 23:
                {
                    var narGo = Instantiate(this.narrationPrefab, this.grid);
                    narGo.GetComponent<UINarration>().Init(nowDialog.dialog, true);
                }
                break;
            //나레이션 (배경 無)
            case 24:
                {
                    var narGo = Instantiate(this.narrationPrefab, this.grid);
                    narGo.GetComponent<UINarration>().Init(nowDialog.dialog, false);
                }
                break;
            //선택지
            case 25:
                {
                    //불러오기를 하고 있지 않을 때
                    if (!this.loading)
                    {
                        //스크립트 정지 후 프리팹 생성
                        this.pause = true;
                        var selGo = Instantiate(this.selectPrefab, this.grid);
                        selGo.GetComponent<UISelect>().Init(nowDialog.dialog, nextDialog.dialog);

                        //말풍선 사이즈 조절
                        var vertical = selGo.transform.GetChild(0).gameObject.GetComponent<VerticalLayoutGroup>();
                        StartCoroutine(this.Refresh(vertical, 40));

                        if (cnt + cnt2 >= 1)
                        {
                            float multi = 1;
                            multi = Mathf.Pow(1.23f, cnt + cnt2);
                            var rec = selGo.GetComponent<RectTransform>();
                            rec.sizeDelta = new Vector2(rec.rect.width, rec.rect.height * multi);
                        }

                        //1번 버튼 선택 이벤트
                        selGo.GetComponent<UISelect>().Button1.onClick.AddListener(() =>
                        {
                            this.pause = false;
                            InGame.Instance.onPlayClickSound();
                            Destroy(this.grid.GetChild(this.grid.childCount - 1).gameObject);
                            this.DisplayMainCharDialog(nowDialog, nowDialog.dialog, cnt, null, 1);

                            if (nowDialog.char_id == 2500)
                            {
                                this.node = this.node.left;
                                this.observUserSelect.Add(this.node.num);
                                this.keyIndex = 1;
                            }
                            else
                            {
                                this.keyIndex += 2;
                                this.arrNonSelect.Add(new NonSelect(this.node.num, 'L'));
                            }


                            if (this.dialogCoroutine != null)
                            {
                                StopCoroutine(this.dialogCoroutine);
                            }
                            this.dialogCoroutine = StartCoroutine(this.PassDialog());
                        });

                        if (nextDialog.dialog != "null")
                        {
                            //2번 버튼 선택지 처리
                            selGo.GetComponent<UISelect>().Button2.onClick.AddListener(() =>
                            {
                                InGame.Instance.onPlayClickSound();
                                this.pause = false;
                                this.keyIndex++;

                                if (nowDialog.char_id == 2500)
                                {
                                    this.node = this.node.right;
                                    this.observUserSelect.Add(this.node.num);
                                    this.keyIndex = 1;
                                }

                                Destroy(this.grid.GetChild(this.grid.childCount - 1).gameObject);

                                this.DisplayMainCharDialog(nextDialog, nextDialog.dialog, cnt2, null, 2);
                                if (nowDialog.char_id == 2510)
                                {
                                    this.keyIndex++;
                                    this.arrNonSelect.Add(new NonSelect(this.node.num, 'R'));
                                }
                                if (this.dialogCoroutine != null)
                                {
                                    StopCoroutine(this.dialogCoroutine);
                                }
                                this.dialogCoroutine = StartCoroutine(this.PassDialog());
                            });
                        }
                        StopCoroutine(this.PassDialog());
                    }
                }
                break;
            //컷씬
            case 26:
                {
                    var cutGo = Instantiate(this.cutScenePrefab, this.grid);

                    if (nowDialog.dialog != "0")
                    {
                        cutGo.GetComponent<UICutscene>().Init(nowDialog.dialog, this.node.num / 100);
                    }

                    InGame.Instance.RefreshAlbum();
                }
                break;
            //EOF
            case 27:
                {
                    if ((this.node.left != null && this.node.right != null) && (this.node.left.num == this.node.right.num))
                    {
                        //자식이 있어 챕터가 종료되지 않는다면?
                        if(this.node.dialogData[this.keyIndex].char_id == 2701)
                        {
                            DataManager.Instance.SaveEndingData(this.observUserSelect, this.arrNonSelect, InGame.Instance.GetDeadCount());
                            DialogAction.Instance.Divide("Refresh=True,Color=000000");
                            this.node = this.node.left;
                        }
                        else
                        {
                            this.node = this.node.left;
                            this.observUserSelect.Add(this.node.num);
                        }
                        this.keyIndex = 0;
                    }
                    else
                    {
                        this.ended = true;
                        DataManager.Instance.SaveEndingData(this.observUserSelect, this.arrNonSelect, InGame.Instance.GetDeadCount());

                        var presentNode = this.observUserSelect[this.observUserSelect.Count - 1];
                        InGame.Instance.CreateEndingPage(presentNode / 100, DataManager.Instance.arrEndingData[presentNode].endingName);

                        InGame.Instance.onRefresh = () =>
                        {
                            InGame.Instance.PrintEnding();
                            InGame.Instance.RefreshRecord();
                            InGame.Instance.RefreshMenu();
                            InGame.Instance.RefreshAlbum();
                        };
                    }
                }
                break;
            //타임어택 선택지
            case 30:
                {
                    if (nowDialog.char_id == 3000)
                    {
                        //로딩중이 아닐 때 (정상적인 게임진행)
                        if (!this.loading)
                        {
                            this.pause = true;

                            var timeGo = Instantiate(this.limitedSelectPrefab, this.grid);
                            var time = timeGo.GetComponent<UITimeAttack>();
                            if (nextDialog.char_id == 3000)
                            {
                                time.Init(nowDialog.dialog, nextDialog.dialog, 10f);
                            }
                            else
                            {
                                time.Init(nowDialog.dialog, null, 10f);
                                var timeRect = timeGo.GetComponent<RectTransform>();
                                timeRect.sizeDelta = new Vector2(timeRect.rect.width, timeRect.rect.height / 3.06f);
                            }
                            StartCoroutine(this.CalculateLimitedSelectTime(time));
                        }
                    }
                    else
                    {
                        if (!this.isEnterTimeAttackZone)
                        {
                            this.isEnterTimeAttackZone = true;
                            var timeAttack = FindObjectOfType<UITimeAttackZone>();
                            StartCoroutine(this.StartTimeAttackZone(timeAttack, Convert.ToSingle(nowDialog.dialog)));
                        }
                        else if (this.isEnterTimeAttackZone)
                        {
                            this.isEnterTimeAttackZone = false;
                        }
                    }
                }
                break;
            //단서찾기
            case 31:
                {
                    if (!this.loading)
                    {
                        this.pause = true;

                        var findGo = Instantiate(Resources.Load<GameObject>("FindingClue/" + nowDialog.dialog), this.canvasTransform);
                        var findClue = findGo.GetComponent<UIFindingClue>();
                        findClue.Init(findClue.deadline);
                        StartCoroutine(this.ProgressFindingClue(findClue));
                    }
                }
                break;
        }
        if (!this.pause || nowDialog.action != "null" || this.loading)
        {
            this.keyIndex++;
        }
    }

    private IEnumerator CalculateLimitedSelectTime(UITimeAttack ta)
    {
        while (true)
        {
            if (ta.GetIsClear() || ta.GetIsOver())
            {
                break;
            }
            yield return null;
        }

        if (ta.GetIsClear())
        {
            this.node = this.node.left;
            if (!this.observUserSelect.Exists(x => x == this.node.num))
            {
                this.observUserSelect.Add(this.node.num);
            }
        }
        else
        {
            this.node = this.node.right;
            InGame.Instance.onDead();
            if (!this.observUserSelect.Exists(x => x == this.node.num))
            {
                this.observUserSelect.Add(this.node.num);
            }
        }
        this.keyIndex = 1;
        Destroy(ta.gameObject);
        this.pause = false;
    }

    private IEnumerator ProgressFindingClue(UIFindingClue find)
    {
        while (true)
        {
            if (find.GetClear())
            {
                yield return new WaitForSeconds(1.5f);
                break;
            } else if (find.GetIsEnd())
            {
                break;
            }
            yield return null;
        }

        if (find.GetClear())
        {
            this.node = this.node.left;
            this.observUserSelect.Add(this.node.num);
        }
        else
        {
            this.node = this.node.right;
            this.observUserSelect.Add(this.node.num);
        }
        this.keyIndex = 1;
        this.pause = false;
        Destroy(find.gameObject);
    }

    private IEnumerator Refresh(VerticalLayoutGroup obj, int spacing)
    {
        yield return new WaitForEndOfFrame();
        obj.enabled = false;
        obj.CalculateLayoutInputVertical();
        obj.enabled = true;
        obj.spacing = spacing;
    }

    private IEnumerator StartTimeAttackZone(UITimeAttackZone time, float seconds)
    {
        time.Init(seconds);
        while (true)
        {
            if (!this.isEnterTimeAttackZone || time.GetIsOver())
            {
                break;
            }
            yield return null;
        }
        time.End();
        time.SetIsStart(false);
        if (time.GetIsOver())
        {
            this.node = this.node.right;
            this.keyIndex = 1;
        }
        else
        {
            this.node = this.node.left;
            this.keyIndex = 1;
        }
    }

    public List<int> GetObservList()
    {
        return this.observUserSelect;
    }

    public List<NonSelect> GetNonSelectList()
    {
        return this.arrNonSelect;
    }

    public int GetKeyIndex()
    {
        return this.keyIndex;
    }


    //주인공 대사 출력
    private void DisplayMainCharDialog(Prologue nowDialog, string dialog, int cnt, GameObject diaGo = null, int btnNum = 0)
    {
        bool isCharIdChanged = default;
        int defaultCharId = 0;

        if (nowDialog.char_id == 2500 || nowDialog.char_id == 2510)
        {
            defaultCharId = nowDialog.char_id == 2500 ? 2500 : 2510;
            isCharIdChanged = true;
            nowDialog.char_id = 100;
        }

        if (defaultCharId / 100 == 25)
        {
            if (this.keyIndex > btnNum && this.node.dialogData[this.keyIndex - btnNum].thumbnail_id == this.node.dialogData[this.keyIndex].thumbnail_id)
            {
                diaGo = Instantiate(this.mainDialogPrefab2, this.grid);
                diaGo.GetComponent<UIDialog>().Init(dialog);

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
                if (nowDialog.thumbnail_id != 0)
                {
                    diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, nowDialog.dialog, DataManager.Instance.arrCharacterThumbnail[nowDialog.thumbnail_id].thumbnail_name);
                }
                else
                {
                    diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, nowDialog.dialog, "0");
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
            if (this.keyIndex > 1 && (this.node.dialogData[this.keyIndex - 1].thumbnail_id == this.node.dialogData[this.keyIndex].thumbnail_id))
            {
                diaGo = Instantiate(this.mainDialogPrefab2, this.grid);
                diaGo.GetComponent<UIDialog>().Init(dialog);

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
                if (nowDialog.thumbnail_id != 0)
                {
                    diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, nowDialog.dialog, DataManager.Instance.arrCharacterThumbnail[nowDialog.thumbnail_id].thumbnail_name);
                }
                else
                {
                    diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, nowDialog.dialog, "0");
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

        if (isCharIdChanged)
        {
            nowDialog.char_id = defaultCharId;
        }
    }

    private void DisplayOtherCharDialog(Prologue nowDialog, string dialog, int cnt, GameObject diaGo = null)
    {
        if (this.keyIndex != 1 &&
                   this.node.dialogData[this.keyIndex - 1].thumbnail_id == this.node.dialogData[this.keyIndex].thumbnail_id)
        {
            diaGo = Instantiate(this.dialogPrefab2, this.grid);
            diaGo.GetComponent<UIDialog>().Init(dialog);

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
            if (nowDialog.thumbnail_id != 0)
            {
                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, dialog, DataManager.Instance.arrCharacterThumbnail[nowDialog.thumbnail_id].thumbnail_name);
            }
            else
            {
                diaGo.GetComponent<UIDialogG>().Init(DataManager.Instance.arrCharData[nowDialog.char_id].name, dialog, "0");
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
