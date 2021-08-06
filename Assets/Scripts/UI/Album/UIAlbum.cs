using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIAlbum : MonoBehaviour
{
    public GameObject cutScenePrevPrefab;
    public RectTransform album;

    public Transform prologueGrid;
    public Transform chapter1Grid;
    public Transform chapter2Grid;
    public Transform chapter3Grid;
    public Transform mainChapterGrid;

    public Sprite lockImg;

    public UserInfo user;

    private List<string> prologueCutScene;
    private List<string> chapter1CutScene;
    private List<string> chapter2CutScene;
    private List<string> chapter3CutScene;
    private List<string> mainChapterCutScene;

    private void Awake()
    {
        if (Screen.height > 1920)
        {
            this.album.sizeDelta = new Vector2(Screen.width, Screen.height / 1.4f);
        }

        this.LoadCutScene();
        this.LoadImage();
    }

    public void Init(UserInfo user)
    {
        if (user != null)
        {
            foreach (var i in prologueCutScene)
            {
                InitImpl(user, i);
            }

            foreach (var i in chapter2CutScene)
            {
                InitImpl(user, i);
            }
        } else
        {
            for(int i = 0; i < prologueCutScene.Count; i++)
            {
                this.prologueGrid.transform.GetChild(i).GetComponent<Image>().sprite = this.lockImg;
                this.prologueGrid.transform.GetChild(i).GetComponent<Button>().enabled = false;
            }

            for (int i = 0; i < chapter2CutScene.Count; i++)
            {
                this.chapter2Grid.transform.GetChild(i).GetComponent<Image>().sprite = this.lockImg;
                this.chapter2Grid.transform.GetChild(i).GetComponent<Button>().enabled = false;
            }
        }
    }

    private void InitImpl(UserInfo user, string cutSceneName)
    {
        var cutSceneData = user.arrCutScene;
        int chapter = Int32.Parse(cutSceneName[0].ToString());

        switch (chapter)
        {
            case 0:
                {
                    int index = this.prologueCutScene.FindIndex(x => x == cutSceneName);

                    if (cutSceneData.ContainsKey(cutSceneName))
                    {
                        this.prologueGrid.transform.GetChild(index).GetComponent<Image>().sprite = InGame.Instance.prologueAtlas.GetSprite(cutSceneName);
                        this.prologueGrid.transform.GetChild(index).GetComponent<Button>().enabled = true;
                    }
                    else
                    {
                        this.prologueGrid.transform.GetChild(index).GetComponent<Image>().sprite = this.lockImg;
                        this.prologueGrid.transform.GetChild(index).GetComponent<Button>().enabled = false;
                    }
                }
                break;
            case 2:
                {
                    int index = this.chapter2CutScene.FindIndex(x => x == cutSceneName);

                    if (cutSceneData.ContainsKey(cutSceneName))
                    {
                        this.chapter2Grid.transform.GetChild(index).GetComponent<Image>().sprite = InGame.Instance.chapter2_Atlas.GetSprite(cutSceneName);
                        this.chapter2Grid.transform.GetChild(index).GetComponent<Button>().enabled = true;
                    }
                    else
                    {
                        this.chapter2Grid.transform.GetChild(index).GetComponent<Image>().sprite = this.lockImg;
                        this.chapter2Grid.transform.GetChild(index).GetComponent<Button>().enabled = false;
                    }
                }
                break;
        }
    }

    private void LoadCutScene() {
        this.prologueCutScene = new List<string>();
        this.chapter2CutScene = new List<string>();

        this.prologueCutScene.Add("01");
        this.prologueCutScene.Add("02");
        this.prologueCutScene.Add("03");
        this.prologueCutScene.Add("05");
        this.prologueCutScene.Add("06");
        this.prologueCutScene.Add("07");
        this.prologueCutScene.Add("08");
        this.prologueCutScene.Add("09");
        this.prologueCutScene.Add("010");
        this.prologueCutScene.Add("011");
        this.prologueCutScene.Add("012");
        this.prologueCutScene.Add("014");
        this.prologueCutScene.Add("015");
        this.prologueCutScene.Add("018");

        this.chapter2CutScene.Add("20");
        this.chapter2CutScene.Add("21");
        this.chapter2CutScene.Add("22");
        this.chapter2CutScene.Add("23");
        this.chapter2CutScene.Add("24");
        this.chapter2CutScene.Add("25");
        this.chapter2CutScene.Add("26");
        this.chapter2CutScene.Add("29");
        this.chapter2CutScene.Add("220");
        this.chapter2CutScene.Add("221");
        this.chapter2CutScene.Add("226");
        this.chapter2CutScene.Add("228");
        this.chapter2CutScene.Add("231");
        this.chapter2CutScene.Add("235");
        this.chapter2CutScene.Add("237");
    }

    private void LoadImage()
    {
        //ƒ∆æ¿ ±Ê¿Ã∏∏≈≠ ¿ÃπÃ¡ˆ πÃ∏Æ ∑ŒµÂ
        for (int i = 0; i < this.prologueCutScene.Count; i++)
        {
            this.CreateCutScene(this.prologueGrid, 0, this.prologueCutScene[i]);
        }

        for(int i = 0; i < this.chapter2CutScene.Count; i++)
        {
            this.CreateCutScene(this.chapter2Grid, 2, this.chapter2CutScene[i]);
        }
    }

    private void CreateCutScene(Transform grid, int chapterNum, string spritename)
    {
        //ƒ∆æ¿ «¡∏Æ∆’ ¡¶¿€
        var cutGo = Instantiate(this.cutScenePrevPrefab, grid);
        cutGo.GetComponent<UIAlbumImage>().Init(chapterNum, spritename);
    }
}