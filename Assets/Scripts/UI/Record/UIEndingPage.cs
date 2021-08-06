using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIEndingPage : MonoBehaviour
{
    public Text txtChapterName;
    public Text txtEndingName;

    private CanvasGroup canvas;

    public void Init(int chapterNum, string endingName)
    {
        this.canvas = this.GetComponent<CanvasGroup>();

        if(chapterNum == 0)
        {
            this.txtChapterName.text = "���ѷα�";
        } else if (chapterNum == 1)
        {
            this.txtChapterName.text = "é��1";
        }
        else if (chapterNum == 2)
        {
            this.txtChapterName.text = "���� �̸�";
        }
        else if (chapterNum == 3)
        {
            this.txtChapterName.text = "é��3";
        }
        else if (chapterNum == 4)
        {
            this.txtChapterName.text = "���ʷα�";
        }

        this.txtEndingName.text = endingName;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += 0.03f)
        {
            this.canvas.alpha = i;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        InGame.Instance.onComplete(false);
    }
}
