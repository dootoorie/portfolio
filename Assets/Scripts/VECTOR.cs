using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Vector의 정체(굉장히 묘한 클래스)

//벡터는 2가지로 나눌 수 있다.
//<벡터>
//1. 위치 벡터(position vector)
//2. 방향 벡터(direction vector)

struct MyVector
{
    public float x;
    public float y;
    public float z;



    //             +
    //       +     + 
    //+------------+

    //1. 위치 벡터(A벡터에서 B벡터를 뺀 값)
    //위치 벡터는 위치는 다르지만 방향과 크기가 같은 무수히 많은 벡터들의 대표 벡터이며, 간단히 점(좌표) 하나로 나타낼 수 있다.
    //특히 위치 벡터의 연산은 화살표 그림 없이 (-1,2) + (4,2)라는 대수적 연산만 수행하면 됨.
    //https://m.blog.naver.com/forfriend5/220599558731

    //2. 방향 벡터
    //      2-1. 거리(크기) : magnitude로 구함
    //      2-2. 실제 방향 : normalized로 구함 (=방향벡터)(=단위벡터)


    //           크기           방향
    //  ------------------------ >

    //                      /
    //                     /
    //                    / 
    //                   /  크기
    //                  /
    //                 /
    //               ↙
    //             방향


    //magnitude : 벡터의 크기 반환(Length)
    //transform.position.magnitude;

    //normalized : 벡터의 크기가 1인 값을 반환(magnitude가 1인 값을 반환) ex) (1,0,0), (0,1,0), (0,0,1) ---- '단위벡터' 라고도 한다
    //transform.position.normalized;


    public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }    //제곱 한 다음, 루트를 해준다.

    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    //a벡터에 b벡터를 더하는 함수
    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    //a벡터에 b벡터를 빼는 함수
    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    //a벡터에 b벡터를 곱하는 함수
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
        //특정 좌표에서 시작
        MyVector pos = new MyVector(0f, 10f, 0f);

        //pos 좌표에 y값을 2만큼 추가
        pos += new MyVector(0f, 2f, 0f);


        //나의 위치 좌표
        MyVector me = new MyVector(5f, 0f, 0f);

        //목적지(상대방)의 위치 좌표
        MyVector destination = new MyVector(10f, 0f, 0f);

        //<방향 구하기>
        //목적지(상대방)의 위치 좌표 - 나의 위치 좌표 = 내가 목적지(상대방)으로 가는 방향
        MyVector dir = destination - me;   //(5f, 0f, 0f)

        //어떠한 좌표를 단위벡터(방향벡터)로 바꾸기
        dir = dir.normalized;       //(1f, 0f, 0f)

        MyVector newPos = me + dir * speed;
        
        //Q) 중단점 노란색 화살표가 다음 코드로 넘어가게 하려면? 
        //A) 단축키 F10(다음 프로시저)를 눌리면 된다.


        //Vector3 누른 후 F12
    }
}
