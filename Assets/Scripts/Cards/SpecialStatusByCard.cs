using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class SpecialStatusByCard
    {
        /// <summary>
        /// Status type
        /// </summary>
        public SpecialStatusType StatusType { get; set; }
        
        /// <summary>
        /// The time when this status end.
        /// </summary>
        public int End { get; set; }
    }

    public enum SpecialStatusType
    {
        NONE,
        WRONG_MOVE,
    }
}
