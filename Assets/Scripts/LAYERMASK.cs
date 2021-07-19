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

            //첫 번째 방법
            //8번 Layer만 1로 만들어서 변수 mask에 저장
            //만약 7번 Layer도 추가 하고 싶으면 |를 붙여주고 값을 추가해준다.
            //int mask = (1 << 8) | (1 << 7);

            //두 번째 방법
            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Ground");

            RaycastHit hit;

            //발사한 ray가 mask변수에 담긴 8번 Layer에 닿으면
            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                //이름으로 출력
                //Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");

                //태그로 출력
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.tag}");
            }
        }
    }
}
