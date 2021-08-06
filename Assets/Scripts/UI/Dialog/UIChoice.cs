using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChoice : MonoBehaviour
{
    public Button btnChoice1;
    public Button btnChoice2;
    public Button btnChoice3; //단서 찾기용 버튼
    public GameObject imgCheck1;
    public GameObject imgCheck2;
    public GameObject imgCheck3; //단서 찾기 완료
    public GameObject popup;
    public Button btnclose;


    private void Start()
    {
        this.Init();
    }

    public void Init() 
    {
        this.btnChoice1.onClick.AddListener(() =>
        {
            this.imgCheck1.gameObject.SetActive(true);

            Debug.Log("1번 단서 찾음");
        });
        this.btnChoice2.onClick.AddListener(() =>
        {
            this.imgCheck2.gameObject.SetActive(true);

            Debug.Log("2번 단서 찾음");
        });
        this.btnChoice3.onClick.AddListener(() =>
        {
            this.imgCheck3.gameObject.SetActive(true);

            Debug.Log("3번 단서 찾음");
        });
        this.btnclose.onClick.AddListener(() =>
        {
            this.popup.gameObject.SetActive(false);
        });

    }
    //void OnHover(bool isOver) 
    //{
        
    //}
}