using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    GameObject tank;

    void Start()
    {
        //이 경우, 프리팹의 이름이 꼭 Tank여야만 실행된다. 그렇지 않으면 실행되지 않는다.
        tank = Managers.Resource.Instantiate("Tank");

        //밑의 코드는 이제 더 이상 쓰지 않는다.

        //코드가 아닌 것들은 전부 Resources에 넣어서 관리.
        //직역 : Resources폴더에서 로드할건데 게임오브젝트를 (Prefabs폴더의 Tank프리팹)
        //prefab = Resources.Load<GameObject>("Prefabs/Tank");
        
        //tank = Instantiate(prefab);

        //Destroy(tank, 3.0f);
    }    
}
