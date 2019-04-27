using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public interface ISpecialCard
    {
        int ID { get; }

        string Name { get; }

        string Description { get; }

        int RequireBlock { get; }

        SpecialStatusType SpecialStatusType { get; }

        float Span { get; }
    }
}
