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

    //Player�� ��ֹ� Cube Two�� �ε����� ��, �Ҹ����� �ϱ�
    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();
        
        //��ǥ�� �����Ͽ� ������ Ʋ���ִ� ��, 2021-07-21
        //audio.PlayClipAtPoint();

        //����1 ���
        //audio.PlayOneShot(audioClip);

        //����2 ���
        //audio.PlayOneShot(audioClip2);

        //����1�� ����2�� ��� ���� �� �� �� ������ ���̸� ���� lifeTime�� ����
        //float lifeTime = Mathf.Max(audioClip.length, audioClip2.length);

        //���̾��Űâ�� ���ӿ�����Ʈ �ı�(0.25�� ��)
        //���⼭ gameObject�� TestSound.cs ������Ʈ�� �����ִ� ���̾��Űâ�� ���ӿ�����Ʈ
        //����1�� ����2�� ������ �� �� ���� �����ð����� ����� �� Cube Two�� �ı�
        //GameObject.Destroy(gameObject, lifeTime);

        i++;

        //¦���� �� ���
        if (i % 2 == 0)
        {
            Managers.Sound.Play(audioClip, Define.Sound.Bgm);
        }        

        //Ȧ���� �� ���
        else
        {
            Managers.Sound.Play(audioClip2, Define.Sound.Bgm);
        }
        
    }
}