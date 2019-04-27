using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class CardBase : ISpecialCard
    {
        public int ID { get; } = -1;
        public string Name { get; } = "CARD_BASE";
        public string Description { get; } = "CARD_BASE";
        public int RequireBlock { get; } = 64;
        public SpecialStatusType SpecialStatusType { get; } = SpecialStatusType.NONE;
        public float Span { get; } = 0f;
    }
}
