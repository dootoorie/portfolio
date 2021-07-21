using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-20
public class TestSound : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [SerializeField]
    AudioClip audioClip;

    [SerializeField]
    AudioClip audioClip2;

    int i = 0;

    //Player와 장애물 Cube Two가 부딪혔을 때, 소리나게 하기
    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();
        
        //좌표를 지정하여 음원을 틀어주는 것, 2021-07-21
        //audio.PlayClipAtPoint();

        //음원1 재생
        //audio.PlayOneShot(audioClip);

        //음원2 재생
        //audio.PlayOneShot(audioClip2);

        //음원1과 음원2의 재생 길이 중 더 긴 음원의 길이를 변수 lifeTime에 저장
        //float lifeTime = Mathf.Max(audioClip.length, audioClip2.length);

        //하이어라키창의 게임오브젝트 파괴(0.25초 후)
        //여기서 gameObject는 TestSound.cs 컴포넌트를 물고있는 하이어라키창의 게임오브젝트
        //음원1과 음원2의 길이중 더 긴 것의 음원시간까지 재생한 후 Cube Two를 파괴
        //GameObject.Destroy(gameObject, lifeTime);

        i++;

        //짝수일 때 재생
        if (i % 2 == 0)
        {
            Managers.Sound.Play(audioClip, Define.Sound.Bgm);
        }        

        //홀수일 때 재생
        else
        {
            Managers.Sound.Play(audioClip2, Define.Sound.Bgm);
        }
        
    }
}