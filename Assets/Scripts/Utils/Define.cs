using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    // Scene, 2021-07-20
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    // Sound, 2021-07-20
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,           //갯수 세기위해 추가, 2021-07-20
    }

    // UI, 2021-07-17
    public enum UIEvent
    {
        Click,
        Drag,
    }

    // 마우스, 2021-07-05
    public enum MouseEvent
    {
        Press,              //마우스를 누르고 있는 동안, 2021-07-05
        Click,              //마우스를 클릭하고 손을 뗐을 때, 2021-07-05
    }

    // 카메라모드, 2021-07-04
    public enum CameraMode
    {
        QuaterView,         //쿼터뷰, 2021-07-04
    }
}