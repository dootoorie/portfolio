using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJECTION : MonoBehaviour
{
    //Local : �������� �������� Ư�� ��ü�� �������� �� ��ǥ
    //World : Ư�� ��ü�� �ƴϰ� ������ ���� �߽����� �ϳ��� ����� ��ǥ
        
    //3D ���� ��ü�� 3D ��ǥ�� ����� ������, �츮�� �ٶ󺸴� ȭ���� 2D ��ǥ�̴�.
    //�ᱹ ���������δ� 3D ��ǥ�� 2D�� ��ȯ�ؾ� �Ѵ�.

    //���� : Local <-> World <-> Viewport <-> Screen(ȭ��)

    void Update()
    {
        //Screen ��ǥ��(ȭ�� �ػ� ũ�� ��ǥ��)
        //Debug.Log(Input.mousePosition);

        //Viewport ��ǥ��(ȭ�� �ػ� ũ�⸦ 0 ~ 1���� ������ ������ ǥ��ȯ ��ǥ��)
        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));

        //if(Input.GetMouseButtonDown(0))
        //{
        //    //Camera.main���� ���� ī�޶� ��������, Screen��ǥ�迡�� World��ǥ��� �Ű��ִ� ScreenToWorldPoint�� ����.
        //    //Vector3 ��ǥ��� �Ű�������(���콺�� Ŭ����x��, ���콺�� Ŭ���� y��, Clipping Plane�� near�簢�� �κ�)
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        //    //���⺤�� = �������� ��ġ - ���� ��ġ
        //    //�̰��� �����ϸ�,
        //    //���⺤�� dir = ī�޶� ����ִ� Clipping plane(Ŭ���� ���)�� near plane(����� ���)�� ��ġ ��ǥ - ���� ����ִ� ī�޶��� ��ġ ��ǥ
        //    Vector3 dir = mousePos - Camera.main.transform.position;

        //    //����� (5, 0, 0) ���� ���� ���´ٰ� ġ��, ���⺤�͸� ���ϴ� ���̱� ������ (1, 0, 0)�÷� ���������Ѵ�. �׷��Ƿ� ������� ����ȭ normalized�� ���ش�.
        //    dir = dir.normalized;


        //    Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);

        //    RaycastHit hit;

        //    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
        //    {
        //        Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
        //    }
        //}


        //�� ������ ���

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
            }
        }
    }
}
