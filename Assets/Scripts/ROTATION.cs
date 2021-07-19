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

        //(첫번째 방법) 절대 회전값
        //유니티 문서에 찾아보면, 오일러앵글의 경우, 한개의 각도만 바꿀 수 있는게 아니라, x, y, z 각도 전부를 적어줘야 한다.
        //transform.eulerAngles = new Vector3(0f, _yAngle, 0f);

        //(두번째 방법)+-delta
        //transform.Rotate(new Vector3(0f, Time.deltaTime * 100f, 0f));

        //Quaternion을 F12 해서 보면 x,y,z,w 변수가 4가지나 있다.
        //그 이유는, x,y,z만 사용하면 짐벌락이라는 것이 발생하기 때문에 4번째 변수 w를 추가 한 것.
        //Quaternion qt = transform.rotation;

        //(세번째 방법)
        //transform.rotation = Quaternion.Euler(new Vector3(0f, _yAngle, 0f));

        if (Input.GetKey(KeyCode.W))
        {
            //세 번째 매개변수가 0이면 첫 번째 매개변수만 읽고, 1이면 두 번째 매개변수만 읽는다. 그러므로 0.5를 해주자. 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.5f);

            //방향키를 누른 쪽으로 바라보게 하기
            //로컬이 아니라 월드 기준임(=캐릭터가 어디를 쳐다보건 상관없이 w키를 누르면 북쪽으로 바라봄)
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
