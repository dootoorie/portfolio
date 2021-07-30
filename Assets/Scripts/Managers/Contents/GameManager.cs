using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-30

public class GameManager    //MonoBehaviour 삭제
{
    GameObject _player;

    //우리는 Monster와 Player가 죽으면 사라지면서 null이 되는 것을 목격하였다.
    //그러므로 따로 GameManager.cs를 파서 관리해주자.
    //Monster와 Player 관리, 2021-07-30
    //int <-> GameObject (int와 GameObject가 짝지은 형태)
    //나중에 서버와 연동하려면 아이디 즉, int 같은 것을 쓰는 것이 좋을 것 같다    
    //Dictionary<int, GameObject> _monsters = new Dictionary<int, GameObject>();
    
    //지금은 서버와 연동하지 않으므로 int를 없앤 HashSet을 쓰자(Dictionary와 비슷), 2021-07-30
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    //2021-07-30
    public GameObject GetPlayer() 
    {
        return _player;
    }

    //스폰 할 것이 player냐 monster를 구분해야 한다 -> 인자로 받아주자, 2021-07-30 
    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch(type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();

        if (bc == null)
        {
            return Define.WorldObject.Unknown;
        }

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch(type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                    }
                }
                break;

            case Define.WorldObject.Player:                
                {
                    if (_player == go)
                    {
                        _player = null;
                    }
                }
                break;
        }

        Managers.Resource.Destroy(go);    
    }
}