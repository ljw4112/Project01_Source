using System.Collections.Generic;
using UnityEngine;

public class UIProfile : MonoBehaviour
{
    public GameObject picPrefab;
    public Transform grid;

    //profileChar: �Ʒ��� �׸�, profilePic: ���� �׸�
    //���ΰ� ��������Ʈ
    public Sprite profileChar00;
    public Sprite profilePic00;

    //������ �����ι� �� �ε����� �����ؼ� �ƾ� ����
    //(�� �ε����� 4ĭ¥�� Sprite�迭�� ��.
    List<Sprite[]> subProfiles;

    //���� ��������Ʈ
    public Sprite profileChar01;
    public Sprite profilePic01;

    //���� ��������Ʈ
    public Sprite profileChar02;
    public Sprite profilePic02;

    //���� ��������Ʈ
    public Sprite profileChar03;
    public Sprite profilePic03;

    //���� ��������Ʈ
    public Sprite profileChar04;
    public Sprite profilePic04;

    public void Init(UserInfo user)
    {
        //0: ��ư �Ϲ��̹���, 1: ��ư �۸�ġ�̹���, 2: �ϴ� �̹���, 3: �ϴ� �۸�ġ�̹���)
        //�ʱ�ȭ
        this.subProfiles = new List<Sprite[]>();

        //���ΰ� ��������Ʈ�� �迭�� �ְ� ����Ʈ�� ����
        Sprite[] main = new Sprite[4];
        main[0] = profilePic00;
        main[2] = profileChar00;
        this.subProfiles.Add(main);

        //���� ��������Ʈ �迭�� �ְ� ����Ʈ�� ����
        Sprite[] bora = new Sprite[4];
        bora[0] = this.profilePic01;
        bora[2] = this.profileChar01;

        //���� ��������Ʈ �迭�� �ְ� ����Ʈ�� ����
        Sprite[] ch = new Sprite[4];
        ch[0] = this.profilePic02;
        ch[2] = this.profileChar02;

        //���� ��������Ʈ �迭�� �ְ� ����Ʈ�� ����
        Sprite[] ys = new Sprite[4];
        ys[0] = this.profilePic03;
        ys[2] = this.profileChar03;

        //���� ��������Ʈ �迭�� �ְ� ����Ʈ�� ����
        Sprite[] sy = new Sprite[4];
        sy[0] = this.profilePic04;
        sy[2] = this.profileChar04;

        this.subProfiles.Add(bora);
        this.subProfiles.Add(ch);
        this.subProfiles.Add(ys);
        this.subProfiles.Add(sy);
        this.subProfiles.Add(null);
        this.subProfiles.Add(null);

        //������ ����� ����
        for (int i = 0; i <= 6; i++)
        {
            int x = i;
            var subGo = Instantiate(this.picPrefab, this.grid);
            var element = subGo.GetComponent<UIProfileElement>();
            if (x == 0 || (user != null && user.profileData.ContainsKey(x)))
            {
                if (x == 0 || user.profileData[x] == (int)eStatus.CLEAR)
                {
                    if (this.subProfiles[x] != null)
                    {
                        element.image.sprite = this.subProfiles[x][0];
                    }
                }
                else if (user.profileData[x] == (int)eStatus.DEAD)
                {
                    if (this.subProfiles[x] != null)
                    {
                        element.image.sprite = this.subProfiles[x][1];
                    }
                }

                if (x != 0)
                {
                    element.Init(x, (eStatus)user.profileData[x], this.subProfiles[x]);
                }
                else
                {
                    element.Init(x, eStatus.CLEAR, this.subProfiles[x]);
                }
            }
        }
    }
}