using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public static class CardGraphicStore
    {
        private const string GRAPHICS_DIRECTORY = "CardGraphics";

        private static Dictionary<int, Sprite> store = new Dictionary<int, Sprite>();

        public static Sprite GetById(int id)
        {
            if(store.ContainsKey(id))
            {
                return store[id];
            }
            else
            {
                var sprite = Resources.Load<Sprite>($"{GRAPHICS_DIRECTORY}/id_{id:0000}");
                store.Add(id, sprite);
                return sprite;
            }
        }
    }
}
