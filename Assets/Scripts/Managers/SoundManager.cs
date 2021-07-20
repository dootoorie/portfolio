using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-20
public class SoundManager //MonoBehaviour ����
{
    //Unity���� �����ϴ� �⺻ Ŭ���� AudioSource.
    //���⼭ MaxCount�� enum������ �Ϻη� �� ������ ���� MaxCount�� ���Ƿ� �߰��� ��
    //1��(����)�� �ƴ� �ټ�(�迭)�� ����� ���� : BGM���� 1��, �Ϲ� Effect���� 1��
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // MP3 Player   -> AudioSource
    // Mp3 ����     -> AudioClip
    // ����(��)     -> AudioListener

    public void Init()
    {
        //@Sound��� �� ���ӿ�����Ʈ�� ã�ƺ���
        GameObject root = GameObject.Find("@Sound");

        //���� ��ã������
        if (root == null)
        {
            //@Sound ��� �̸����� �� ���ӿ�����Ʈ�� ���������
            root = new GameObject { name = "@Sound" };

            Object.DontDestroyOnLoad(root);

            //���� �̸� ����
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            //Length - 1 �� ���� : enum���� �� ������ ���� MaxCount�� ���Ƿ� �߰������� -1�� ���ش�. 
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                //���ο� ���ӿ�����Ʈ �����
                GameObject go = new GameObject { name = soundNames[i] };

                //���θ��� ���ӿ�����Ʈ�� ������Ʈ AudioSource �ٿ��� �迭�� ����
                _audioSources[i] = go.AddComponent<AudioSource>();

                //parent�� ���� (root.transform�� �θ�� ����)
                //UI�� ��� : UI�� ���� RectTransform�̶� SetParent�� ����ߴ� ����.
                //�Ϲ����� ��� : transform.parent �ϸ� ��
                go.transform.parent = root.transform;
            }

            //�迭
            //�Ϲ� Effect ������ �ƴ� Bgm���� ���� ���, loop(=���ѹݺ�)�� ���ش�.
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }


    //Define.Sound type : enum Sound
    //path : ���� ���, pitch : ���� �ӵ�����
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        //Unityâ���� Resources ���� �Ʒ��� Sounds ������ �����.
        //��ΰ� Sounds������ �ƴϸ�
        if (path.Contains("Sound/") == false)
        {
            //��θ� Sounds ������ ����
            path = $"Sounds/{path}";
        }

        //enum Sound���� Bgm�������� �Ϲ� Effect�������� �˰� �ʹ�
        //���� Bgm�����̸�
        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            //���� audioClip(= mp3����) �� ������
            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");

                return;
            }

            //�迭
            //BGM����
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            //�����ӵ� ����
            audioSource.pitch = pitch;

            //���
            audioSource.Play();
        }

        //���� Bgm������ �ƴϸ�
        else
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");

                return;
            }

            //�迭
            //�Ϲ� Effect����
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            //�����ӵ� ����
            audioSource.pitch = pitch;

            //���
            audioSource.PlayOneShot(audioClip);
        }
    }
}