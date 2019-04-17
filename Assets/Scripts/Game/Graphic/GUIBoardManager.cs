using System.Collections;
using System.Collections.Generic;

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

        /// <summary>
        /// [WARNINGS] This function depends on the BLOCK NAME (formated as "Block(x)-(y)")
        /// </summary>
        protected void InitalizeBlockManagers()
        {
            blockManagers = new Dictionary<(int x, int y), BlockManager>();

            foreach(var block in blocks)
            {
                var blockManager = block.GetComponent<BlockManager>();

                var x = int.Parse(block.name[5].ToString());
                var y = int.Parse(block.name[7].ToString());

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

        /// <summary>
        /// Callback from the block objects
        /// </summary>
        /// <param name="arg">A text which formated as "(x)-(y)"</param>
        public void BlockClickedCallback(string arg)
        {
            var x = byte.Parse(arg[0].ToString());
            var y = byte.Parse(arg[2].ToString());
        }
    }
}
