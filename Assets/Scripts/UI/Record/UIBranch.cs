using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBranch : MonoBehaviour
{
    public Text text;
    public Button btn;
    public void Init(string s)
    {
        this.text.text = s;
    }
}
