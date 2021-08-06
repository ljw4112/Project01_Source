using System.Collections.Generic;
using UnityEngine;

public class UIProfile : MonoBehaviour
{
    public GameObject picPrefab;
    public Transform grid;

    //profileChar: 아래의 그림, profilePic: 위의 그림
    //주인공 스프라이트
    public Sprite profileChar00;
    public Sprite profilePic00;

    //각각의 등장인물 별 인덱스로 구분해서 컷씬 저장
    //(한 인덱스에 4칸짜리 Sprite배열이 들어감.
    List<Sprite[]> subProfiles;

    //보라 스프라이트
    public Sprite profileChar01;
    public Sprite profilePic01;

    //찬훈 스프라이트
    public Sprite profileChar02;
    public Sprite profilePic02;

    //유성 스프라이트
    public Sprite profileChar03;
    public Sprite profilePic03;

    //서율 스프라이트
    public Sprite profileChar04;
    public Sprite profilePic04;

    public void Init(UserInfo user)
    {
        //0: 버튼 일반이미지, 1: 버튼 글리치이미지, 2: 하단 이미지, 3: 하단 글리치이미지)
        //초기화
        this.subProfiles = new List<Sprite[]>();

        //주인공 스프라이트를 배열에 넣고 리스트에 삽입
        Sprite[] main = new Sprite[4];
        main[0] = profilePic00;
        main[2] = profileChar00;
        this.subProfiles.Add(main);

        //보라 스프라이트 배열에 넣고 리스트에 삽입
        Sprite[] bora = new Sprite[4];
        bora[0] = this.profilePic01;
        bora[2] = this.profileChar01;

        //찬훈 스프라이트 배열에 넣고 리스트에 삽입
        Sprite[] ch = new Sprite[4];
        ch[0] = this.profilePic02;
        ch[2] = this.profileChar02;

        //유성 스프라이트 배열에 넣고 리스트에 삽입
        Sprite[] ys = new Sprite[4];
        ys[0] = this.profilePic03;
        ys[2] = this.profileChar03;

        //서율 스프라이트 배열에 넣고 리스트에 삽입
        Sprite[] sy = new Sprite[4];
        sy[0] = this.profilePic04;
        sy[2] = this.profileChar04;

        this.subProfiles.Add(bora);
        this.subProfiles.Add(ch);
        this.subProfiles.Add(ys);
        this.subProfiles.Add(sy);
        this.subProfiles.Add(null);
        this.subProfiles.Add(null);

        //나머지 사람들 생성
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