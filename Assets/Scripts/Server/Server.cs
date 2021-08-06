using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Server : MonoBehaviour
{
    private static Server instance;
    public static Server Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Server>();
            }
            return instance;
        }
    }
    public bool coroutineEnd;
    private string host = "http://ec2-3-35-205-27.ap-northeast-2.compute.amazonaws.com";
    private int port = 3000;

    private Server() { }


    public void StartChapterDownload(int num, Action<int, string> onDownload, Action<string, int> onError = null)
    {
        StartCoroutine(this.GetChapterData(num, onDownload, onError));
    }

    public void StartUserInfoDownload(string userid, Action<string> callback)
    {
        StartCoroutine(this.GetUserInfo(userid, callback));
    }

    public void StartUploadUserInfo(UserInfo user, Action<string> callback)
    {
        StartCoroutine(this.SaveUserInfo(user, callback));
    }

    public void StartDownloadEndingData(Action<string> callback)
    {
        StartCoroutine(this.GetEndingData(callback));
    }

    public void StartDownloadCharData(Action<string> callback)
    {
        StartCoroutine(this.GetCharData(callback));
    }

    public void StartDownloadThumbData(Action<string> callback)
    {
        StartCoroutine(this.GetThumbData(callback));
    }

    public void StartDonloadPercentageEnding(string endingNum, Action<int> callback)
    {
        StartCoroutine(this.GetEndingPercentage(endingNum, callback));
    }

    public void StartGetPurchaseData(string userId, Action<string> callback)
    {
        StartCoroutine(this.GetPurchaseData(userId, callback));
    }

    public void StartPostPurchaseData(PurchaseData data, Action<string> callback)
    {
        StartCoroutine(this.PostPurchaseData(data, callback));
    }

    private IEnumerator SaveUserInfo(UserInfo user, Action<string> callback)
    {
        string data = JsonConvert.SerializeObject(user);
        using(UnityWebRequest request = UnityWebRequest.Post(host + ":3000/user", data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                if(callback != null)
                {
                    callback("Error Occured, code: " + request.downloadHandler.text);
                }
            }
            else
            {
                if(callback != null)
                {
                    callback("저장이 완료되었습니다.");
                }
            }
        }
    }

    private IEnumerator GetChapterData(int dialogNum, Action<int, string> onDownload, Action<string, int> onError = null)
    {
        UnityWebRequest request;
        string tmpNum = dialogNum.ToString();
        if(dialogNum < 100)
        {
            tmpNum = tmpNum.PadLeft(3, '0');
        }

        using (request = UnityWebRequest.Get(host + ":" + port + "/chapter/" + tmpNum))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                if(onError != null)
                {
                    onError(request.error, (int)request.responseCode);
                }
            }
            else
            {
                onDownload(dialogNum, request.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetUserInfo(string userid, Action<string> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/user/" + userid))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {

                callback(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetEndingData(Action<string> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/chapter/ending"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetCharData(Action<string> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/chapter/char"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetThumbData(Action<string> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/chapter/thumb"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetEndingPercentage(string endingNum, Action<int> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/user/endingper/" + endingNum))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                float percentage = Convert.ToSingle(request.downloadHandler.text);
                callback(Convert.ToInt32(percentage));
            }
        }
    }

    private IEnumerator GetPurchaseData(string userId, Action<string> callback)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(host + ":" + port + "/purchase/" + userId))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator PostPurchaseData(PurchaseData data, Action<string> callback)
    {
        string sendingData = JsonConvert.SerializeObject(data);
        using (UnityWebRequest request = UnityWebRequest.Post(host + ":3000/purchase", sendingData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(sendingData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (callback != null)
                {
                    callback("Error Occured, code: " + request.downloadHandler.text);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("구매가 완료되었습니다.");
                }
            }
        }
    }
}
