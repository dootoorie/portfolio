using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-20
public class SoundManager //MonoBehaviour 삭제
{
    //Unity에서 제공하는 기본 클래스 AudioSource.
    //여기서 MaxCount는 enum값에서 일부러 맨 마지막 값에 MaxCount를 임의로 추가한 값
    //1개(변수)가 아닌 다수(배열)로 만드는 이유 : BGM음원 1개, 일반 Effect음원 1개
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    //_audioClips가 캐싱 역할을 하게 된다.
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // MP3 Player   -> AudioSource
    // Mp3 음원     -> AudioClip
    // 관객(귀)     -> AudioListener

    public void Init()
    {
        //@Sound라는 빈 게임오브젝트를 찾아보자
        GameObject root = GameObject.Find("@Sound");

        //만약 못찾았으면
        if (root == null)
        {
            //@Sound 라는 이름으로 빈 게임오브젝트를 만들어주자
            root = new GameObject { name = "@Sound" };

            Object.DontDestroyOnLoad(root);

            //사운드 이름 저장
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            //Length - 1 인 이유 : enum에서 맨 마지막 값에 MaxCount를 임의로 추가했으니 -1을 빼준다. 
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                //새로운 게임오브젝트 만들기
                GameObject go = new GameObject { name = soundNames[i] };

                //새로만든 게임오브젝트에 컴포넌트 AudioSource 붙여서 배열에 저장
                _audioSources[i] = go.AddComponent<AudioSource>();

                //parent를 연결 (root.transform를 부모로 저장)
                //UI의 경우 : UI할 때는 RectTransform이라 SetParent를 사용했던 것임.
                //일반적인 경우 : transform.parent 하면 됨
                go.transform.parent = root.transform;
            }

            //배열
            //일반 Effect 음원이 아닌 Bgm음원 같은 경우, loop(=무한반복)을 해준다.
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }


    //Clear() 함수 만들어야 하는 이유
    //GetOrAddAudioClip() 함수 덕분에
    //방금 이전에 만났던 애는 또 찾지 않고 바로 들고 온다(빠르게)
    //하지만 단점으로는 _audioClips로 항상 들고있다는 점이다.
    //SoundManager의 경우, Managers 산하에 있는데,
    //그 Managers는 DontDestroyOnLoad() 로 영영 사라지지 않고 영구적으로 존재한다.
    //그렇다는건, SoundManager도 Managers 산하에 있기 때문에 영구적으로 존재한다.
    //씬이 바뀔 때 마다,
    //Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); 가
    //없어지지 않고, 계속 추가만 되면, 언젠가는 메모리가 터질수도 있다...
    //그러므로..!  Clear() 함수를 만들어줘야 한다.
    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;

            audioSource.Stop();
        }

        //Unity에서 제공하는 Dictionary 함수 산하의 Clear 함수 
        _audioClips.Clear();
    }

    //<audioClip을 받는 Play함수>를 받는 Play함수
    //Define.Sound type : enum의 Sound에서 기본 값을 Effect음원으로(Bgm은 잘 사용하지 않을 거 같아서)
    //path : 음원 경로, pitch : 음원 속도조절
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);

        Play(audioClip, type, pitch);
    }


    //audioClip을 받는 Play함수, 2021-07-21
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        //audioClip이 있는지 없는지 체크
        if (audioClip == null)
        {
            return;
        }

        //enum Sound에서 Bgm음원인지 일반 Effect음원인지 알고 싶다
        //만약 Bgm음원이면
        if (type == Define.Sound.Bgm)
        {
            //배열
            // MP3 Player   -> AudioSource
            // Mp3 음원     -> AudioClip
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            //다른 BGM이 play 중이면,
            if (audioSource.isPlaying)
            {
                //audioSource를 끈다.
                audioSource.Stop();
            }

            //음원속도 조절
            audioSource.pitch = pitch;

            // MP3 Player   -> AudioSource
            // Mp3 음원     -> AudioClip
            audioSource.clip = audioClip;

            //재생
            audioSource.Play();
        }

        //만약 Bgm음원이 아니면
        else
        {
            //배열
            //일반 Effect음원
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            //음원속도 조절
            audioSource.pitch = pitch;

            //재생
            audioSource.PlayOneShot(audioClip);
        }
    }



    //BGM음원이건, Effect음원이건 둘 다 사용할 수 있게 만듬, 2021-07-21
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        //Unity창에서 Resources 폴더 아래에 Sounds 폴더를 만든다.
        //경로가 Sounds폴더가 아니면
        if (path.Contains("Sound/") == false)
        {
            //경로를 Sounds 폴더로 지정
            path = $"Sounds/{path}";
        }

        AudioClip audioClip = null;

        //enum Sound에서 Bgm음원인지 일반 Effect음원인지 알고 싶다
        //만약 Bgm음원이면
        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }

        //만약 Bgm음원이 아니면
        else
        {
            //기존에 있던 audioClip을 찾기(캐싱 역할을 하는 _audioClip에서)
            //매개변수 : 만약 path가 있다고 나오면, 바로 꺼내서 뱉어주기 
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                //만약 기존에 audioClip을 찾아도 없으면(캐싱 역할을 하는 _audioClip에서)
                audioClip = Managers.Resource.Load<AudioClip>(path);

                //캐싱역할을 하는 _audioClips에 audioClip을 추가
                _audioClips.Add(path, audioClip);
            }

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");
            }
        }

        //만약 audioClip(= mp3음원) 이 없으면
        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing ! {path}");
        }

        //audioClip을 뱉어주기
        return audioClip;
    }       
}