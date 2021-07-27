using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-27

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;              //���콺 Ŀ�� Attack ������ �̹���, 2021-07-26
    Texture2D _handIcon;                //���콺 Ŀ�� Hand ������ �̹���, 2021-07-26


    //���콺 Ŀ�� �̹����� �ڲ� �����Ÿ��� ���װ� �߻��Ͽ� enum������ �����Ͽ� ���� ��ĥ ��, 2021-07-26
    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    //���콺 Ŀ�� �̹��� �ʱⰪ�� None, 2021-07-26
    CursorType _cursorType = CursorType.None;

    void Start()
    {
        //���콺 Ŀ�� Attack �̹��� �ε�, 2021-07-26
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");

        //���콺 Ŀ�� Hnad �̹��� �ε�, 2021-07-26
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        //���콺�� ���� ���¶��, � ������ε�, ���콺 Ŀ�� �������� �ٲٰ� �ؼ��� �ȵȴ�, 2021-07-26
        //���콺�� ������. 2021-07-26
        if (Input.GetMouseButton(0))
        {
            //���콺�� �����ٸ�(= true���) return, 2021-07-26
            return;
            //return���� ����, ���콺�� ���� �� ������ �ִ� ���¿��� ���콺�� �̵� ��Ű�ٰ�
            //���콺�� Enemy ���� ��������, Į ��� ���콺 Ŀ���� �ٲ��� �ʴ´�, 2021-07-26
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //Unityâ�� �ٷ� �����ؼ� Unity�� Layer�̸��� �����ͼ� ���� ��, 2021-07-26
        //LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Monster");
        //Define.cs�� Layer enum�� ���� �������� �����ͼ� Layer��ȣ�� �̿��Ͽ� Layer�� �����ϴ� ��, 2021-07-26
        LayerMask _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

        RaycastHit hit;

        //ray�� ���� mask������ �ִ� ���� ������, 2021-07-05
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            // Q) ray�� ��ü�� ��� ���� �����ߴٰ� �ϸ�, Ground����, Monster���� ��� �˱�?
            // � ��ü�� ray�� ���� ��Ҵ��� �� �� ������ �ȴ�.

            //���� ���� Ŭ���� ��ü�� ���Ͷ�� ,2021-07-26
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                //���콺 Ŀ�� �̹��� �����Ÿ��� ���׸� ��ġ�� ���ؼ� enum������ ���콺 �̹��� ������ �����Ͽ�, �̹��� ���� �ּ�ȭ, 2021-07-26
                //���� ����Ű�� ���콺 �̹��� Ŀ���� ���� ��� ���ŵǴ� ���� �ƴ϶�, ��ҿ��� ���ŵ��� ������, Enemy���� ���콺�� �ø� ���� ���ŵȴ�, 2021-07-26
                //�̷��� �ϸ� ���콺 �̹��� Ŀ���� �����Ÿ��� ��������, 2021-07-26
                if (_cursorType != CursorType.Attack)
                {
                    //Ŀ�� ������ �ڽ��� (0, 0)�� Ŀ�� ������ Į�ڷ� �� �κ��� ��ǥ�� �ٸ��Ƿ� �������� ����
                    //Ŀ�� ������ �ڽ��� x���� 5���� 1���� =  Ŀ�� ������ Į�ڷ� �� �κ�. �׷��Ƿ� 5���� 1�������� Ŀ�� ���κ��� ���Ѵ�, 2021-07-26
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);

                    //���콺 Ŀ�� �̹����� Ÿ���� Attack����, 2021-07-26
                    _cursorType = CursorType.Attack;
                }

            }

            //���� ���� Ŭ���� ��ü�� ���Ͱ� �ƴ϶��, 2021-07-26
            else
            {
                //���콺 Ŀ�� �̹��� �����Ÿ��� ���׸� ��ġ�� ���ؼ� enum������ ���콺 �̹��� ������ �����Ͽ�, �̹��� ���� �ּ�ȭ, 2021-07-26
                //���� ����Ű�� ���콺 �̹��� Ŀ���� ���� ��� ���ŵǴ� ���� �ƴ϶�, ��ҿ��� ���ŵ��� ������, Enemy���� ���콺�� �ø� ���� ���ŵȴ�, 2021-07-26
                //�̷��� �ϸ� ���콺 �̹��� Ŀ���� �����Ÿ��� ��������, 2021-07-26
                if (_cursorType != CursorType.Hand)
                {
                    //Ŀ�� ������ �ڽ��� (0, 0)�� Ŀ�� ������ �հ��� �� �κ��� ��ǥ�� �ٸ��Ƿ� �������� ����
                    //Ŀ�� ������ �ڽ��� x���� 3���� 1���� =  Ŀ�� ������ �հ��� �� �κ�. �׷��Ƿ� 3���� 1�������� Ŀ�� ���κ��� ���Ѵ�, 2021-07-26
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);

                    //���콺 Ŀ�� �̹����� Ÿ���� Attack����, 2021-07-26
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
