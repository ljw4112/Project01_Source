using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIAlbumImage : MonoBehaviour
{
    public Button image;
    public GameObject albumImageDetail;

    public void Init(int chapterNum, string sprite)
    { 
        if (sprite != null)
        {
            switch (chapterNum)
            {
                case 0:
                    {
                        image.gameObject.GetComponent<Image>().sprite = InGame.Instance.prologueAtlas.GetSprite(sprite);
                    }
                    break;
                case 2:
                    {
                        image.gameObject.GetComponent<Image>().sprite = InGame.Instance.chapter2_Atlas.GetSprite(sprite);
                    }
                    break;
            }
        }

        this.image.onClick.AddListener(() => {
            InGame.Instance.onPlayClickSound();
            var imageDetail = Instantiate(albumImageDetail, this.GetComponentInParent<UIAlbum>().GetComponent<Transform>());
            imageDetail.GetComponent<UIAlbumImageDetail>().Init(chapterNum, sprite);
            
        });
    }
}