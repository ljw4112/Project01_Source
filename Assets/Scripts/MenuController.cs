using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public GameObject dialogPanel;
    public GameObject recordPanel;
    public GameObject galleryPanel;
    public GameObject profilePanel;
    public GameObject bg;
    public Button btnGallery;
    public Button btnDialog;
    public Button btnRecord;
    public Button btnProfile;
    public Sprite focusGallery;
    public Sprite nonFocusGallery;
    public Sprite focusDialog;
    public Sprite nonFocusDialog;
    public Sprite focusRecord;
    public Sprite nonFocusRecord;
    public Sprite focusProfile;
    public Sprite nonFocusProfile;
    public Sprite bottom3icon;
    public Sprite bottom4icon;

    //center는 변하지 않는 값이기에 get만 할 수 있도록 선언.
    private Vector3 center
    {
        get
        {
            return new Vector3(0, 0, 0);
        }
    }
    private RectTransform[] panels;
    private int nowIndex;   //0: Gallery, 1: Dialog, 2: Record, 3: Profile
    private int[] panelsXpos;

    private void Start()
    {
        this.panels = new RectTransform[4];
        this.panels[0] = this.galleryPanel.GetComponent<RectTransform>();
        this.panels[1] = this.dialogPanel.GetComponent<RectTransform>();
        this.panels[2] = this.recordPanel.GetComponent<RectTransform>();
        this.panels[3] = this.profilePanel.GetComponent<RectTransform>();

        for(int i=0; i<this.panels.Length; i++)
        {
            this.panels[i].sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        this.panelsXpos = new int[4];
        this.panelsXpos[0] = (int)this.panels[0].anchoredPosition.x;
        this.panelsXpos[1] = (int)this.panels[1].anchoredPosition.x;
        this.panelsXpos[2] = (int)this.panels[2].anchoredPosition.x;
        this.panelsXpos[3] = (int)this.panels[2].anchoredPosition.x;
    }

    public void Init()
    {
        //신규유저
        if (DataManager.Instance.GetUser() == null)
        {
            this.btnProfile.gameObject.SetActive(false);
            this.bg.GetComponent<Image>().sprite = bottom3icon;
        }
        else
        {
            //프롤로그 진행 X(엔딩이 하나도 없을 경우)
            if (DataManager.Instance.GetUser().arrEnding.Count == 0)
            {
                this.btnProfile.gameObject.SetActive(false);
                this.bg.GetComponent<Image>().sprite = bottom3icon;
            }
            else
            {
                //프롤로그 진행 O
                this.btnProfile.gameObject.SetActive(true);
                this.bg.GetComponent<Image>().sprite = bottom4icon;
            }
        }

        this.nowIndex = 1;
        this.SetButtonSprite();

        //버튼 입력처리
        //앨범 버튼
        this.btnGallery.onClick.AddListener(() =>
        {
            if (InGame.Instance.onDialogStop != null)
            {
                InGame.Instance.onDialogStop();
            }

            if (InGame.Instance.onPauseAction != null)
            {
                InGame.Instance.onPauseAction();
            }

            if (this.nowIndex != 0)
            {
                this.panels[0].DOAnchorPos(this.center, 0.2f, true);
                this.panels[this.nowIndex].DOAnchorPos(new Vector2(this.panelsXpos[0], 0), 0.2f, true);
                nowIndex = 0;
                this.SetButtonSprite();
            }
            InGame.Instance.onPlayClickSound();
            InGame.Instance.SetMute();
        });

        //다이얼로그 버튼
        this.btnDialog.onClick.AddListener(() =>
        {
            InGame.Instance.onDialogStart();

            if (InGame.Instance.onStartAction != null)
            {
                InGame.Instance.onStartAction();
            }

            if (this.nowIndex != 1)
            {
                this.panels[1].DOAnchorPos(this.center, 0.2f, true);
                this.panels[this.nowIndex].DOAnchorPos(new Vector2(this.panelsXpos[this.nowIndex], 0), 0.2f, true);
                nowIndex = 1;
                this.SetButtonSprite();
            }
            InGame.Instance.onPlayClickSound();
            InGame.Instance.SetUnMute();
        });

        //레코드 버튼
        this.btnRecord.onClick.AddListener(() =>
        {
            if (InGame.Instance.onDialogStop != null)
            {
                InGame.Instance.onDialogStop();
            }

            if(InGame.Instance.onPauseAction != null)
            {
                InGame.Instance.onPauseAction();
            }

            if (this.nowIndex != 2)
            {
                this.panels[2].DOAnchorPos(this.center, 0.2f, true);
                this.panels[this.nowIndex].DOAnchorPos(new Vector2(this.panelsXpos[2], 0), 0.2f, true);
                nowIndex = 2;
                this.SetButtonSprite();
            }
            InGame.Instance.onPlayClickSound();
            InGame.Instance.SetMute();
        });

        //프로필 버튼
        this.btnProfile.onClick.AddListener(() =>
        {
            if (InGame.Instance.onDialogStop != null)
            {
                InGame.Instance.onDialogStop();
            }

            if (InGame.Instance.onPauseAction != null)
            {
                InGame.Instance.onPauseAction();
            }

            if (this.nowIndex != 3)
            {
                this.panels[3].DOAnchorPos(this.center, 0.2f, true);
                this.panels[this.nowIndex].DOAnchorPos(new Vector2(this.panelsXpos[3], 0), 0.2f, true);
                nowIndex = 3;
                this.SetButtonSprite();
            }
            InGame.Instance.onPlayClickSound();
            InGame.Instance.SetMute();
        });

        this.btnGallery.enabled = false;
        this.btnDialog.enabled = false;
        this.btnProfile.enabled = false;
        this.btnRecord.enabled = false;
    }

    private void SetButtonSprite()
    {
        switch (this.nowIndex)
        {
            case 0:
                {
                    this.btnGallery.transform.GetChild(0).GetComponent<Image>().sprite = this.focusGallery;
                    this.btnDialog.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusDialog;
                    this.btnRecord.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusRecord;
                    this.btnProfile.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusProfile;
                }
                break;
            case 1:
                {
                    this.btnGallery.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusGallery;
                    this.btnDialog.transform.GetChild(0).GetComponent<Image>().sprite = this.focusDialog;
                    this.btnRecord.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusRecord;
                    this.btnProfile.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusProfile;
                }
                break;
            case 2:
                {
                    this.btnGallery.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusGallery;
                    this.btnDialog.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusDialog;
                    this.btnRecord.transform.GetChild(0).GetComponent<Image>().sprite = this.focusRecord;
                    this.btnProfile.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusProfile;
                }
                break;
            case 3:
                {
                    this.btnGallery.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusGallery;
                    this.btnDialog.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusDialog;
                    this.btnRecord.transform.GetChild(0).GetComponent<Image>().sprite = this.nonFocusRecord;
                    this.btnProfile.transform.GetChild(0).GetComponent<Image>().sprite = this.focusProfile;
                }
                break;
        }
    }
}