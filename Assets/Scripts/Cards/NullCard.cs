using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class NullCard : ISpecialCard
    {
        public int ID { get; } = -1;
        public string Name { get; } = "NULL";
        public string Description { get; } = "NULL";
        public int RequireBlock { get; private set; }
        public SpecialStatusType SpecialStatusType { get; } = SpecialStatusType.NONE;
        public float Span { get; } = 0f;

        public NullCard(int requireBlock)
        {
            RequireBlock = requireBlock;
        }
    }
}
