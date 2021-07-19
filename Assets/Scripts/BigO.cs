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
        //Big-O 표기법 2단계 : 대장만 남긴다

        //규칙1) 영향력이 가장 큰 대표 항목만 남기고 삭제
        //규칙2) 상수 무시(ex : 2N => N)

        //우선 sum이란 변수를 만들고 0을 넣어준다.
        int sum = 0;

        //덧셈 연산을 N번
        for (int i = 0; i < N; i++)
            sum += i;

        //이중for문. 덧셈이 실행되는 횟수는 2N*2N 그러므로 총 4N^2
        for (int i = 0; i < 2 * N; i++)
            for (int j = 0; j < 2 * N; j++)
                sum += 1;

        //그리고 존재감 모를 어떤 덧셈. 1로 치자.
        sum += 1234567;

        return sum;
    }

    //그러므로 정리하면,
    //O(1 + N + 4 * N^2 + 1)
    //=O(4 * N^2)               //규칙1 적용
    //=O(N^2)                   //규칙2 적용

    //참고로 O는 Order Of라고 읽는다.

    void Start()
    {
        

    }

    void Update()
    {
        
    }
}
