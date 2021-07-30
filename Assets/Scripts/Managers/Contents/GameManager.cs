using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-30

public class GameManager    //MonoBehaviour ����
{
    GameObject _player;

    //�츮�� Monster�� Player�� ������ ������鼭 null�� �Ǵ� ���� ����Ͽ���.
    //�׷��Ƿ� ���� GameManager.cs�� �ļ� ����������.
    //Monster�� Player ����, 2021-07-30
    //int <-> GameObject (int�� GameObject�� ¦���� ����)
    //���߿� ������ �����Ϸ��� ���̵� ��, int ���� ���� ���� ���� ���� �� ����    
    //Dictionary<int, GameObject> _monsters = new Dictionary<int, GameObject>();
    
    //������ ������ �������� �����Ƿ� int�� ���� HashSet�� ����(Dictionary�� ���), 2021-07-30
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    //2021-07-30
    public GameObject GetPlayer() 
    {
        return _player;
    }

    //���� �� ���� player�� monster�� �����ؾ� �Ѵ� -> ���ڷ� �޾�����, 2021-07-30 
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