using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2021-07-28

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    //체력은 Stat.cs에서 관리하고 있었다, 2021-07-28
    Stat _stat;

    //UI_Base.cs (최상위부모)에서 Start(){ Init(); }을 하는 중이므로, 여기에 있는 Init()도 호출하여 실행중이다, 2021-07-28
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        //체력은 Stat.cs에서 관리하고 있었다, 2021-07-28
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        //HPBar의 위치가 이상하니, 강제로 머리 위에 위치하게끔 만드는 것, 2021-07-28
        //우선 부모님의 위치를 뽑아와야한다.
        //gameObject : UI_HPBar.cs를 들고있는 게임오브젝트에 가서
        //transform : 그 게임오브젝트의 트랜스폼(위치,좌표,크기)를 뽑아낸 다음,
        //parent : 그 게임오브젝트의 부모에 접근
        Transform parent = gameObject.transform.parent;

        //부모(ex:Player, Monster)의 위치를 UI_HPBar의 위치(gameObject.transform.position)에 저장
        //더 디테일 하게는, 부모(ex:Player, Monster)의 위치에다가 y축으로 조금 더한값으로 하는게 좋다.        
        //gameObject.transform.position = parent.position + Vector3.up * 2.0f;        
        //문제점 : 객체마다 키가 달라서 하드코드로 HPBar의 위치를 적어줘야한다.
        //해결책 : 콜라이더의 위치로 하면 객체마다 키가 다르더라도 머리 위에 정확히 올릴 수 있다.
        gameObject.transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y * 0.5f);

        //Player의 Rotation이 돌아갈 때마다, HPBar의 Rotation도 돌아가므로,
        //이를 고정시키기 위해, HPBar가 항상 카메라를 바라보게 하기 (일명 빌보드), 2021-07-28 
        //gameObject.transform.LookAt(Camera.main.transform);
        //빌보드는 좌우가 뒤집히는 버그가 있었으므로, HPBar의 rotation을 카메라의 rotation과 똑같이 해준다, 2021-07-28
        gameObject.transform.rotation = Camera.main.transform.rotation;

        //캐스팅을 해주는 이유 : ratio가 float이기 때문에 _stat.Hp / _stat.MaxHP; 둘 다 int일 경우,
        //한 개라도 float으로 만들면 결과값이 float이므로 캐스팅 한다, 2021-07-28 
        float ratio = _stat.Hp / (float)_stat.MaxHP;

        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        //HPBar의 체력을 깎이게 하려면, 프리팹의 HPBar 게임오브젝트의 Slider 컴포넌트에 접근한 후,
        //Value의 값에 변화를 주면 된다, 2021-07-28
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio; 
    }
}