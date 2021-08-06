using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelect : MonoBehaviour
{
    public GameObject btn1;
    public GameObject btn2;
    public Button Button1;
    public Button Button2;
    public Text t1;
    public Text t2;
    
    public void Init(string s1, string s2)
    {
        this.t1.text = s1;
        this.t2.text = s2;

        if(s2 == "null")
        {
            this.btn2.SetActive(false);
        }
    }
}
