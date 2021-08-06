using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Text txtVersion;
    public Image image;
    public Image dim;
    public Action onComplete;
    public AudioSource source;
    public AudioClip[] clips;
    public GameObject skipGo;
    public Button btnMove;
    public Button btnCancel;
    public GameObject TermsOfServicePopup;
    public Button btnAgree;

    private bool isAgree;

    public void Init(bool isNew)
    {
        this.isAgree = false;
        this.source.clip = this.clips[0];
        this.source.Play();
        this.source.loop = true;
        this.source.volume = 0.3f;
        this.txtVersion.text = string.Format("ver. {0}   ", Application.version);

        if (isNew)
        {
            this.TermsOfServicePopup.SetActive(true);

            this.btnAgree.onClick.AddListener(() => {
                this.TermsOfServicePopup.SetActive(false);
                StartCoroutine(this.FadeDimImage());
                this.isAgree = true;
            });
        } else
        {
            StartCoroutine(this.FadeDimImage());
            this.isAgree = true;
        }
    }

    private IEnumerator FadeDimImage()
    {
        for (float i = dim.color.a; i >= 0; i -= 0.03f)
        {
            Color a = dim.color;
            a.a = i;
            dim.color = a;
            yield return null;
        }
        StartCoroutine(this.FadeImage());
    }

    private IEnumerator FadeDimImageOut()
    {
        for (float i = dim.color.a; i < 1; i += 0.05f)
        {
            Color a = dim.color;
            a.a = i;
            dim.color = a;
            yield return null;
        }
        this.onComplete();
    }

    private IEnumerator FadeImage()
    {
        while (true)
        {
            for (float i = 0; i < 1; i += 0.02f)
            {
                var color = this.image.color;
                color.a = i;
                this.image.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            for (float i = 1; i >= 0; i -= 0.02f)
            {
                var color = this.image.color;
                color.a = i;
                this.image.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.isAgree == true)
        {
            this.source.PlayOneShot(this.clips[1]);
            StopCoroutine(FadeImage());
            StartCoroutine(FadeDimImageOut());
            return;
        }
    }
}
