using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndingComp : MonoBehaviour
{
    public Text txtEndingName;
    public Text txtEndingPercentage;
    public Image imgEndingThumbnail;
    public Button btn;

    public void Init(UserInfo user, int endingNum, string endingName, int percent, string thumbnailName)
    {
        this.txtEndingName.text = endingName;
        this.txtEndingPercentage.text = string.Format("엔딩 통계 : {0}%", percent);
        this.imgEndingThumbnail.sprite = InGame.Instance.endingThumbnail.GetSprite(thumbnailName); ;

        this.btn.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            var prevUI = GameObject.Find("ReplayVisible").transform.GetChild(0).GetComponent<UIEndingPrev>();
            prevUI.Init(user, endingNum);
        });
    }
}
