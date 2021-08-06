using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRecord : MonoBehaviour
{
    public Button prologue;
    public Button chapter1;
    public Button chapter2;
    public Button chapter3;
    public Button mainChapter;
    public Sprite prologueLock;
    public Sprite prologueUnlock;
    public Sprite chapter1Lock;
    public Sprite chapter1Unlock;
    public Sprite chapter2Lock;
    public Sprite chapter2Unlock;
    public Sprite chapter3Lock;
    public Sprite chapter3Unlock;
    public Sprite chapterMainLock;
    public Sprite chapterMainUnlock;
    public GameObject endingPage;

    private bool prologueClear;
    private bool chapter1Clear;
    private bool chapter2Clear;
    private bool chapter3Clear;
    private bool chapterMainClear;
    public void Init(UserInfo user)
    {
        prologue.enabled = false;
        chapter1.enabled = false;
        chapter2.enabled = false;
        chapter3.enabled = false;
        mainChapter.enabled = false;

        if(user != null)
        {
            var chapterData = user.arrEnding;
            //각 챕터당 클리어 루트가 단 하나라도 있는지 체크
            foreach (var i in chapterData)
            {
                int chapter = Convert.ToInt32(i.Key.Replace("e", "")) / 100;
                switch (chapter)
                {
                    case 0:
                        {
                            this.prologueClear = true;
                            this.prologue.enabled = true;
                        }
                        break;
                    case 1:
                        {
                            this.chapter1Clear = true;
                            chapter1.enabled = true;
                        }
                        break;
                    case 2:
                        {
                            this.chapter2Clear = true;
                            chapter2.enabled = true;
                        }
                        break;
                    case 3:
                        {
                            this.chapter3Clear = true;
                            chapter3.enabled = true;
                        }
                        break;
                    case 4:
                        {
                            this.chapterMainClear = true;
                            mainChapter.enabled = true;
                        }
                        break;
                }
            }
        }
        else
        {
            this.chapter1Clear = false;
            this.chapter2Clear = false;
            this.chapter3Clear = false;
            this.chapterMainClear = false;
        }

        //버튼 이벤트 붙이기
        this.prologue.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.endingPage.SetActive(true);
            this.endingPage.GetComponent<UIEnding>().Init(user, 0);
        });

        this.chapter1.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.endingPage.SetActive(true);
            this.endingPage.GetComponent<UIEnding>().Init(user, 1);
        });

        this.chapter2.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.endingPage.SetActive(true);
            this.endingPage.GetComponent<UIEnding>().Init(user, 2);
        });

        this.chapter3.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.endingPage.SetActive(true);
            this.endingPage.GetComponent<UIEnding>().Init(user, 3);
        });

        this.mainChapter.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            this.endingPage.SetActive(true);
            this.endingPage.GetComponent<UIEnding>().Init(user, 4);
        });

        //이미지 출력
        if (this.prologueClear)
        {
            this.prologue.gameObject.GetComponent<Image>().sprite = this.prologueUnlock;
        }
        //if (this.chapter1Clear)
        //{
        //    this.chapter1.gameObject.GetComponent<Image>().sprite = this.chapter1Unlock;
        //}
        if (this.chapter2Clear)
        {
            this.chapter2.gameObject.GetComponent<Image>().sprite = this.chapter2Unlock;
        }
        //if (this.chapter3Clear)
        //{
        //    this.chapter3.gameObject.GetComponent<Image>().sprite = this.chapter3Unlock;
        //}
        //if (this.chapterMainClear)
        //{
        //    this.mainChapter.gameObject.GetComponent<Image>().sprite = this.chapterMainUnlock;
        //}
    }
}
