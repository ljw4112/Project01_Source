using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public partial class TitleSelect : MonoBehaviour
{
    private void AttachIAPButtonEvent()
    {
        for (int i = 0; i < this.purchaseButtons.Length; i++)
        {
            int x = i;

            this.purchaseButtons[x].onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
            {
                if (DataManager.Instance.Purchase == null)
                {
                    DataManager.Instance.Purchase = new PurchaseData();
                    DataManager.Instance.Purchase.id = this.app.userID;
                    DataManager.Instance.Purchase.chapterName.Add(this.productIds[x]);

                    Debug.Log("===================== Save Started =====================");
                    this.ConncectToServer();
                }
                else
                {
                    DataManager.Instance.Purchase.chapterName.Add(this.productIds[x]);
                    this.ConncectToServer();
                }
                Debug.Log("===================== Purchase Success =====================");
                this.chapterCanvases[x].SetActive(false);
                this.RefreshChapterButton();
            }));

            this.purchaseButtons[x].onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
            {
                Debug.Log("===================== Purchase Failed =====================");
                this.FaildPurchase();
                this.chapterCanvases[x].SetActive(false);
            }));
        }
    }

    private void ConncectToServer()
    {
        Server.Instance.StartPostPurchaseData(DataManager.Instance.Purchase, (result) =>
        {
        });
    }

    private void FaildPurchase()
    {
        this.alarmText.text = "구매에 실패했습니다.";
        this.alarmBox.DOAnchorPos(new Vector2(0, 850), 0.85f, true).SetEase(Ease.InOutBack).onComplete = () =>
        {
            StartCoroutine(this.ReturnAlarmBox());
        };
    }
}
