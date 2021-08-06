using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEnding : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject endingPrefab;
    public Transform grid;
    public RectTransform endingPrefabRect;
    public Button btnBack;
    public Image dim;

    private void Awake()
    {
        this.endingPrefabRect.sizeDelta = new Vector2(Screen.width, Screen.height / 1.2f);
        this.btnBack.onClick.AddListener(() =>
        {
            this.dim.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }

    public void Init(UserInfo user, int chapterNum)
    {
        foreach(Transform i in this.grid)
        {
            Destroy(i.gameObject);
        }

        var endingData = DataManager.Instance.arrEndingData;
        var userData = user.arrEnding.Keys.ToArray();
        this.DisplayLoadingMessage(false, "로딩중입니다...");

        switch (chapterNum)
        {
            case 0:
                {
                    foreach (var i in endingData)
                    {
                        foreach(var x in userData)
                        {
                            var data = Convert.ToInt32(x.Replace("e", ""));
                            if (i.Key / 100 == 0 && i.Key == data)
                            {
                                Server.Instance.StartDonloadPercentageEnding(x, (result) =>
                                {
                                    this.DisplayLoadingMessage(true, "로딩중입니다...");
                                    this.dim.gameObject.SetActive(false);
                                    var endGo = Instantiate(this.endingPrefab, this.grid);
                                    endGo.GetComponent<UIEndingComp>().Init(user, data, i.Value.endingName, result,  i.Value.thumbnailName);
                                });
                            }
                        }
                    }
                }
                break;
            case 1:
                {
                    foreach(var i in endingData)
                    {
                        foreach (var x in userData)
                        {
                            var data = Convert.ToInt32(x.Replace("e", ""));
                            if (i.Key / 100 == 1 && i.Key == data)
                            {
                                Server.Instance.StartDonloadPercentageEnding(x, (result) =>
                                {
                                    this.DisplayLoadingMessage(true, "로딩중입니다...");
                                    var endGo = Instantiate(this.endingPrefab, this.grid);
                                    this.dim.gameObject.SetActive(false);
                                    endGo.GetComponent<UIEndingComp>().Init(user, data, i.Value.endingName, result, i.Value.thumbnailName);
                                });
                            }
                        }
                    }
                }
                break;
            case 2:
                {
                    foreach (var i in endingData)
                    {
                        foreach (var x in userData)
                        {
                            var data = Convert.ToInt32(x.Replace("e", ""));
                            if (i.Key / 100 == 2 && i.Key == data)
                            {
                                Server.Instance.StartDonloadPercentageEnding(x, (result) =>
                                {
                                    this.DisplayLoadingMessage(true, "로딩중입니다...");
                                    this.dim.gameObject.SetActive(false);
                                    var endGo = Instantiate(this.endingPrefab, this.grid);
                                    endGo.GetComponent<UIEndingComp>().Init(user, data, i.Value.endingName, result, i.Value.thumbnailName);
                                });
                            }
                        }
                    }
                }
                break;
            case 3:
                {

                }
                break;
            case 4:
                {

                }
                break;
        }
    }

    private void DisplayLoadingMessage(bool status, string message)
    {
        var rect = this.messageBox.GetComponent<RectTransform>();
        var text = this.messageBox.transform.GetChild(0).GetComponent<Text>();

        //status -> 0: 열기 / 1: 닫기
        if (!status)
        {
            text.text = message;
            rect.DOAnchorPos(new Vector2(0, 600), 0.85f, true).SetEase(Ease.InOutBack);
        }
        else
        {
            rect.DOAnchorPos(new Vector2(0, 1450), 0.85f, true).SetEase(Ease.InOutBack);
        }
    }
}