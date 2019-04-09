using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GUIBoardManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSpawner bornParticleSpawner;
        [SerializeField]
        private ParticleSpawner readyParticleSpawner;

        [SerializeField]
        private List<GameObject> blocks;

        private Dictionary<(int x, int y), BlockManager> blockManagers;

        // Start is called before the first frame update
        void Start()
        {
            InitalizeBlockManagers();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void InitalizeBlockManagers()
        {
            blockManagers = new Dictionary<(int x, int y), BlockManager>();

            var regex = new Regex("[1-5]");

            foreach(var block in blocks)
            {
                var blockManager = block.GetComponent<BlockManager>();

                var matches = regex.Matches(block.name);

                var x = int.Parse(matches[0].Groups[0].Value);
                var y = int.Parse(matches[1].Groups[0].Value);

                blockManagers.Add((x, y), blockManager);
            }
        }

        public void ShowBornParticleAt(int x, int y)
        {
            bornParticleSpawner.Spawn(CalcPosition(x, y) + new Vector3(0, 0.5f, 0));
        }

        public void ShowReadyParticleAt(int x, int y)
        {
            readyParticleSpawner.Spawn(CalcPosition(x, y) + new Vector3(0, -0.1f, 0));
        }

        public void ChangeGraphicAll(BlockBoard board)
        {
            readyParticleSpawner.Free();
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    blockManagers[(x, y)].ChangeMaterial(board[x, y]);
                    if (board[x, y] >= 64)
                    {
                        ShowReadyParticleAt(x, y);
                    }
                }
            }
        }

        protected const float BLOCKS_Y = 0.75f;
        protected const float BLOCK_SPACE = 4f;
        protected Vector3 CalcPosition(int x, int y)
        {
            float pos_x = transform.position.x + (x - 1) * BLOCK_SPACE - 6f;
            float pos_y = BLOCKS_Y;
            float pos_z = transform.position.z + (y - 1) * BLOCK_SPACE - 6f;
            return new Vector3(pos_x, pos_y, pos_z);
        }
    }
}
