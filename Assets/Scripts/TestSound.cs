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

    //Player�� ��ֹ� Cube Two�� �ε����� ��, �Ҹ����� �ϱ�
    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();

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

        //���� �̸����� �÷���
        //Managers.Sound.Play(Define.Sound.Effect, "Sang_Kuem");
        //Managers.Sound.Play(Define.Sound.Effect, "Waves_Select");
    }
}