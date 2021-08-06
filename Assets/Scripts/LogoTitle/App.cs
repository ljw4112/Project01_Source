using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;

public class App : MonoBehaviour
{
    public enum eStatus
    {
        TEST, BUILD
    }

    public UpdateChecker updateChecker;
    public GameObject updatePopup;
    public Button btnOk;
    public eStatus status;          //개발상태인지 빌드상태인지
    public string userID;

    void Start()
    {
        this.updatePopup.SetActive(false);
        this.SetResolution();
        this.btnOk.onClick.AddListener(() =>
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL("market://details?id=com.DJS.InChoice");
                Application.Quit();
            }
            else
            {
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.DJS.InChoice");
            }
        });

        if (this.status == eStatus.BUILD)
        {
            //GPGS LOGIN PART
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
            {
                Debug.Log(Application.version);
                if (result == SignInStatus.Success)
                {
                    //FIREBASE LOGIN PART
                    var localUser = (PlayGamesLocalUser)Social.localUser;
                    var googleIdToken = localUser.GetIdToken();

                    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                    Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
                    auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("SignInWithCredentialAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                            return;
                        }

                        FirebaseUser newUser = task.Result;
                        this.userID = newUser.UserId;
                    });
                    this.ChangeScene("Logo");
                }
                else
                {
                    Debug.Log("===================> " + result);
                }
            });
        }
        else
        {
            if (this.userID == string.Empty)
            {
                Debug.LogError("userID is empty!!!");
                return;
            }
            else
            {
                //if (this.updateChecker.CheckVersion())
                //{
                //    this.ChangeScene("Logo");
                //}
                //else
                //{
                //    this.updatePopup.SetActive(true);
                //}
                this.ChangeScene("Logo");
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene(string sceneName, int chapterNum = 0, int keyIndex = 1)
    {
        switch (sceneName)
        {
            case "Logo":
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Logo");
                    ao.completed += (obj) =>
                    {
                        var logo = FindObjectOfType<Logo>();
                        logo.Init();
                        logo.onComplete = (success) =>
                        {
                            if (success)
                            {
                                this.ChangeScene("Title");
                            }
                            else
                            {
                                this.ChangeScene("Logo");
                            }
                        };
                    };
                }
                break;
            case "Title":
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Title");
                    ao.completed += (obj) =>
                    {
                        var title = FindObjectOfType<Title>();
                        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);

                        if (!File.Exists(path))
                        {
                            title.Init(true);
                        }
                        else
                        {
                            title.Init(false);
                        }

                        title.onComplete = () =>
                        {
                            if (File.Exists(path))
                            {
                                var json = File.ReadAllText(path);
                                UserInfo tmpUser = JsonConvert.DeserializeObject<UserInfo>(json);

                                if (tmpUser.saveChapterList.Count != 0)
                                {
                                    title.skipGo.SetActive(true);
                                    title.btnMove.onClick.AddListener(() =>
                                    {
                                        this.ChangeScene("Main", tmpUser.saveChapterList[tmpUser.saveChapterList.Count - 1] / 100, tmpUser.keyIndex);
                                    });

                                    title.btnCancel.onClick.AddListener(() =>
                                    {
                                        if (tmpUser.arrEnding.Count > 0)
                                        {
                                            this.ChangeScene("TitleSelect");
                                        }
                                        else
                                        {
                                            this.ChangeScene("Main", 0);
                                        }
                                    });
                                }
                                else
                                {
                                    this.ChangeScene("TitleSelect");
                                }
                            }
                            else
                            {
                                this.ChangeScene("Main");
                            }
                        };
                    };
                }
                break;
            case "TitleSelect":
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("TitleSelect");
                    ao.completed += (obj) =>
                    {
                        var title = FindObjectOfType<TitleSelect>();
                        title.Init();

                        title.onComplete = (chapNum) =>
                        {
                            this.ChangeScene("Main", chapNum);
                        };
                    };
                }
                break;
            case "Main":
                {
                    AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
                    ao.completed += (obj) =>
                    {
                        var dataManager = FindObjectOfType<DialogScript>();
                        var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
                        if (!File.Exists(path))
                        {
                            dataManager.Init(0, true);
                        }
                        else
                        {
                            if (keyIndex > 1)
                            {
                                dataManager.Init(chapterNum, false, keyIndex);
                            }
                            else
                            {
                                dataManager.Init(chapterNum, false);
                            }
                        }

                        InGame.Instance.onComplete = (isLoad) =>
                        {
                            if (!isLoad)
                            {
                                this.ChangeScene("TitleSelect");
                            }
                            else
                            {
                                var path = string.Format("{0}/user_info.json", Application.persistentDataPath);
                                var json = File.ReadAllText(path);
                                UserInfo tmpUser = JsonConvert.DeserializeObject<UserInfo>(json);
                                this.ChangeScene("Main", tmpUser.saveChapterList[tmpUser.saveChapterList.Count - 1] / 100, tmpUser.keyIndex);
                            }
                        };
                    };
                }
                break;
        }
    }

    public void SetResolution()
    {
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(1080, (int)(((float)deviceHeight / deviceWidth) * 1080), true);
    }
}