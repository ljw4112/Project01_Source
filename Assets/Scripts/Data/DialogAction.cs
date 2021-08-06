using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogAction : MonoBehaviour
{
    public Action onComplete;
    public Action onLoadSavePoint;
    public Action onCamera;
    public GameObject spacing;

    private Coroutine volumeCoroutine;
    private bool isFadeOccured;
    private bool isComplete;            //Action이 끝났는가?
    private string[] arrActions;
    [SerializeField]
    private float lerpTime;
    private float prevDialogSpeed;

    private static DialogAction instance;
    public static DialogAction Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogAction>();
            }
            return instance;
        }
    }
    private DialogAction() { }

    public void Divide(string action, GameObject spacing = null)
    {
        if (spacing != null)
        {
            this.spacing = spacing;
        }

        if (action != "null")
        {
            action = action.Replace(" ", "");
            this.StartCoroutine(this.DivideImpl(action));
        }
    }

    private IEnumerator DivideImpl(string action)
    {
        var actions = action.Split(',');
        this.arrActions = actions;

        string[] orders;

        int a = 0;
        this.isComplete = true;
        while (a < actions.Length)
        {
            if (!this.isComplete)
            {
                yield return null;
                continue;
            }

            if (string.IsNullOrEmpty(actions[a]))
            {
                break;
            }
            orders = actions[a].Split('=');
            this.HandleAction(orders[0], orders[1]);
            a++;
            yield return null;
        }
    }

    private void HandleAction(string order, string value)
    {
        this.isComplete = false;

        //한 작업이 끝난 후엔 무조건 this.isComplete = true, this.onComplete()를 해줘야함.
        //액션 구현은 따로 함수를 빼서 구현. case에서는 호출만 수행.
        switch (order)
        {
            case GameConstants.Color:
                {
                    this.SetColor(value);
                }
                break;
            case GameConstants.Sleep:
                {
                    this.StartCoroutine(this.Sleep(Convert.ToSingle(value)));
                }
                break;
            case GameConstants.FadeIn:
                {
                    this.StartCoroutine(this.FadeIn(value));
                }
                break;
            case GameConstants.FadeOut:
                {
                    this.StartCoroutine(this.FadeOut(Convert.ToSingle(value)));
                }
                break;
            case GameConstants.BGAudio:
                {
                    this.SetBGAudio(value);
                }
                break;
            case GameConstants.Refresh:
                {
                    this.Refresh();
                }
                break;
            case GameConstants.BGAudioVolume:
                {
                    this.SetBGAudioVolume(value);
                }
                break;
            case GameConstants.Vibrator:
                {
                    this.Vibrator(value);
                }
                break;
            case GameConstants.CameraFilter:
                {
                    this.SetCameraFilter(value);
                }
                break;
            case GameConstants.Time:
                {
                    this.SetTime(value);
                }
                break;
            case GameConstants.Receive:
                {
                    this.SetReceive(Convert.ToInt32(value));
                }
                break;
            case GameConstants.EffectAudio:
                {
                    this.SetEFAudio(value);
                }
                break;
            case GameConstants.EffectAudioVolume:
                {
                    this.SetEFAudioVolume(value);
                }
                break;
        }
    }

    private void SetBGAudio(string value)
    {
        if(this.volumeCoroutine != null)
        {
            StopCoroutine(this.volumeCoroutine);
        }
        var audio = GameObject.FindGameObjectWithTag("BGAudio").GetComponent<AudioSource>();
        string path = string.Format("Audio/{0}", value);
        var source = Resources.Load<AudioClip>(path);
        audio.clip = source;
        audio.volume = 1;
        audio.Play();
        audio.loop = true;

        foreach (var i in this.arrActions)
        {
            var tmp = i.Split('=');
            if (tmp[0] == "BGAudioVolume")
            {
                //같은 Action에서 볼륨을 조절한다면?
                audio.volume = Convert.ToSingle(tmp[1]) / 100;
                break;
            }
        }

        this.isComplete = true;
        if (this.onComplete != null)
        {
            this.onComplete();
        }
    }

    private void SetEFAudio(string value)
    {
        var audio = GameObject.FindGameObjectWithTag("EFAudio").GetComponent<AudioSource>();
        string path = string.Format("Audio/{0}", value);

        if (audio.isPlaying)
        {
            audio.Stop();
        }

        var source = Resources.Load<AudioClip>(path);
        audio.volume = 1;
        audio.PlayOneShot(source);

        this.isComplete = true;

        if (this.arrActions.Length <= 1 || this.arrActions[this.arrActions.Length - 1].Contains(value))
        {
            this.onComplete();
        }
    }

    private void SetEFAudioVolume(string value)
    {
        var audio = GameObject.FindGameObjectWithTag("EFAudio").GetComponent<AudioSource>();
        audio.volume = Convert.ToSingle(value) / 100;

        this.isComplete = true;
        this.onComplete();
    }

    private void SetBGAudioVolume(string value)
    {
        var audio = GameObject.FindGameObjectWithTag("BGAudio").GetComponent<AudioSource>();

        //같은 곳에서 브금을 틀고 볼륨까지 같이 조절하는지 확인
        bool isVolume = false;
        bool isBg = false;
        foreach(var i in this.arrActions)
        {
            var tmp = i.Split('=');
            if(tmp[0] == "BGAudio")
            {
                isBg = true;
            }

            if(tmp[0] == "BGAudioVolume")
            {
                isVolume = true;
            }
        }

        if(!isBg && isVolume)
        {
            this.volumeCoroutine = this.StartCoroutine(this.SetAudioVolume(audio, Convert.ToSingle(value) / 100));
        }

        this.isComplete = true;
        if (this.onComplete != null)
        {
            this.onComplete();
        }
    }

    private void SetTime(string time)
    {
        var txtTime = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        txtTime.text = time;

        this.isComplete = true;
        this.onComplete();
    }

    private void SetReceive(int data)
    {
        var receive = GameObject.FindGameObjectWithTag("Signal").GetComponent<Transform>();
        var noise = FindObjectOfType<CameraFilterPack_TV_Noise>();
        noise.enabled = true;
        List<GameObject> receives = new List<GameObject>();
        foreach (Transform i in receive)
        {
            receives.Add(i.gameObject);
            i.gameObject.SetActive(false);
        }
        float fadeNum = 0;
        switch (data)
        {
            case 1:
                {
                    receives[0].SetActive(true);
                    fadeNum = 0.75f;
                }
                break;
            case 2:
                {
                    receives[0].SetActive(true);
                    receives[1].SetActive(true);
                    fadeNum = 0.5f;
                }
                break;
            case 3:
                {
                    receives[0].SetActive(true);
                    receives[1].SetActive(true);
                    receives[2].SetActive(true);
                    fadeNum = 0.25f;
                }
                break;
            case 4:
                {
                    receives[0].SetActive(true);
                    receives[1].SetActive(true);
                    receives[2].SetActive(true);
                    receives[3].SetActive(true);
                    fadeNum = 0;
                }
                break;
        }
        noise.Fade = fadeNum;

        InGame.Instance.onPauseAction += () =>
        {
            noise.enabled = false;
        };

        InGame.Instance.onStartAction += () =>
        {
            noise.enabled = true;
        };


        this.isComplete = true;
        this.onComplete();
    }

    private IEnumerator SetAudioVolume(AudioSource audio, float maxVol)
    {
        var originVol = audio.volume;
        while (true)
        {
            if (Mathf.Abs(maxVol - audio.volume) < 0.1f || maxVol == audio.volume)
            {
                audio.volume = maxVol;
                break;
            }

            if (originVol < maxVol)
            {
                audio.volume += Time.fixedDeltaTime * 1.2f;
            }
            else
            {
                audio.volume -= Time.fixedDeltaTime * 1.2f;
            }
            yield return null;
        }

        if (maxVol == 0)
        {
            audio.Stop();
        }
    }

    private void SetCameraFilter(string value)
    {
        var values = value.Split('/');
        var filter = values[0];
        var check = Convert.ToBoolean(values[1]);

        this.StartCoroutine(SetCameraFilterImpl(filter, check));
    }

    private IEnumerator SetCameraFilterImpl(string filter, bool check)
    {
        switch (filter)
        {
            case "Rain":
                {
                    var script = GameObject.FindGameObjectWithTag("UICamera").GetComponent<CameraFilterPack_Atmosphere_Rain_Pro>();

                    script.StormFlashOnOff = 1;
                    script.Distortion = 0.015f;

                    if (check)
                    {
                        script.enabled = check;

                        while (script.Fade < 1)
                        {
                            script.Fade += 0.1f;

                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                    else
                    {
                        while (script.Fade > 0)
                        {
                            script.Fade -= 0.1f;

                            yield return new WaitForSeconds(0.1f);
                        }

                        script.enabled = check;
                    }

                    InGame.Instance.onPauseAction += () =>
                    {
                        script.enabled = false;
                    };

                    InGame.Instance.onStartAction += () =>
                    {
                        script.enabled = true;
                    };

                    isComplete = true;
                    onComplete();
                }
                break;
            case "Broken":
                {
                    var script = GameObject.FindGameObjectWithTag("UICamera").GetComponent<CameraFilterPack_TV_BrokenGlass>();

                    script.Broken_Big = 0;
                    script.Broken_Small = 64;
                    script.Broken_Medium = 64;
                    script.Broken_High = 64;

                    if (check)
                    {
                        script.enabled = check;
                    }
                    else
                    {
                        script.enabled = check;
                    }

                    isComplete = true;
                    onComplete();
                }
                break;
        }
    }

    private void Refresh()
    {
        //Grid 뒤집기
        var grid = GameObject.FindGameObjectWithTag("Grid");
        var verticalLayout = grid.GetComponent<VerticalLayoutGroup>();
        var rect = grid.GetComponent<RectTransform>();
        var child = grid.GetComponent<Transform>();

        foreach (Transform tmp in child)
        {
            Destroy(tmp.gameObject);
        }

        verticalLayout.padding.top = 50;
        verticalLayout.padding.bottom = 0;

        rect.anchorMin = new Vector2(0.5f, 1.0f);
        rect.anchorMax = new Vector2(0.5f, 1.0f);
        rect.pivot = new Vector2(0.5f, 1.0f);

        this.isComplete = true;
        this.onComplete();

        StartCoroutine(this.ReverseGrid(rect, verticalLayout));
    }

    private void SetColor(string value)
    {
        var color = value;
        this.lerpTime = 1f;
        color = color.Insert(0, "#");
        Color tmpColor;
        ColorUtility.TryParseHtmlString(color, out tmpColor);
        var image = GameObject.FindGameObjectWithTag("Dialog").GetComponent<Image>();
        image.CrossFadeColor(tmpColor, this.lerpTime, true, true);
        this.onComplete();
    }

    private void Vibrator(string value)
    {
        var values = value.Split('/');
        var time = Convert.ToSingle(values[0]);
        var num = Convert.ToSingle(values[1]);

        if (InGame.Instance.isVibrator)
        {
            StartCoroutine(this.Vibrator(time, num));
        }
        else
        {
            this.isComplete = true;
            this.onComplete();
        }
    }

    private IEnumerator Vibrator(float time, float num)
    {
        while (num != 0)
        {
            for (int i = 0; i < time; i++)
            {
                Handheld.Vibrate();
                yield return new WaitForSeconds(0.5f);
            }
            num--;
            yield return new WaitForSeconds(0.5f);
        }
        this.isComplete = true;
        this.onComplete();
    }

    private IEnumerator Sleep(float time)
    {
        yield return new WaitForSeconds(time);
        this.isComplete = true;
    }

    private IEnumerator FadeIn(string value)
    {
        var fadeImage = GameObject.FindGameObjectWithTag("Dim").GetComponent<Image>();

        var values = value.Split('/');
        var time = Convert.ToSingle(values[0]);
        var targetColor = values[1];
        Color tmpColor;
        ColorUtility.TryParseHtmlString("#" + targetColor, out tmpColor);
        tmpColor.a = 0;
        fadeImage.color = tmpColor;

        while (tmpColor.a < 1f)
        {
            tmpColor.a += Time.deltaTime / time;
            fadeImage.color = tmpColor;

            if (tmpColor.a >= 1f)
            {
                tmpColor.a = 1f;
            }

            yield return null;
        }

        fadeImage.color = tmpColor;
        this.isComplete = true;

        if (this.arrActions.Length < 2)
        {
            this.prevDialogSpeed = InGame.Instance.dialogSpeed;
            this.onComplete();
            InGame.Instance.dialogSpeed = 1;
            this.isFadeOccured = true;
        }
    }

    private IEnumerator FadeOut(float time)
    {
        var fadeImage = GameObject.FindGameObjectWithTag("Dim").GetComponent<Image>();
        var tmpColor = fadeImage.color;

        while (tmpColor.a > 0f)
        {
            tmpColor.a -= Time.deltaTime / time;
            fadeImage.color = tmpColor;

            if (tmpColor.a <= 0f)
            {
                tmpColor.a = 0f;
            }

            yield return null;
        }

        fadeImage.color = tmpColor;

        this.isComplete = true;
        if (this.isFadeOccured)
        {
            InGame.Instance.dialogSpeed = this.prevDialogSpeed;
            this.isFadeOccured = false;
        }
        this.onComplete();
    }

    private IEnumerator ReverseGrid(RectTransform grid, VerticalLayoutGroup vertical)
    {
        yield return null;

        while (grid.rect.height < Screen.height * 0.8f)
        {
            yield return null;
        }

        grid.anchorMin = new Vector2(0.5f, 0);
        grid.anchorMax = new Vector2(0.5f, 0);
        grid.pivot = new Vector2(0.5f, 0);

        vertical.padding.top = 0;
        vertical.padding.bottom = 50;
    }
}