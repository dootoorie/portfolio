using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    //2021-07-04
    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuaterView;

    [SerializeField]
    //_delta : Camera와 Player가 얼마만큼 떨어져 있는지
    //초기화 시켜주지 않으면 노란색 오류가 뜨기 때문에 초기화 : new Vector3(0.0f, 15.0f, -5.0f); ,2021-07-04
    Vector3 delta = new Vector3(0.0f, 15.0f, -5.0f);

    [SerializeField]
    //초기화 시켜주지 않으면 노란색 오류가 뜨기 때문에 초기화 : null, 2021-07-04
    GameObject _player = null;

    //2021-07-30
    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    void LateUpdate()
    {
        if (mode == Define.CameraMode.QuaterView)
        {
            //Player가 죽었을 때, 오류가 뜨게 하는 것을 방지, 2021-07-29
            //null 체크 || Player 가 꺼진 상태인지 판별, 2021-07-30
            if (_player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;

            //만약 player를 계속해서 비추는 camera가 player가 아닌 땅이나, 벽을 비추게 된다면, 2021-07-05
            //player 위치 = player.transform.position, camera가 있는 위치 = delta = new Vector3(0.0f, 15.0f, -5.0f);
            if (Physics.Raycast(_player.transform.position, delta, out hit, delta.magnitude,
                LayerMask.GetMask("Ground")         //Layer 선택 방법1
                | LayerMask.GetMask("Wall")         //Layer 선택 방법1
                | 1 << (int)Define.Layer.Block))    //Layer 선택 방법2
            {
                //wall에 가려서 player가 보이지 않아, camera가 player를 찍지 못하면, 
                //player와 wall 사이의 거리보다 좀 더 당겨서 player를 찍기 위해 camera를 새로 위치 시킬것이다.
                //ray가 wall에 부딪힌 지점에서부터 player까지의 방향벡터의 크기값에다가 0.8배를 하면, 1배 보다 작으므로 wall과 player 사이의 0.8배 부분에 camera가 위치하게 된다, 2021-07-05
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                //dist는 ray가 맞은 wall의 특정부분에서부터 player까지의 거리에서 0.8배 한 거리, 2021-07-05

                //camera의 새로운 위치 = 플레이어의 위치 + 카메라의 원래위치 * 정규화 * ray가 맞은 wall과 player사이의 거리에서 0.8배 한 거리, 2021-07-05 
                transform.position = _player.transform.position + delta.normalized * dist;
            }

            else
            {
                //카메라의 위치를 플레이어에서 delta만큼 떨어진 곳에 위치하게 하기, 2021-07-04
                transform.position = _player.transform.position + delta;

                //현재 Rotation값에 관계없이 무조건 어떤 객체를 바라보게 만들기, 2021-07-04
                transform.LookAt(_player.transform);
                //이제 더 이상 player의 떨림 현상은 나타나지 않는다. , 2021-07-04 
            }
        }
    }

    //쿼터뷰를 코드상으로 세팅 할 수도 있으니, 2021-07-04
    public void SetQuaterView(Vector3 _delta)
    {
        mode = Define.CameraMode.QuaterView;
        delta = _delta;
    }
}