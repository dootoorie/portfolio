using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigO : MonoBehaviour
{


    public int Add(int N)
    {
        return N + N;
    }


    public int Add2(int N)
    {
        int sum = 0;

        for (int i = 0; i < N; i++)
            sum += i;

        return sum;
    }


    public int Add3(int N)
    {
        int sum = 0;

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                sum += 1;

        return sum;
    }

    public int Add4(int N)
    {
        //Big-O ǥ��� 2�ܰ� : ���常 �����

        //��Ģ1) ������� ���� ū ��ǥ �׸� ����� ����
        //��Ģ2) ��� ����(ex : 2N => N)

        //�켱 sum�̶� ������ ����� 0�� �־��ش�.
        int sum = 0;

        //���� ������ N��
        for (int i = 0; i < N; i++)
            sum += i;

        //����for��. ������ ����Ǵ� Ƚ���� 2N*2N �׷��Ƿ� �� 4N^2
        for (int i = 0; i < 2 * N; i++)
            for (int j = 0; j < 2 * N; j++)
                sum += 1;

        //�׸��� ���簨 �� � ����. 1�� ġ��.
        sum += 1234567;

        return sum;
    }

    //�׷��Ƿ� �����ϸ�,
    //O(1 + N + 4 * N^2 + 1)
    //=O(4 * N^2)               //��Ģ1 ����
    //=O(N^2)                   //��Ģ2 ����

    //����� O�� Order Of��� �д´�.

    void Start()
    {
        

    }

    void Update()
    {
        
    }
}
