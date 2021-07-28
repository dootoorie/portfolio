using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager //MonoBehaviour �����ش�
{
    //UIManager ���� ���� : UI_Button�� Sort Order ������ ����, 2021-07-18

    int order = 10;

    //� �ڷᱸ���� ��������� ������? -> Stack ����
    //���� : ���� �������� ����� �˾��� ���� �����Ǿ� �ϴϱ� -> Stack���� ��������

    //GameObject ��� ������Ʈ�� �������� ���� �ξ� ����ϴ�.
    //UI_Button�� UI_Popup�� ��ӹް� ������ UI_Popup�� ����.
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();

    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");

            //Find�� �ߴµ� @UI_Root�� ��ã�Ƽ�, root�� null�̸�, 2021-07-18
            if (root == null)
            {
                //���̾��Űâ���� ��Ŭ���Ͽ� Create ���ִ� ������ �ڵ�� �����Ͽ� ���ӿ�����Ʈ�� ������ָ� �ȴ�.
                //�� ���ӿ�����Ʈ�� �̸��� @UI_Root �� ���� ������., 2021-07-18
                root = new GameObject { name = "@UI_Root" };
            }

            //���� ã������ root�� ����
            return root;
        }
    }

    //SetCanvas �Լ��� ���� : �ܺο��� �˾� ���� UI�� ���� ��,
    //������ UIManager���� SetCanvas�� ��û�ؼ� �ڱ� Canvas�� �ִ� order�� ä���޶�� ��Ź
    public void SetCanvas(GameObject go, bool sort = true)
    {
        //Util�� ���� Canvas ������Ʈ�� ����
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //canvas�ȿ� canvas�� ��ø�ؼ� ���� ��, �θ� � ���� ������, ���� ������ �� sortingOrder�� �����ٴ� �ǹ�
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            
            order++;
        }

        //sorting�� ��û ���ߴٴ� ����, �˾��� ���� ���� �Ϲ�UI
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    //UI_HPBar �����, 2021-07-28
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        //���� name�� ���� �ʾҴٸ�,
        if (string.IsNullOrEmpty(name))
        {
            //<T>�� �Ȱ��� �̸��� ����ϰڴٴ� ��
            name = typeof(T).Name;
        }

        //������ ����� : UI����/WorldSpace/
        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        //Resources/Prefabs/WorldSpace/UI_HPBar�����տ�
        //����ī�޶� �پ����� ���� ��Ȳ���� �������� ���̰� ���� ��, �ڵ�� ����, 2021-07-28
        //Canvas ������Ʈ�� �ν��Ͻ�ȭ �� ������ ����
        Canvas canvas = go.GetOrAddComponent<Canvas>();

        //renderMode�� WorldSpace�� ����
        canvas.renderMode = RenderMode.WorldSpace;

        //EventCamera�� ����ī�޶�� ����
        canvas.worldCamera = Camera.main;
        
        return Util.GetOrAddComponent<T>(go);
    }

    //������ �����, 2021-07-19
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        //���� name�� ���� �ʾҴٸ�,
        if (string.IsNullOrEmpty(name))
        {
            //<T>�� �Ȱ��� �̸��� ����ϰڴٴ� ��
            name = typeof(T).Name;
        }

        //������ ����� : UI����/Popup����/
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Util.GetOrAddComponent<T>(go);
    }

    //���� ���� �Լ�
    //string name : �����հ� ���� -> Prefabs����/UI����/Popup����/UI_Button(���������� �̸�)�� �ǳ��ִ� ����
    //public T : ��ũ��Ʈ�� ���� -> Scripts����/UI����/Popup����/UI_Button(��ũ��Ʈ���� �̸�)�� �ǳ��ִ� ����
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene  //T�� �ƹ��� �޴°��� �ƴ϶�, UI_Scene�� ��ӹ޴� ��
    {
        //���� name�� ���� �ʾҴٸ�,
        if (string.IsNullOrEmpty(name))
        {
            //<T>�� �Ȱ��� �̸��� ����ϰڴٴ� ��
            name = typeof(T).Name;
        }

        //������ ����� : UI����/Popup����/
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        //���� ������Ʈ�� ���� ���� �߿��� ��ɵ��� ������Ʈ�� �����ִ�.        
        //������ ���������(���ӿ�����Ʈ), ����� �ο� �ҷ���, ������Ʈ�� ��������Ѵ�.
        //������Ʈ�� ������ �����, Util.cs���ٰ� ����� �ϳ� �־���Ҵ�.
        T sceneUI = Util.GetOrAddComponent<T>(go);

        _sceneUI = sceneUI;                

        //go�� ��ġ(= ���̾��Űâ�� UI ���ӿ�����Ʈ�� ��ġ)�� root�� ��ġ�� ���Ѵ�(�θ������ �����Ͽ�), 2021-07-18
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }



    //�˾��� ���� �Լ�
    //string name : �����հ� ���� -> Prefabs����/UI����/Popup����/UI_Button(���������� �̸�)�� �ǳ��ִ� ����
    //public T : ��ũ��Ʈ�� ���� -> Scripts����/UI����/Popup����/UI_Button(��ũ��Ʈ���� �̸�)�� �ǳ��ִ� ����
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup  //T�� �ƹ��� �޴°��� �ƴ϶�, UI_Popup�� ��ӹ޴� ��
    {
        //���� name�� ���� �ʾҴٸ�,
        if(string.IsNullOrEmpty(name))
        {
            //<T>�� �Ȱ��� �̸��� ����ϰڴٴ� ��
            name = typeof(T).Name;
        }

        //������ ����� : UI����/Popup����/
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        //���� ������Ʈ�� ���� ���� �߿��� ��ɵ��� ������Ʈ�� �����ִ�.        
        //������ ���������(���ӿ�����Ʈ), ����� �ο� �ҷ���, ������Ʈ�� ��������Ѵ�.
        //������Ʈ�� ������ �����, Util.cs���ٰ� ����� �ϳ� �־���Ҵ�.
        T popup = Util.GetOrAddComponent<T>(go);

        popupStack.Push(popup);

        //���⼭ order++ �ϸ� �ȵǴ� ���� : ���̾��Ű�� UI_Button ���� ������Ʈ�� �巡�� �� ������� �������� �����Ҷ����� �������� ��� ó���� �ȵǴ� ������ ���� 
        //order++;
                

        //go�� ��ġ(= ���̾��Űâ�� UI ���ӿ�����Ʈ�� ��ġ)�� root�� ��ġ�� ���Ѵ�(�θ������ �����Ͽ�), 2021-07-18
        go.transform.SetParent(Root.transform);

        return popup;
    }

    //�˾��� �ݴ� �Լ�(���� ����)
    //UI�� ���� ��, �������� ������ UI�� �ݴ� ���� �ƴ� �߰��κ� UI�� �� ���� �ִ°��� �̿��� �����ϱ� ����
    public void ClosePopupUI(UI_Popup popup)
    {
        //������
        if (popupStack.Count == 0)
        {
            //�ƹ��͵� �� �Ѵ�.
            return;
        }

        //������ �Ǿ� �ϴ°Ÿ�,
        //Peek() : ������ ���� ���� �ִ� �׸��� ��ȯ 
        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            
            return;
        }

        ClosePopupUI();
    }

    //�˾��� �ݴ� �Լ�(�Ϲ� ����)
    public void ClosePopupUI()
    {
        //stack�� �ǵ帱 ����, �׻� �׻� Count�� üũ�ϴ� ���� ����ȭ ����
        //stack�� �ƹ��͵� ������
        if (popupStack.Count == 0)
        {
            return;
        }

        //���� stack�� �ϳ��� ������, Pop() �ؼ� popup������Ʈ�� �ֱ�
        UI_Popup popup = popupStack.Pop();

        //�̾ƿ����� ������� �ϱ� ������ Destroy
        //Destroy(popup.gameObject) : popup ������Ʈ�� �����ִ� ���� ������Ʈ�� ����
        Managers.Resource.Destroy(popup.gameObject);

        //Destroy�� ���� �� �̻� popup�� �����ϸ� �ȵ�. �׷��Ƿ� null�� �־�����.
        popup = null;

        order--;
    }

    //��� UI�� �ݴ� �Լ�
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    //�޸� ������ ���� ������ �����ֱ�, 2021-07-21
    public void Clear()
    {
        //Stack<UI_Popup>�� �����ִ� �Լ��� �̹� ���������� ���� �߰�
        CloseAllPopupUI();

        //UI_Scene�� ��������
        _sceneUI = null;
    }
}