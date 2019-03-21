using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSpawner bornParticleSpawner;
        [SerializeField]
        private ParticleSpawner readyParticleSpawner;

        private Dictionary<(int x, int y), BlockManager> blockManagers;
        private BlockBoard board = new BlockBoard();

        // Start is called before the first frame update
        void Start()
        {
            blockManagers = new Dictionary<(int x, int y), BlockManager>();
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    var blockManager = GameObject.Find($"Block{x}-{y}").GetComponent<BlockManager>();
                    blockManagers.Add((x, y), blockManager);
                }
            }
            board.RandomSpawn();
            ChangeGraphicAll();

            if(GameStartArgment.OnlineGame)
            {
                Debug.Log("Unload Matching Scene...");
                StartCoroutine(UnloadMatchingScene());

                var master = PhotonNetwork.MasterClient;
                Debug.Log($"MasterNickName:{master.NickName}");
            }
        }

        IEnumerator UnloadMatchingScene()
        {
            yield return SceneManager.UnloadSceneAsync("OnlineMatching");
            Debug.Log("Unloaded Matching Scene Successfuly");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                board.Move(MoveDirection.UP, out List<(int, int)> changed);
                if (changed.Count != 0 && !board.Full)
                {
                    RandomSpawn();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                board.Move(MoveDirection.DOWN, out List<(int, int)> changed);
                if (changed.Count != 0 && !board.Full)
                {
                    RandomSpawn();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                board.Move(MoveDirection.LEFT, out List<(int, int)> changed);
                if (changed.Count != 0 && !board.Full)
                {
                    RandomSpawn();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                board.Move(MoveDirection.RIGHT, out List<(int, int)> changed);
                if (changed.Count != 0 && !board.Full)
                {
                    RandomSpawn();
                }
            }
        }

        void RandomSpawn()
        {
            var pos = board.RandomSpawn();
            bornParticleSpawner.Spawn(CalcPosition(pos.x, pos.y) + new Vector3(0, 0.5f, 0));
            ChangeGraphicAll();
        }

        void ChangeGraphicAll()
        {
            readyParticleSpawner.Free();
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    blockManagers[(x, y)].ChangeMaterial(board[x, y]);
                    if (board[x, y] >= 64)
                    {
                        readyParticleSpawner.Spawn(CalcPosition(x, y) + new Vector3(0, -0.1f, 0));
                    }
                }
            }
        }

        const float BLOCKS_Y = 0.75f;
        Vector3 CalcPosition(int x, int y)
        {
            return new Vector3((x - 1) * 5f - 7.5f, BLOCKS_Y, (y - 1) * 5f - 7.5f);
        }
    }
}
