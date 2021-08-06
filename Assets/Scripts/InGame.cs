using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame : MonoBehaviour
{
    public Action onPauseAction;
    public Action onStartAction;
    public Action onStopCoroutine;
    public Action onDialogStop;
    public Action onDialogStart;
    public Action onDialogLoadComplete;         //���̾�α� �ε��� ������ ��
    public Action onChapterClear;               //é�Ͱ� ������ �� é�� ����ȭ������ �̵���
    public Action onPlayClickSound;
    public Action onDead;
    public Action onRefresh;
    public Action<bool> onComplete;
    public Action<bool> onVibratorPress;        //���� ��ư�� ���� �Ŀ� �߻�
    public GameObject vibeOn;
    public GameObject vibeOff;
    public Text progressIcon;
    public Button btnExit;
    public Button btnReturn;
    public Button btnOption;                    //�ɼ� ��ư
    public Canvas endingPage;
    public Canvas exitCanvas;
    public AudioSource bgPlayer;                //������� ��� �Ű�ü
    public AudioSource efPlayer;
    public AudioSource clickPlayer;
    public AudioClip clickSound;
    public UserInfo user;
    public SpriteAtlas endingThumbnail;
    public SpriteAtlas thumbAtlas;
    public SpriteAtlas prologueAtlas;
    public SpriteAtlas chapter2_Atlas;
    public Image saveDim;
    public float dialogSpeed;
    public float masterVolume;                      //������ ����
    public bool isVibrator;                     //���� on,off ����

    private App app;
    private DialogScript chapter;
    private MenuController menu;
    private UIOption option;
    private float bgVolume;
    private float efVolume;
    private int deadCount;                      //���� ī��Ʈ

    private static InGame instance;
    public static InGame Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGame>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if(DataManager.Instance.GetUser() != null)
        {
            this.deadCount = DataManager.Instance.GetUser().deadCount;
        }
        this.menu = FindObjectOfType<MenuController>();
        this.chapter = FindObjectOfType<DialogScript>();
        this.option = FindObjectOfType<UIOption>();
        this.app = FindObjectOfType<App>();
        this.menu.Init();

        //���̾�α� �ε��� ������ ��, �޴���ư Ȱ��ȭ.
        this.onDialogLoadComplete = () =>
        {
            this.menu.btnGallery.enabled = true;
            this.menu.btnDialog.enabled = true;
            this.menu.btnProfile.enabled = true;
            this.menu.btnRecord.enabled = true;
        };

        this.onDead = () =>
        {
            this.deadCount++;
        };
    }

    void Start()
    {
        if (DataManager.Instance.GetUser() == null)
        {
            this.isVibrator = true;
            this.dialogSpeed = 1;
            this.masterVolume = 1f;
        }
        else
        {
            if (PlayerPrefs.GetFloat("Speed") == 0)
            {
                this.isVibrator = true;
                this.dialogSpeed = 1;
                this.masterVolume = 1f;
            }
            else
            {
                //�⺻ ����
                //1. ������ �ҷ�����
                this.isVibrator = Convert.ToBoolean(PlayerPrefs.GetInt("Vibration"));
                //2. ���Ӽӵ� �ҷ�����
                this.dialogSpeed = PlayerPrefs.GetFloat("Speed");
                //3.�����, ȿ���� ���� �ҷ�����
                this.masterVolume = PlayerPrefs.GetFloat("BGVol");
            }
        }

        //�ɼ� �˾����� ���� �Ѱ��ָ� �ʱ�ȭ
        this.option.Init(this.isVibrator, this.dialogSpeed, this.masterVolume);
        this.option.gameObject.SetActive(false);
        this.endingPage.gameObject.SetActive(false);

        if (this.isVibrator)
        {
            this.isVibrator = true;
            this.vibeOn.SetActive(true);
            this.vibeOff.SetActive(false);
        }
        else
        {
            this.isVibrator = false;
            this.vibeOn.SetActive(false);
            this.vibeOff.SetActive(true);
        }

        //��ư���� Ŭ�� �̺�Ʈ�� ����
        this.option.btnSave.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            this.option.btnExit.enabled = false;
            this.OnSave(chapter.GetObservList(), chapter.GetNonSelectList(), chapter.GetKeyIndex());
        });

        this.option.btnLoad.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            this.OnLoad();
        });

        this.option.sliderGameSpeed.onValueChanged.AddListener((value) =>
        {
            this.dialogSpeed = value;
        });

        this.option.sliderVolume.onValueChanged.AddListener((value) =>
        {
            AudioListener.volume = value;
            this.masterVolume = value;
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
            this.onDialogStart();
        });

        this.btnOption.onClick.AddListener(() =>
        {
            this.onPlayClickSound();
            if (this.onDialogStop != null)
            {
                this.onDialogStop();
            }
            this.option.gameObject.SetActive(true);
        });

        this.onVibratorPress = (result) =>
        {
            //������ On���� ���� ��
            if (result)
            {
                this.isVibrator = true;
                this.vibeOn.SetActive(true);
                this.vibeOff.SetActive(false);
            }
            else
            {
                this.isVibrator = false;
                this.vibeOn.SetActive(false);
                this.vibeOff.SetActive(true);
            }
        };

        this.onPlayClickSound = () =>
        {
            this.clickPlayer.clip = this.clickSound;
            this.clickPlayer.Play();
        };

        //������ ������ �� Record�� Init
        //���߿� é�� �ϳ��� ���������� Init���ֱ�
        this.PrintEnding();
        this.RefreshRecord();
        this.RefreshProfile();
        this.RefreshAlbum();
    }

    void Update()
    {
        //��� �������� Back�� ������ ȣ���.
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                this.onDialogStop();
                this.exitCanvas.gameObject.SetActive(true);
            }
        }
    }

    private void OnSave(List<int> nodes, List<NonSelect> nonSelect, int keyIndex)
    {
        //������ ���� ���������� �ƴ��� Ȯ����.
        var tmpNode = DataManager.Instance.FindNode(nodes[nodes.Count - 1] / 100, nodes[nodes.Count - 1]);
        bool isSelectDialog = tmpNode.dialogData[keyIndex].char_id == 2500 | tmpNode.dialogData[keyIndex].char_id == 2510;
        if (isSelectDialog)
        {
            keyIndex--;
        }

        this.saveDim.raycastTarget = true;
        this.option.onShowMessage("�������Դϴ�......", false);

        DataManager.Instance.SaveData(this.app.userID, nodes, nonSelect, keyIndex, this.deadCount, () =>
        {
            Server.Instance.StartUploadUserInfo(DataManager.Instance.GetUser(), (result) =>
            {
                this.option.onSuccessProcess(result);
                this.saveDim.raycastTarget = false;
            });
        });
    }

    private void OnLoad()
    {
        var tmpUser = DataManager.Instance.LoadData();
        if (tmpUser.saveChapterList.Count == 0)
        {
            this.option.onShowMessage("����� �����Ͱ� �����ϴ�.", true);
        }
        else
        {
            this.endingPage.gameObject.SetActive(true);
            this.endingPage.GetComponent<Image>().CrossFadeAlpha(1, 0.5f, true);
            this.onComplete(true);
        }
    }

    private void OnApplicationQuit()
    {
        //������ ����� �� �ɼ� ����.
        if (this.app.status == App.eStatus.TEST)
        {
            this.app.userID = "";
        }
        this.SaveOptions();

        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }

    public void RefreshRecord()
    {
        this.user = DataManager.Instance.GetUser();
        var recordInit = FindObjectOfType<UIRecord>();
        recordInit.Init(this.user);
    }

    public void RefreshAlbum()
    {
        this.user = DataManager.Instance.GetUser();
        var albumInit = FindObjectOfType<UIAlbum>();
        albumInit.Init(this.user);
    }

    public void RefreshProfile()
    {
        this.user = DataManager.Instance.GetUser();
        var profileInit = FindObjectOfType<UIProfile>();
        profileInit.Init(this.user);
    }

    public void RefreshMenu()
    {
        var menuInit = FindObjectOfType<MenuController>();
        menuInit.Init();
    }

    public void CreateEndingPage(int chapterNum, string endingName)
    {
        this.user = DataManager.Instance.GetUser();
        this.user.id = this.app.userID;
        Server.Instance.StartUploadUserInfo(this.user, null);
        this.endingPage.gameObject.SetActive(true);
        this.SaveOptions();
        var ending = this.endingPage.GetComponent<UIEndingPage>();
        ending.Init(chapterNum, endingName);

        if (onRefresh != null)
        {
            this.onRefresh();
        }
    }

    public void PrintEnding()
    {
        var caculation = DataManager.Instance.CalculationEnding();
        this.progressIcon.text = caculation.ToString() + "%";
    }

    public int GetDeadCount()
    {
        return this.deadCount;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("Vibration", this.isVibrator ? 1 : 0);
        PlayerPrefs.SetFloat("Speed", dialogSpeed);
        PlayerPrefs.SetFloat("BGVol", this.masterVolume);
    }

    public void SetMute()
    {
        this.bgVolume = this.bgPlayer.volume;
        this.efVolume = this.efPlayer.volume;

        this.bgPlayer.volume = 0;
        this.efPlayer.volume = 0;
    }

    public void SetUnMute()
    {
        this.bgPlayer.volume = this.bgVolume;
        this.efPlayer.volume = this.efVolume;
    }
}