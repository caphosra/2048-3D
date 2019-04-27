using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class WrongMoveCard128 : ISpecialCard
    {
        public int ID { get; } = 1;
        public string Name { get; } = "WrongMove";
        public string Description { get; } = "相手の盤面のみ動かした方向と逆の方向に動くようになる";
        public int RequireBlock { get; } = 128;
        public SpecialStatusType SpecialStatusType { get; } = SpecialStatusType.WRONG_MOVE;
        public float Span { get; } = 5f;
    }
}
