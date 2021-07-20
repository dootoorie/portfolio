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


    //Define.Sound type : enum Sound
    //path : 음원 경로, pitch : 음원 속도조절
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        //Unity창에서 Resources 폴더 아래에 Sounds 폴더를 만든다.
        //경로가 Sounds폴더가 아니면
        if (path.Contains("Sound/") == false)
        {
            //경로를 Sounds 폴더로 지정
            path = $"Sounds/{path}";
        }

        //enum Sound에서 Bgm음원인지 일반 Effect음원인지 알고 싶다
        //만약 Bgm음원이면
        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            //만약 audioClip(= mp3음원) 이 없으면
            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");

                return;
            }

            //배열
            //BGM음원
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            //음원속도 조절
            audioSource.pitch = pitch;

            //재생
            audioSource.Play();
        }

        //만약 Bgm음원이 아니면
        else
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");

                return;
            }

            //배열
            //일반 Effect음원
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];

            //음원속도 조절
            audioSource.pitch = pitch;

            //재생
            audioSource.PlayOneShot(audioClip);
        }
    }
}