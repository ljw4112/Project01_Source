using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFindingClue : MonoBehaviour
{
    public Button[] buttons;
    public Image[] images;
    public Text txtTimer;
    public Text alarmboxText;
    public Text[] clues;
    public RectTransform alarmBox;
    public int deadline;

    private Coroutine timer;
    private int checker;
    private float fixedTime;
    private float time;
    private bool isClear;
    private bool isEnd;
    private bool isHalf;

    public void Init(float time)
    {
        this.time = time;
        this.fixedTime = this.time;
        this.checker = 0;
        for(int i = 0; i < buttons.Length; i++)
        {
            var x = i;
            this.buttons[x].onClick.AddListener(() =>
            {
                this.checker++;
                this.buttons[x].enabled = false;
                this.clues[x].gameObject.SetActive(true);
                InGame.Instance.onPlayClickSound();
            });
        }
        this.timer = StartCoroutine(this.Timer());
    }

    private void Update()
    {
        if (!this.isEnd)
        {
            this.time -= Time.deltaTime;
            this.txtTimer.text = this.time.ToString("00.00");
            if (this.checker >= this.buttons.Length)
            {
                this.isClear = true;
                this.isEnd = true;
                StopCoroutine(this.timer);
            }
        }

        if(!this.isHalf && this.fixedTime / 2 >= this.time)
        {
            this.isHalf = true;
            this.alarmboxText.text = "뭔가 찾을 게 있지 않을까?";
            this.alarmBox.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
            {
                StartCoroutine(this.ReturnAlarmBox());
            };
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(this.fixedTime);
        this.isClear = false;
        this.isEnd = true;
    }

    private IEnumerator ReturnAlarmBox()
    {
        yield return new WaitForSeconds(1.5f);
        this.alarmBox.DOAnchorPos(new Vector2(0, 1450), 0.85f, true).SetEase(Ease.InOutBack);
    }

    public bool GetClear()
    {
        return this.isClear;
    }

    public bool GetIsEnd()
    {
        return this.isEnd;
    }
}