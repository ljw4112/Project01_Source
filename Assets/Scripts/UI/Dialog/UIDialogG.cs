using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIDialogG : MonoBehaviour
{
    public Text charName;
    public Text dialog;
    public Image thumbnail;

    public void Init(string charName, string dialog, string thumbNail)
    {
        this.charName.text = charName;
        this.dialog.text = dialog;

        if (thumbNail != null || thumbNail != "0")
        {
            this.thumbnail.sprite = InGame.Instance.thumbAtlas.GetSprite(thumbNail);
        }
    }
}