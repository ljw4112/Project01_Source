using UnityEngine;
using UnityEngine.UI;

public class UIProfileElement : MonoBehaviour
{
    public Image image;
    public Button btn;
    public void Init(int index, eStatus status, Sprite[] sprites)
    {
        var detailImg = GameObject.FindGameObjectWithTag("Profile");

        if (Screen.height > 1920)
        {
            detailImg.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        if (sprites == null)
        {
            return;
        }
        this.btn.onClick.AddListener(() =>
        {
            InGame.Instance.onPlayClickSound();
            //해당 버튼을 클릭했을 때 인덱스에 맞는 데이터를 보여준다.
            switch (index)
            {
                case 0:
                    {
                        detailImg.GetComponent<Image>().sprite = sprites[2];
                    }
                    break;
                case 1:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
                case 2:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
                case 3:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
                case 4:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
                case 5:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
                case 6:
                    {
                        if (status == eStatus.DEAD)
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[3];
                        }
                        else
                        {
                            detailImg.GetComponent<Image>().sprite = sprites[2];
                        }
                    }
                    break;
            }
            detailImg.GetComponent<Image>().SetNativeSize();

            if(Screen.height > 1920)
            {
                detailImg.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        });
    }
}
