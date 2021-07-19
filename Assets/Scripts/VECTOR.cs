using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Vector�� ��ü(������ ���� Ŭ����)

//���ʹ� 2������ ���� �� �ִ�.
//<����>
//1. ��ġ ����(position vector)
//2. ���� ����(direction vector)

struct MyVector
{
    public float x;
    public float y;
    public float z;



    //             +
    //       +     + 
    //+------------+

    //1. ��ġ ����(A���Ϳ��� B���͸� �� ��)
    //��ġ ���ʹ� ��ġ�� �ٸ����� ����� ũ�Ⱑ ���� ������ ���� ���͵��� ��ǥ �����̸�, ������ ��(��ǥ) �ϳ��� ��Ÿ�� �� �ִ�.
    //Ư�� ��ġ ������ ������ ȭ��ǥ �׸� ���� (-1,2) + (4,2)��� ����� ���길 �����ϸ� ��.
    //https://m.blog.naver.com/forfriend5/220599558731

    //2. ���� ����
    //      2-1. �Ÿ�(ũ��) : magnitude�� ����
    //      2-2. ���� ���� : normalized�� ���� (=���⺤��)(=��������)


    //           ũ��           ����
    //  ------------------------ >

    //                      /
    //                     /
    //                    / 
    //                   /  ũ��
    //                  /
    //                 /
    //               ��
    //             ����


    //magnitude : ������ ũ�� ��ȯ(Length)
    //transform.position.magnitude;

    //normalized : ������ ũ�Ⱑ 1�� ���� ��ȯ(magnitude�� 1�� ���� ��ȯ) ex) (1,0,0), (0,1,0), (0,0,1) ---- '��������' ��� �Ѵ�
    //transform.position.normalized;


    public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }    //���� �� ����, ��Ʈ�� ���ش�.

    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    //a���Ϳ� b���͸� ���ϴ� �Լ�
    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    //a���Ϳ� b���͸� ���� �Լ�
    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    //a���Ϳ� b���͸� ���ϴ� �Լ�
    public static MyVector operator *(MyVector a, float d)
    {
        return new MyVector(a.x * d, a.y * d, a.z * d);
    }
}

public class VECTOR : MonoBehaviour
{
    float speed = 10.0f;


    void Start()
    {
        //Ư�� ��ǥ���� ����
        MyVector pos = new MyVector(0f, 10f, 0f);

        //pos ��ǥ�� y���� 2��ŭ �߰�
        pos += new MyVector(0f, 2f, 0f);


        //���� ��ġ ��ǥ
        MyVector me = new MyVector(5f, 0f, 0f);

        //������(����)�� ��ġ ��ǥ
        MyVector destination = new MyVector(10f, 0f, 0f);

        //<���� ���ϱ�>
        //������(����)�� ��ġ ��ǥ - ���� ��ġ ��ǥ = ���� ������(����)���� ���� ����
        MyVector dir = destination - me;   //(5f, 0f, 0f)

        //��� ��ǥ�� ��������(���⺤��)�� �ٲٱ�
        dir = dir.normalized;       //(1f, 0f, 0f)

        MyVector newPos = me + dir * speed;
        
        //Q) �ߴ��� ����� ȭ��ǥ�� ���� �ڵ�� �Ѿ�� �Ϸ���? 
        //A) ����Ű F10(���� ���ν���)�� ������ �ȴ�.


        //Vector3 ���� �� F12
    }
}
