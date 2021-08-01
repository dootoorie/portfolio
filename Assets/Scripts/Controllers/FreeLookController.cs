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

        //�� ���ٺ��ʹ� ������5 �κ����� �̵� ��Ŵ
        //GameObject gameobject : GameObject�� ������ gameobject�� �ν��Ͻ�ȭ �� ��,
        //GameObject.Find("@Managers") : ���� ������Ʈ ã��(@Manager�� �̸��� ����)
        //= :  ������ gameobject�� �ν��Ͻ�ȭ �� �ڵ忡 �ֱ�, 2021-06-30
        //_player = GameObject.Find("ClazyRunner");

        freeLookCam.Follow = _player;
        freeLookCam.LookAt = _player;
    }
    public void LateUpdate()
    {
        if (mode == Define.CinemaChineMode.FreeLook)
        {
            //Player�� �׾��� ��, ������ �߰� �ϴ� ���� ����, 2021-07-29
            //null üũ || Player �� ���� �������� �Ǻ�, 2021-07-30
            //if (_player.IsValid() == false)
            //{
            //    return;
            //}

            //GameObject FreeLookCamera = GameObject.Find("FreeLookCamera");

            
            
        }
    }
}
