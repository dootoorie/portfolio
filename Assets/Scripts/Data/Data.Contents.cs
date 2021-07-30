using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-26

namespace Data
{

    [Serializable]      //메모리에서 들고있는 것을 파일로 변환할 수 있는 기능
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int defense;
        public int totalExp;
    }

    [Serializable]      //메모리에서 들고있는 것을 파일로 변환할 수 있는 기능
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }

            return dict;
        }
    }

}