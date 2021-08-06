using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITimeAttackZone : MonoBehaviour
{
    public Text timer;
    public GameObject panel;

    private float seconds;
    private bool isStart;
    private bool isOver;

    public void Init(float seconds)
    {
        this.panel = this.gameObject;
        this.panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 718), 0.3f, true).SetEase(Ease.OutQuart);
        this.seconds = seconds;
        this.isStart = true;
        this.isOver = false;
    }

    public void End()
    {
        this.panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1110), 0.3f, true).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isStart)
        {
            this.seconds -= Time.deltaTime;
            timer.text = seconds.ToString("00.00");
        }

        if(this.seconds <= 0)
        {
            this.seconds = 0;
            this.isOver = true;
            return;
        }
    }

    public void SetIsStart(bool value)
    {
        this.isStart = value;
    }

    public bool GetIsOver()
    {
        return this.isOver;
    }
}
