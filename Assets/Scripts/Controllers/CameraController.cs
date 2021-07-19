using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //2021-07-04
    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuaterView;

    [SerializeField]
    //_delta : Camera�� Player�� �󸶸�ŭ ������ �ִ���
    //�ʱ�ȭ �������� ������ ����� ������ �߱� ������ �ʱ�ȭ : new Vector3(0.0f, 15.0f, -5.0f); ,2021-07-04
    Vector3 delta = new Vector3(0.0f, 15.0f, -5.0f);

    [SerializeField]
    //�ʱ�ȭ �������� ������ ����� ������ �߱� ������ �ʱ�ȭ : null, 2021-07-04
    GameObject player = null;

    void Update()
    {
       
    }

    void LateUpdate()
    {
        if (mode == Define.CameraMode.QuaterView)
        {
            RaycastHit hit;

            //���� player�� ����ؼ� ���ߴ� camera�� player�� �ƴ� ���̳�, ���� ���߰� �ȴٸ�, 2021-07-05
            //player ��ġ = player.transform.position, camera�� �ִ� ��ġ = delta = new Vector3(0.0f, 15.0f, -5.0f);
            if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Ground")| LayerMask.GetMask("Wall")))
            {
                //wall�� ������ player�� ������ �ʾ�, camera�� player�� ���� ���ϸ�, 
                //player�� wall ������ �Ÿ����� �� �� ��ܼ� player�� ��� ���� camera�� ���� ��ġ ��ų���̴�.
                //ray�� wall�� �ε��� ������������ player������ ���⺤���� ũ�Ⱚ���ٰ� 0.8�踦 �ϸ�, 1�� ���� �����Ƿ� wall�� player ������ 0.8�� �κп� camera�� ��ġ�ϰ� �ȴ�, 2021-07-05
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                //dist�� ray�� ���� wall�� Ư���κп������� player������ �Ÿ����� 0.8�� �� �Ÿ�, 2021-07-05

                //camera�� ���ο� ��ġ = �÷��̾��� ��ġ + ī�޶��� ������ġ * ����ȭ * ray�� ���� wall�� player������ �Ÿ����� 0.8�� �� �Ÿ�, 2021-07-05 
                transform.position = player.transform.position + delta.normalized * dist;
            }

            else
            {
                //ī�޶��� ��ġ�� �÷��̾�� delta��ŭ ������ ���� ��ġ�ϰ� �ϱ�, 2021-07-04
                transform.position = player.transform.position + delta;

                //���� Rotation���� ������� ������ � ��ü�� �ٶ󺸰� �����, 2021-07-04
                transform.LookAt(player.transform);
                //���� �� �̻� player�� ���� ������ ��Ÿ���� �ʴ´�. , 2021-07-04 
            }
        }
    }

    //���ͺ並 �ڵ������ ���� �� ���� ������, 2021-07-04
    public void SetQuaterView(Vector3 _delta)
    {
        mode = Define.CameraMode.QuaterView;
        delta = _delta;
    }
}