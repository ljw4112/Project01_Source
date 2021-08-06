using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public Text dialog;
    public void Init(string data)
    {
        this.dialog.text = data;
    }
}
