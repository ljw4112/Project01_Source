using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChoice : MonoBehaviour
{
    public Button btnChoice1;
    public Button btnChoice2;
    public Button btnChoice3; //�ܼ� ã��� ��ư
    public GameObject imgCheck1;
    public GameObject imgCheck2;
    public GameObject imgCheck3; //�ܼ� ã�� �Ϸ�
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

            Debug.Log("1�� �ܼ� ã��");
        });
        this.btnChoice2.onClick.AddListener(() =>
        {
            this.imgCheck2.gameObject.SetActive(true);

            Debug.Log("2�� �ܼ� ã��");
        });
        this.btnChoice3.onClick.AddListener(() =>
        {
            this.imgCheck3.gameObject.SetActive(true);

            Debug.Log("3�� �ܼ� ã��");
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