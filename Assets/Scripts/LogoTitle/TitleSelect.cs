using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using Newtonsoft.Json;
using DG.Tweening;

public partial class TitleSelect : MonoBehaviour
{
    public enum eStatus
    {
        DEVELOP, BUILD
        //0: 개발중, 1: 개발완료
    }

    public Action<int> onComplete;
    public Action onPlayClickSound;
    public Button[] btnChapters;
    public Button[] btnCloses;
    public Canvas exitCanvas;
    public Canvas prepareCanvas;
    public Button btnExit;
    public Button btnReturn;
    public Button btnOK;
    public Button btnTweet;
    public AudioSource clickPlayer;
    public AudioClip clickSound;
    public AudioClip mainClip;
    public Image dim;
    public Sprite[] chaptersOpen;
    public Sprite chapterUnlock;
    public RectTransform alarmBox;
    public Text alarmText;
    public GameObject[] chapterCanvases;        //0: 챕터 1, 1: 챕터 3, 2: 메인 챕터
    public IAPButton[] purchaseButtons;         //위의 주석과 동일한 구성요소
    public eStatus statusChapter;
    public string[] productIds;

    private AudioSource mainAudio;
    private App app;


    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                this.exitCanvas.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator FadeOutDim()
    {
        for (float i = dim.color.a; i >= 0; i -= 0.05f)
        {
            Color a = dim.color;
            a.a = i;
            dim.color = a;
            yield return null;
        }
        Color tmp = new Color(1, 1, 1, 0);
        dim.color = tmp;
    }

    private IEnumerator FadeInDim(int x)
    {
        for (float i = dim.color.a; i <= 1; i += 0.05f)
        {
            Color a = dim.color;
            a.a = i;
            dim.color = a;
            yield return null;
        }
        this.onComplete(x);
    }

    private IEnumerator ReturnAlarmBox()
    {
        yield return new WaitForSeconds(0.5f);
        this.alarmBox.DOAnchorPos(new Vector2(0, 1270), 0.85f, true).SetEase(Ease.InOutBack);
    }

    public void InitImpl()
    {
        this.app = FindObjectOfType<App>();
        this.prepareCanvas.gameObject.SetActive(false);
        this.mainAudio = GetComponent<AudioSource>();
        this.mainAudio.clip = this.mainClip;
        this.mainAudio.Play();
        this.mainAudio.loop = true;
        this.mainAudio.volume = 0.3f;
        StartCoroutine(this.FadeOutDim());

        //0: Prologue, 1~3: Chapter1~3, 4: Main Chapter
        for (int i = 0; i < this.btnChapters.Length; i++)
        {
            int x = i;

            var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
            if (System.IO.File.Exists(path))
            {
                var existUser = JsonConvert.DeserializeObject<UserInfo>(System.IO.File.ReadAllText(path));
                var keys = existUser.arrEnding.Keys.ToArray();
                foreach (var k in keys)
                {
                    string temp = k.Remove(0, 1);
                    if (Convert.ToInt32(temp) / 100 == x)
                    {
                        this.btnChapters[x].gameObject.GetComponent<Image>().sprite = this.chaptersOpen[x];
                    }
                }
            }

            this.btnChapters[x].onClick.AddListener(() =>
            {
                int originIndex = 1;
                this.onPlayClickSound();
                if (x == 1 || x == 3 || x == 4)
                {
                    if (this.statusChapter == eStatus.DEVELOP)
                    {
                        this.prepareCanvas.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (x == 3)
                        {
                            x = 1;
                            originIndex = 3;
                        }
                        else if (x == 4)
                        {
                            x = 2;
                            originIndex = 4;
                        }
                        else if (x == 1)
                        {
                            x = 0;
                            originIndex = 1;
                        }

                        if (DataManager.Instance.Purchase != null
                        && DataManager.Instance.Purchase.chapterName[x] == this.productIds[x])
                        {
                            x = originIndex;
                            if(DataManager.Instance.arrTrees[x] == null)
                            {
                                this.prepareCanvas.gameObject.SetActive(true);
                            }
                            StartCoroutine(this.FadeInDim(x));
                        }
                        else
                        {
                            Debug.Log("x ============> " + x);
                            this.chapterCanvases[x].SetActive(true);
                        }
                    }
                    x = originIndex;
                }
                else
                {
                    StartCoroutine(this.FadeInDim(x));
                }
            });
        }

        for (int i = 0; i < this.btnCloses.Length; i++)
        {
            int x = i;
            this.btnCloses[x].onClick.AddListener(() =>
            {
                this.onPlayClickSound();
                this.chapterCanvases[x].gameObject.SetActive(false);
            });
        }

        this.btnTweet.onClick.AddListener(() =>
        {
            Application.OpenURL("https://twitter.com/Team00355218");
        });

        DefineEvents();
    }

    private void DefineEvents()
    {
        this.AttachIAPButtonEvent();

        this.btnOK.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            this.prepareCanvas.gameObject.SetActive(false);
        });

        this.btnExit.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            Application.Quit();
        });

        this.btnReturn.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            this.exitCanvas.gameObject.SetActive(false);
        });

        this.onPlayClickSound = () =>
        {
            this.clickPlayer.clip = this.clickSound;
            this.clickPlayer.Play();
        };
    }

    public void RefreshChapterButton()
    {
        for(int i = 0; i < this.purchaseButtons.Length; i++)
        {
            for (int j = 0; j < DataManager.Instance.Purchase.chapterName.Count; j++)
            {
                if (this.purchaseButtons[i].productId == DataManager.Instance.Purchase.chapterName[j])
                {
                    var image = this.purchaseButtons[i].gameObject.GetComponent<Image>();
                    image.sprite = this.chapterUnlock;
                }
            }
        }
    }
}
