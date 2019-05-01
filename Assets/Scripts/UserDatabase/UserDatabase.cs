using System;
using System.Collections.Generic;

using Com.Capra314Cabra.Project_2048Ex;

namespace Com.Capra314Cabra.Project_2048Ex.DataBase
{
    public static class UserDatabase
    {
        public static Dictionary<int, ISpecialCard> PlayersSpecialCards { get; set; } =
            new Dictionary<int, ISpecialCard>()
            {
                { 64, new NullCard(64) },
                { 128, new WrongMoveCard128() },
                { 256, new NullCard(256) },
                { 512, new NullCard(512) }
            };
    }
}
