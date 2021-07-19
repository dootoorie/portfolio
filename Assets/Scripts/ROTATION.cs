using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTATION : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;
    float yAngle = 0.0f;

    void Update()
    {
        yAngle += Time.deltaTime * speed;

        //(ù��° ���) ���� ȸ����
        //����Ƽ ������ ã�ƺ���, ���Ϸ��ޱ��� ���, �Ѱ��� ������ �ٲ� �� �ִ°� �ƴ϶�, x, y, z ���� ���θ� ������� �Ѵ�.
        //transform.eulerAngles = new Vector3(0f, _yAngle, 0f);

        //(�ι�° ���)+-delta
        //transform.Rotate(new Vector3(0f, Time.deltaTime * 100f, 0f));

        //Quaternion�� F12 �ؼ� ���� x,y,z,w ������ 4������ �ִ�.
        //�� ������, x,y,z�� ����ϸ� �������̶�� ���� �߻��ϱ� ������ 4��° ���� w�� �߰� �� ��.
        //Quaternion qt = transform.rotation;

        //(����° ���)
        //transform.rotation = Quaternion.Euler(new Vector3(0f, _yAngle, 0f));

        if (Input.GetKey(KeyCode.W))
        {
            //�� ��° �Ű������� 0�̸� ù ��° �Ű������� �а�, 1�̸� �� ��° �Ű������� �д´�. �׷��Ƿ� 0.5�� ������. 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.5f);

            //����Ű�� ���� ������ �ٶ󺸰� �ϱ�
            //������ �ƴ϶� ���� ������(=ĳ���Ͱ� ��� �Ĵٺ��� ������� wŰ�� ������ �������� �ٶ�)
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);

            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);

            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.5f);

            //transform.rotation = Quaternion.LookRotation(Vector3.back);

            //transform.Translate(Vector3.back * Time.deltaTime * _speed);

            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.5f);

            //transform.rotation = Quaternion.LookRotation(Vector3.left);

            //transform.Translate(Vector3.left * Time.deltaTime * _speed);

            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.5f);

            //transform.rotation = Quaternion.LookRotation(Vector3.right);

            //transform.Translate(Vector3.right * Time.deltaTime * _speed);

            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }
}
