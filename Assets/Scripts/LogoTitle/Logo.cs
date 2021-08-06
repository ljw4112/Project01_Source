using System;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Logo : MonoBehaviour
{
    public Image image;
    public Text txtProgress;
    public Text txtErrorLog;
    public Text txtUserID;
    public Action<bool> onComplete;
    public Action<int, string> onDownload;
    public Action<string, int> onError;
    public Slider slider;


    private App app;
    private int prologue_num = 12;
    private int chapter1_num = 31;
    private int chapter2_num = 14;

    private bool chapter0_DC;   //다운로드 완료를 체크하는 변수
    private bool chapter1_DC;
    private bool chatper2_DC;
    private bool chapter3_DC;
    private bool userData_DC;
    private bool endingData_DC;
    private bool charData_DC;
    private bool thumbData_DC;
    private bool purchaseData_DC;

    public void Init()
    {
        this.app = FindObjectOfType<App>();
        if (this.app.userID != null)
        {
            txtUserID.text = this.app.userID;
        }

        if (this.app.status == App.eStatus.BUILD)
        {
            if (!string.IsNullOrEmpty(this.app.userID))
            {
                txtUserID.text = txtUserID.text + '\n' + "Google Firebase connected successfully";
            }
        }
        else
        {
            txtUserID.text = txtUserID.text + '\n' + "MongoDB local environment connected successfully";
        }

        this.slider.maxValue = this.prologue_num + this.chapter1_num + this.chapter2_num + 5;
        this.slider.gameObject.SetActive(false);
        this.txtProgress.gameObject.SetActive(false);
        StartCoroutine(this.FadeImage());
    }

    private IEnumerator FadeImage()
    {
        for (float i = 1; i >= 0; i -= 0.05f)
        {
            var color = this.image.color;
            color.a = i;
            this.image.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(this.DownloadData());
    }

    public IEnumerator DownloadData()
    {
        int downloadStatus = default;
        this.slider.gameObject.SetActive(true);
        this.txtProgress.gameObject.SetActive(true);
        DataManager.Instance.Init();

        //다운로드가 끝났는지를 판별하기위한 bool형 배열
        bool[] chapter0DLComplete = new bool[DataManager.Instance.arrChapter0.Count];
        bool[] chapter1DLComplete = new bool[DataManager.Instance.arrChapter1.Count];
        bool[] chapter2DLComplete = new bool[DataManager.Instance.arrChapter2.Count];

        this.onDownload = (num, value) =>
        {
            switch (num / 100)
            {
                case 0:
                    {
                        DataManager.Instance.arrChapter0[num - 1] = JsonConvert.DeserializeObject<Prologue[]>(value).ToDictionary(x => x.id);
                        chapter0DLComplete[num - 1] = true;
                    }
                    break;
                case 1:
                    {
                        DataManager.Instance.arrChapter1[num - 100] = JsonConvert.DeserializeObject<Prologue[]>(value).ToDictionary(x => x.id);
                        chapter1DLComplete[num - 100] = true;
                    }
                    break;
                case 2:
                    {
                        DataManager.Instance.arrChapter2[num - 200] = JsonConvert.DeserializeObject<Prologue[]>(value).ToDictionary(x => x.id);
                        chapter2DLComplete[num - 200] = true;
                    }
                    break;
            }
            this.slider.value++;
            int percentage = (int)(this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        };

        this.onError = (error, res) =>
        {
            this.txtProgress.text = "Failed to download files. / CODE: " + res;
            this.txtErrorLog.text = "ERROR: " + error;
            downloadStatus = -1;
        };

        Server.Instance.StartGetPurchaseData(this.app.userID, (result) =>
        {
            this.purchaseData_DC = true;
            if(result == "500")
            {
                DataManager.Instance.Purchase = null;
            }
            else
            {
                DataManager.Instance.Purchase = JsonConvert.DeserializeObject<PurchaseData>(result);
            }
            this.slider.value++;
            int percentage = (int)(this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        });

        for (int i = 0; i < chapter0DLComplete.Length; i++)
        {
            Server.Instance.StartChapterDownload(i + 1, onDownload, onError);
        }

        for (int i = 0; i < chapter1DLComplete.Length; i++)
        {
            Server.Instance.StartChapterDownload(i + 100, onDownload, onError);
        }

        for (int i = 0; i < chapter2DLComplete.Length; i++)
        {
            Server.Instance.StartChapterDownload(i + 200, onDownload, onError);
        }

        Server.Instance.StartUserInfoDownload(this.app.userID, (data) =>
        {
            //기존 로컬데이터가 잔존할 때 덮어씌움.
            var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string tempData;
            if (data == "500")
            {
                //신규유저
                UserInfo newUser = new UserInfo();
                tempData = JsonConvert.SerializeObject(newUser);
                this.userData_DC = true;
            }
            else
            {
                //기존유저
                data = data.Remove(0, 1);
                data = data.Remove(data.Length - 1, 1);
                var existUser = JsonConvert.DeserializeObject<UserInfo>(data);
                existUser.id = this.app.userID;
                File.WriteAllText(path, data);
                this.userData_DC = true;
            }
            int percentage = (int)(++this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        });

        Server.Instance.StartDownloadEndingData((result) =>
        {
            DataManager.Instance.arrEndingData = JsonConvert.DeserializeObject<EndingData[]>(result).ToDictionary(x => x.endingNum);
            this.endingData_DC = true;

            int percentage = (int)(++this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        });

        Server.Instance.StartDownloadCharData((result) =>
        {
            DataManager.Instance.arrCharData = JsonConvert.DeserializeObject<CharData[]>(result).ToDictionary(x => x.id);
            this.charData_DC = true;

            int percentage = (int)(++this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        });

        Server.Instance.StartDownloadThumbData((result) =>
        {
            DataManager.Instance.arrCharacterThumbnail = JsonConvert.DeserializeObject<CharacterThumbnail[]>(result).ToDictionary(x => x.thumbnail_id);
            this.thumbData_DC = true;

            int percentage = (int)(++this.slider.value / this.slider.maxValue * 100);
            this.txtProgress.text = "Data Downloading... " + percentage.ToString() + "%";
        });

        StartCoroutine(this.CheckChapter0Download(chapter0DLComplete));
        StartCoroutine(this.CheckChapter1Download(chapter1DLComplete));
        StartCoroutine(this.CheckChapter2Download(chapter2DLComplete));

        while (true)
        {
            if (downloadStatus < 0)
            {
                yield return new WaitForSeconds(1f);
                this.onComplete(false);
            }

            if (this.userData_DC && this.endingData_DC && this.chapter0_DC && this.chapter1_DC
               && this.chatper2_DC && this.charData_DC && this.thumbData_DC && this.purchaseData_DC)
            {
                break;
            }
            yield return null;
        }

        for (float a = 0; a <= 1; a += 0.03f)
        {
            var color = this.image.color;
            color.a = a;
            this.image.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        this.onComplete(true);
    }

    private IEnumerator CheckChapter0Download(bool[] check)
    {
        bool notComplete = false;
        while (true)
        {
            for (int i = 0; i < check.Length; i++)
            {
                if (!check[i])
                {
                    notComplete = true;
                    break;
                }
            }

            if (notComplete)
            {
                notComplete = false;
            }
            else
            {
                break;
            }
            yield return null;
        }

        this.chapter0_DC = true;
    }

    private IEnumerator CheckChapter1Download(bool[] check)
    {
        bool notComplete = false;
        while (true)
        {
            for (int i = 0; i < check.Length; i++)
            {
                if (!check[i])
                {
                    notComplete = true;
                    break;
                }
            }

            if (notComplete)
            {
                notComplete = false;
            }
            else
            {
                break;
            }
            yield return null;
        }

        this.chapter1_DC = true;
    }

    private IEnumerator CheckChapter2Download(bool[] check)
    {
        bool notComplete = false;
        while (true)
        {
            for (int i = 0; i < check.Length; i++)
            {
                if (!check[i])
                {
                    notComplete = true;
                    break;
                }
            }

            if (notComplete)
            {
                notComplete = false;
            }
            else
            {
                break;
            }
            yield return null;
        }

        this.chatper2_DC = true;
    }
}