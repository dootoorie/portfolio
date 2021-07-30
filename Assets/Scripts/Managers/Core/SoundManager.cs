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

    //_audioClips�� ĳ�� ������ �ϰ� �ȴ�.
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

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


    //Clear() �Լ� ������ �ϴ� ����
    //GetOrAddAudioClip() �Լ� ���п�
    //��� ������ ������ �ִ� �� ã�� �ʰ� �ٷ� ��� �´�(������)
    //������ �������δ� _audioClips�� �׻� ����ִٴ� ���̴�.
    //SoundManager�� ���, Managers ���Ͽ� �ִµ�,
    //�� Managers�� DontDestroyOnLoad() �� ���� ������� �ʰ� ���������� �����Ѵ�.
    //�׷��ٴ°�, SoundManager�� Managers ���Ͽ� �ֱ� ������ ���������� �����Ѵ�.
    //���� �ٲ� �� ����,
    //Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); ��
    //�������� �ʰ�, ��� �߰��� �Ǹ�, �������� �޸𸮰� �������� �ִ�...
    //�׷��Ƿ�..!  Clear() �Լ��� �������� �Ѵ�.
    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;

            audioSource.Stop();
        }

        //Unity���� �����ϴ� Dictionary �Լ� ������ Clear �Լ� 
        _audioClips.Clear();
    }

    //<audioClip�� �޴� Play�Լ�>�� �޴� Play�Լ�
    //Define.Sound type : enum�� Sound���� �⺻ ���� Effect��������(Bgm�� �� ������� ���� �� ���Ƽ�)
    //path : ���� ���, pitch : ���� �ӵ�����
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);

        Play(audioClip, type, pitch);
    }


    //audioClip�� �޴� Play�Լ�, 2021-07-21
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        //audioClip�� �ִ��� ������ üũ
        if (audioClip == null)
        {
            return;
        }

        //enum Sound���� Bgm�������� �Ϲ� Effect�������� �˰� �ʹ�
        //���� Bgm�����̸�
        if (type == Define.Sound.Bgm)
        {
            //�迭
            // MP3 Player   -> AudioSource
            // Mp3 ����     -> AudioClip
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            //�ٸ� BGM�� play ���̸�,
            if (audioSource.isPlaying)
            {
                //audioSource�� ����.
                audioSource.Stop();
            }

            //�����ӵ� ����
            audioSource.pitch = pitch;

            // MP3 Player   -> AudioSource
            // Mp3 ����     -> AudioClip
            audioSource.clip = audioClip;

            //���
            audioSource.Play();
        }

        //���� Bgm������ �ƴϸ�
        else
        {
            //�迭
            //�Ϲ� Effect����
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            //�����ӵ� ����
            audioSource.pitch = pitch;

            //���
            audioSource.PlayOneShot(audioClip);
        }
    }



    //BGM�����̰�, Effect�����̰� �� �� ����� �� �ְ� ����, 2021-07-21
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        //Unityâ���� Resources ���� �Ʒ��� Sounds ������ �����.
        //��ΰ� Sounds������ �ƴϸ�
        if (path.Contains("Sound/") == false)
        {
            //��θ� Sounds ������ ����
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        //enum Sound���� Bgm�������� �Ϲ� Effect�������� �˰� �ʹ�
        //���� Bgm�����̸�
        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }

        //���� Bgm������ �ƴϸ�
        else
        {
            //������ �ִ� audioClip�� ã��(ĳ�� ������ �ϴ� _audioClip����)
            //�Ű����� : ���� path�� �ִٰ� ������, �ٷ� ������ ����ֱ� 
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                //���� ������ audioClip�� ã�Ƶ� ������(ĳ�� ������ �ϴ� _audioClip����)
                audioClip = Managers.Resource.Load<AudioClip>(path);

                //ĳ�̿����� �ϴ� _audioClips�� audioClip�� �߰�
                _audioClips.Add(path, audioClip);
            }

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");
            }
        }

        //���� audioClip(= mp3����) �� ������
        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing ! {path}");
        }

        //audioClip�� ����ֱ�
        return audioClip;
    }       
}