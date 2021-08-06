using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIOption : MonoBehaviour
{
    public Action<string> onSuccessProcess;
    public Action<string, bool> onShowMessage;
    public Button btnExit;
    public Button btnVibrator;
    public Button btnChapter;
    public Button btnSave;
    public Button btnLoad;
    public Button btnCredit;
    public Button btnSurvey;
    public Button btnTermsOfService;
    public Button btnOK;
    public Button btnClose;
    public Button btnPopUpClose_credit;
    public Button btnPopUpClose_terms;
    public Slider sliderVolume;
    public Slider sliderGameSpeed;
    public GameObject chapterPopup;
    public GameObject termsOfServicePopup;
    public GameObject creditPopup;
    public GameObject messageBox;

    private bool isVibrator;

    public void Init(bool isVibrator, float speed, float volume)
    {
        //넘어온 값들 초기화
        this.isVibrator = isVibrator;
        this.sliderGameSpeed.value = speed;
        this.sliderVolume.value = volume;

        if (this.isVibrator)
        {
            this.btnVibrator.GetComponent<RectTransform>().anchoredPosition = new Vector2(40.6f, 2.9f);
        }
        else
        {
            this.btnVibrator.GetComponent<RectTransform>().anchoredPosition = new Vector2(-35.8f, 2.9f);
        }

        this.btnExit.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.gameObject.SetActive(false);
            InGame.Instance.onDialogStart();
        });

        this.btnVibrator.onClick.AddListener(() =>
        {
            var rect = this.btnVibrator.GetComponent<RectTransform>();
            if (this.isVibrator)
            {
                rect.DOAnchorPos(new Vector2(-35.8f, 2.9f), 0.2f, true).SetEase(Ease.OutCubic);
            }
            else
            {
                rect.DOAnchorPos(new Vector2(40.6f, 2.9f), 0.2f, true).SetEase(Ease.OutCubic);
            }
            this.isVibrator = !this.isVibrator;
            InGame.Instance.onVibratorPress(this.isVibrator);
        });

        this.btnChapter.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();

            if (DataManager.Instance.GetUser() != null)
            {
                if(DataManager.Instance.GetUser().arrEnding.Count > 0)
                {
                    InGame.Instance.SaveOptions();
                    this.chapterPopup.SetActive(true);
                } else
                {
                    var rect = this.messageBox.GetComponent<RectTransform>();
                    var text = this.messageBox.transform.GetChild(0).GetComponent<Text>();
                    text.text = "지금은 하던 걸 먼저 끝내자";

                    rect.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
                    {
                        this.onSuccessProcess("지금은 하던 걸 먼저 끝내자");
                    };
                }
            } else
            {
                var rect = this.messageBox.GetComponent<RectTransform>();
                var text = this.messageBox.transform.GetChild(0).GetComponent<Text>();
                text.text = "지금은 하던 걸 먼저 끝내자";

                rect.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
                {
                    this.onSuccessProcess("지금은 하던 걸 먼저 끝내자");
                };
            }
        });

        this.btnSurvey.onClick.AddListener(()=> {
            Application.OpenURL("https://forms.gle/iZTswjZr4aorJe6p9");
        });

        this.btnTermsOfService.onClick.AddListener(()=> {
            this.termsOfServicePopup.SetActive(true);

            this.btnPopUpClose_terms.onClick.AddListener(()=> {
                this.termsOfServicePopup.SetActive(false);
            });
        });

        this.btnCredit.onClick.AddListener(() => {
            this.creditPopup.SetActive(true);

            this.btnPopUpClose_credit.onClick.AddListener(() => {
                this.creditPopup.SetActive(false);
            });
        });

        this.btnOK.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            InGame.Instance.onComplete(false);
        });

        this.btnClose.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.chapterPopup.SetActive(false);
            
        });

        this.onShowMessage = (message, process) =>
        {
            var rect = this.messageBox.GetComponent<RectTransform>();
            var text = this.messageBox.transform.GetChild(0).GetComponent<Text>();
            text.text = message;

            if (process)
            {
                rect.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
                {
                    this.onSuccessProcess(message);
                };
            }
            else
            {
                rect.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack);
            }
        };

        this.onSuccessProcess = (message) =>
        {
            var rect = this.messageBox.GetComponent<RectTransform>();
            var text = this.messageBox.transform.GetChild(0).GetComponent<Text>();
            text.text = message;

            StartCoroutine(this.FinishSaveProcess(rect));
        };

    }

    private IEnumerator FinishSaveProcess(RectTransform rect)
    {
        yield return new WaitForSeconds(0.5f);

        rect.DOAnchorPos(new Vector2(0, 1450), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
        {
            if (!this.btnExit.enabled)
            {
                this.btnExit.enabled = true;
            }
        };
    }
}