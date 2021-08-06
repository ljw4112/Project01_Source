using UnityEngine;
using UnityEngine.UI;

public class UINarration : MonoBehaviour
{
    public Text text;
    public Image image;
    public void Init(string s, bool validBackground)
    {
        this.text.text = s;
        if (!validBackground)
        {
            Color c = image.color;
            c.a = 0;
            image.color = c;
        }
    }
}