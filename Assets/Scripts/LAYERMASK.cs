using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAYERMASK : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            //ù ��° ���
            //8�� Layer�� 1�� ���� ���� mask�� ����
            //���� 7�� Layer�� �߰� �ϰ� ������ |�� �ٿ��ְ� ���� �߰����ش�.
            //int mask = (1 << 8) | (1 << 7);

            //�� ��° ���
            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Ground");

            RaycastHit hit;

            //�߻��� ray�� mask������ ��� 8�� Layer�� ������
            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                //�̸����� ���
                //Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");

                //�±׷� ���
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.tag}");
            }
        }
    }
}
