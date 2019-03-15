using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<(int x, int y), BlockManager> blockManagers;
    private BlockBoard board = new BlockBoard();

    // Start is called before the first frame update
    void Start()
    {
        blockManagers = new Dictionary<(int x, int y), BlockManager>();
        for(int x = 1; x <= 4; x++)
        {
            for(int y = 1; y <= 4; y++)
            {
                var blockManager = GameObject.Find($"Block{x}-{y}").GetComponent<BlockManager>();
                blockManagers.Add((x, y), blockManager);
            }
        }
        board.RandomSpawn();
        ChangeMaterialsAll();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            board.Move(MoveDirection.UP, out List<(int, int)> changed);
            if(changed.Count != 0 && !board.Full)
            {
                board.RandomSpawn();
                ChangeMaterialsAll();
            }
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            board.Move(MoveDirection.DOWN, out List<(int, int)> changed);
            if(changed.Count != 0 && !board.Full)
            {
                board.RandomSpawn();
                ChangeMaterialsAll();
            }
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            board.Move(MoveDirection.LEFT, out List<(int, int)> changed);
            if(changed.Count != 0 && !board.Full)
            {
                board.RandomSpawn();
                ChangeMaterialsAll();
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            board.Move(MoveDirection.RIGHT, out List<(int, int)> changed);
            if(changed.Count != 0 && !board.Full)
            {
                board.RandomSpawn();
                ChangeMaterialsAll();
            }
        }
    }

    void ChangeMaterialsAll()
    {
        for(int x = 1; x <= 4; x++)
        {
            for(int y = 1; y <= 4; y++)
            {
                blockManagers[(x, y)].ChangeMaterial(board[x, y]);
            }
        }
    }
}
