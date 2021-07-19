using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJECTION : MonoBehaviour
{
    //Local : 개개인을 기준으로 특정 물체를 기준으로 한 좌표
    //World : 특정 물체는 아니고 공간의 축을 중심으로 하나의 공통된 좌표
        
    //3D 게임 자체가 3D 좌표로 만들어 졌지만, 우리가 바라보는 화면은 2D 좌표이다.
    //결국 최종적으로는 3D 좌표를 2D로 변환해야 한다.

    //순서 : Local <-> World <-> Viewport <-> Screen(화면)

    void Update()
    {
        //Screen 좌표계(화면 해상도 크기 좌표계)
        //Debug.Log(Input.mousePosition);

        //Viewport 좌표계(화면 해상도 크기를 0 ~ 1까지 범위를 비율로 표현환 좌표계)
        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));

        //if(Input.GetMouseButtonDown(0))
        //{
        //    //Camera.main으로 메인 카메라를 가져오고, Screen좌표계에서 World좌표계로 옮겨주는 ScreenToWorldPoint를 쓴다.
        //    //Vector3 좌표계는 매개변수로(마우스로 클릭한x축, 마우스로 클릭한 y축, Clipping Plane의 near사각형 부분)
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        //    //방향벡터 = 목적지의 위치 - 나의 위치
        //    //이것을 응용하면,
        //    //방향벡터 dir = 카메라가 찍고있는 Clipping plane(클리핑 평면)의 near plane(가까운 평면)의 위치 좌표 - 현재 찍고있는 카메라의 위치 좌표
        //    Vector3 dir = mousePos - Camera.main.transform.position;

        //    //결과는 (5, 0, 0) 같은 값이 나온다고 치면, 방향벡터를 구하는 것이기 때문에 (1, 0, 0)꼴로 만들어줘야한다. 그러므로 결과값에 정규화 normalized를 해준다.
        //    dir = dir.normalized;


        //    Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);

        //    RaycastHit hit;

        //    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
        //    {
        //        Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
        //    }
        //}


        //더 간단한 방법

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
