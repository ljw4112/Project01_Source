using UnityEngine;
using UnityEngine.UI;

public class UITimeAttack : MonoBehaviour
{
    public Text ques01;
    public Text ques02;
    public Text txtTimer;
    public Button btn01;
    public Button btn02;

    private bool isOver;
    private bool isClear;
    private float time;
    private float timer = 10f;
    private int result;

    private void Update()
    {
        if (!this.isOver)
        {
            timer -= Time.deltaTime;
            this.txtTimer.text = this.timer.ToString("00.00");
            if (timer <= 0)
            {
                this.isOver = true;
                return;
            }
        }
    }

    public void Init(string s1, string s2, float time)
    {
        this.ques01.text = s1;
        if (string.IsNullOrEmpty(s2))
        {
            Destroy(btn02.gameObject);
        }
        else
        {
            this.ques02.text = s2;
        }
        this.time = time;
        this.btn01.onClick.AddListener(() =>
        {
            if (!this.isOver)
            {
                this.result = 0;
                this.isClear = true;
            }
        });
        if (string.IsNullOrEmpty(s2))
        {
            this.btn02.onClick.AddListener(() =>
            {
                if (!this.isOver)
                {
                    this.result = 1;
                    this.isClear = true;
                }
            });
        }
    }

    public bool GetIsOver()
    {
        return this.isOver;
    }

    public int GetResult()
    {
        return this.result;
    }

    public bool GetIsClear()
    {
        return this.isClear;
    }
}
