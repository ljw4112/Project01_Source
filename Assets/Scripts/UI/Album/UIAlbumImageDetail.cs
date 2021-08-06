using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIAlbumImageDetail : MonoBehaviour
{
    public Image image;

    public Button btnClose;

    public void Init(int chapterNum, string sprite)
    {
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        if (sprite != null)
        {
            switch (chapterNum)
            {
                case 0:
                    {
                        image.sprite = InGame.Instance.prologueAtlas.GetSprite(sprite);
                    }
                    break;
                case 2:
                    {
                        image.sprite = InGame.Instance.chapter2_Atlas.GetSprite(sprite);
                    }
                    break;
            }
            
        }

        this.btnClose.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            Destroy(this.GetComponentInParent<UIAlbumImageDetail>().gameObject);
        });
    }
}