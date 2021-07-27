using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-27

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;              //마우스 커서 Attack 아이콘 이미지, 2021-07-26
    Texture2D _handIcon;                //마우스 커서 Hand 아이콘 이미지, 2021-07-26


    //마우스 커서 이미지가 자꾸 깜빡거리는 버그가 발생하여 enum값으로 설정하여 버그 고칠 것, 2021-07-26
    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    //마우스 커서 이미지 초기값은 None, 2021-07-26
    CursorType _cursorType = CursorType.None;

    void Start()
    {
        //마우스 커서 Attack 이미지 로드, 2021-07-26
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");

        //마우스 커서 Hnad 이미지 로드, 2021-07-26
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        //마우스를 누른 상태라면, 어떤 방식으로든, 마우스 커서 아이콘을 바꾸게 해서는 안된다, 2021-07-26
        //마우스를 누르면. 2021-07-26
        if (Input.GetMouseButton(0))
        {
            //마우스를 눌렀다면(= true라면) return, 2021-07-26
            return;
            //return으로 인해, 마우스로 땅을 꾹 누르고 있는 상태에서 마우스를 이동 시키다가
            //마우스를 Enemy 위로 지나가도, 칼 모양 마우스 커서로 바뀌지 않는다, 2021-07-26
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //Unity창에 바로 접근해서 Unity의 Layer이름을 가져와서 쓰는 것, 2021-07-26
        //LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster");
        //Define.cs의 Layer enum에 적힌 변수들을 가져와서 Layer번호를 이용하여 Layer를 설정하는 것, 2021-07-26
        LayerMask _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

        RaycastHit hit;

        //ray를 쏴서 mask변수에 있는 값에 닿으면, 2021-07-05
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            // Q) ray가 객체에 닿는 것을 성공했다고 하면, Ground인지, Monster인지 어떻게 알까?
            // 어떤 객체가 ray에 먼저 닿았는지 알 수 있으면 된다.

            //만약 내가 클릭한 객체가 몬스터라면 ,2021-07-26
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                //마우스 커서 이미지 깜빡거리는 버그를 고치기 위해서 enum값으로 마우스 이미지 값들을 설정하여, 이미지 갱신 최소화, 2021-07-26
                //땅을 가리키는 마우스 이미지 커서는 이제 계속 갱신되는 것이 아니라, 평소에는 갱신되지 않지만, Enemy위에 마우스를 올릴 때만 갱신된다, 2021-07-26
                //이렇게 하면 마우스 이미지 커서의 깜빡거림이 없어진다, 2021-07-26
                if (_cursorType != CursorType.Attack)
                {
                    //커서 아이콘 박스의 (0, 0)과 커서 아이콘 칼자루 끝 부분의 좌표가 다르므로 수동으로 설정
                    //커서 아이콘 박스의 x축을 5분의 1지점 =  커서 아이콘 칼자루 끝 부분. 그러므로 5분의 1지점으로 커서 끝부분을 정한다, 2021-07-26
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);

                    //마우스 커서 이미지의 타입을 Attack으로, 2021-07-26
                    _cursorType = CursorType.Attack;
                }

            }

            //만약 내가 클릭한 객체가 몬스터가 아니라면, 2021-07-26
            else
            {
                //마우스 커서 이미지 깜빡거리는 버그를 고치기 위해서 enum값으로 마우스 이미지 값들을 설정하여, 이미지 갱신 최소화, 2021-07-26
                //땅을 가리키는 마우스 이미지 커서는 이제 계속 갱신되는 것이 아니라, 평소에는 갱신되지 않지만, Enemy위에 마우스를 올릴 때만 갱신된다, 2021-07-26
                //이렇게 하면 마우스 이미지 커서의 깜빡거림이 없어진다, 2021-07-26
                if (_cursorType != CursorType.Hand)
                {
                    //커서 아이콘 박스의 (0, 0)과 커서 아이콘 손가락 끝 부분의 좌표가 다르므로 수동으로 설정
                    //커서 아이콘 박스의 x축을 3분의 1지점 =  커서 아이콘 손가락 끝 부분. 그러므로 3분의 1지점으로 커서 끝부분을 정한다, 2021-07-26
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);

                    //마우스 커서 이미지의 타입을 Attack으로, 2021-07-26
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
