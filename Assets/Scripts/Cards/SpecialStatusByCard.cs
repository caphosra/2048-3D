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
        /// <summary>
        /// If ([this value] == ([this value] & [the value checked]) is true, it is not the type of attack.
        /// </summary>
        IS_NOT_ATTACK = 0b10000000,

        NONE = 0b0,
        WRONG_MOVE = 0b1,

        ALL_DELETE = IS_NOT_ATTACK | 0b1
    }

    public static class SpecialStatusTypeEx
    {
        public static bool IsAttack(this SpecialStatusType type)
        {
            return !(SpecialStatusType.IS_NOT_ATTACK == (SpecialStatusType.IS_NOT_ATTACK & type));
        }
    }
}
