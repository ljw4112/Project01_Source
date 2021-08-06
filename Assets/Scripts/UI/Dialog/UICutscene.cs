using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class UICutscene : MonoBehaviour
{
    public Image image;

    public void Init(string spriteName, int chapterNum)
    {
        if (spriteName != null)
        {
            if (chapterNum == 0)
            {
                if (spriteName.Length < 4)
                {
                    spriteName = "0" + spriteName;
                }
                image.sprite = InGame.Instance.prologueAtlas.GetSprite(spriteName);
            }
            else if (chapterNum == 1)
            {
                //image.sprite = InGame.Instance.chapter2Atlas.GetSprite(spriteName);
            }
            else if (chapterNum == 2)
            {
                this.image.sprite = InGame.Instance.chapter2_Atlas.GetSprite(spriteName);
            }

            DataManager.Instance.SaveCutScene(spriteName);
            InGame.Instance.RefreshAlbum();
        }
    }
}