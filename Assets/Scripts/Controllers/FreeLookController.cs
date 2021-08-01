using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FreeLookController))]
public class FreeLookController : MonoBehaviour
{
    Cinemachine.CinemachineFreeLook freeLookCam = new Cinemachine.CinemachineFreeLook();

    Define.CinemaChineMode mode = Define.CinemaChineMode.FreeLook;

    [SerializeField]    
    Transform _player = null;

    public void SetPlayer(Transform player2)
    {
        _player = player2;
    }

    public void Start()
    {        
        Transform _player = transform.Find("ClazyRunner");

        //이 밑줄부터는 발자취5 부분으로 이동 시킴
        //GameObject gameobject : GameObject를 변수명 gameobject로 인스턴스화 한 후,
        //GameObject.Find("@Managers") : 게임 오브젝트 찾기(@Manager란 이름을 가진)
        //= :  변수명 gameobject로 인스턴스화 한 코드에 넣기, 2021-06-30
        //_player = GameObject.Find("ClazyRunner");

        freeLookCam.Follow = _player;
        freeLookCam.LookAt = _player;
    }
    public void LateUpdate()
    {
        if (mode == Define.CinemaChineMode.FreeLook)
        {
            //Player가 죽었을 때, 오류가 뜨게 하는 것을 방지, 2021-07-29
            //null 체크 || Player 가 꺼진 상태인지 판별, 2021-07-30
            //if (_player.IsValid() == false)
            //{
            //    return;
            //}

            //GameObject FreeLookCamera = GameObject.Find("FreeLookCamera");

            
            
        }
    }
}
