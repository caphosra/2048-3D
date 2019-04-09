using System;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class BlockBoard : ICloneable
    {
        private int[,] blockNumber = new int[4, 4];
        private int[] dx = new int[4] { 1, -1, 0, 0 };
        private int[] dy = new int[4] { 0, 0, 1, -1 };

        public int GetValue(int x, int y)
        {
            return blockNumber[x - 1, y - 1];
        }

        public void SetValue(int x, int y, int val)
        {
            blockNumber[x - 1, y - 1] = val;
        }

        public int this[int x, int y]
        {
            get { return GetValue(x, y); }
            set { SetValue(x, y, value); }
        }


        public void Move(MoveDirection direction, out List<(int, int)> changed)
        {
            changed = new List<(int, int)>();
            switch (direction)
            {
                case MoveDirection.UP:
                    {
                        for (int x = 1; x <= 4; x++)
                        {
                            for (int y = 4; y >= 1; y--)
                            {
                                int currentVal = this[x, y];
                                if (currentVal == 0) continue;

                                int dy = y;
                                while (true)
                                {
                                    if (dy == 4)
                                    {
                                        if (dy != y)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((x, dy));
                                        }
                                        this[x, y] = 0;
                                        this[x, dy] = currentVal;
                                        break;
                                    }
                                    if (this[x, dy + 1] == 0)
                                    {
                                        dy++;
                                    }
                                    else if (this[x, dy + 1] == currentVal)
                                    {
                                        changed.Add((x, y));
                                        changed.Add((x, dy + 1));
                                        this[x, y] = 0;
                                        this[x, dy + 1] = currentVal * 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (dy != y)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((x, dy));
                                            this[x, y] = 0;
                                            this[x, dy] = currentVal;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.DOWN:
                    {
                        for (int x = 1; x <= 4; x++)
                        {
                            for (int y = 1; y <= 4; y++)
                            {
                                int currentVal = this[x, y];
                                if (currentVal == 0) continue;

                                int dy = y;
                                while (true)
                                {
                                    if (dy == 1)
                                    {
                                        if (dy != y)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((x, dy));
                                        }
                                        this[x, y] = 0;
                                        this[x, dy] = currentVal;
                                        break;
                                    }
                                    if (this[x, dy - 1] == 0)
                                    {
                                        dy--;
                                    }
                                    else if (this[x, dy - 1] == currentVal)
                                    {
                                        changed.Add((x, y));
                                        changed.Add((x, dy - 1));
                                        this[x, y] = 0;
                                        this[x, dy - 1] = currentVal * 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (dy != y)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((x, dy));
                                            this[x, y] = 0;
                                            this[x, dy] = currentVal;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.LEFT:
                    {
                        for (int y = 1; y <= 4; y++)
                        {
                            for (int x = 1; x <= 4; x++)
                            {
                                int currentVal = this[x, y];
                                if (currentVal == 0) continue;

                                int dx = x;
                                while (true)
                                {
                                    if (dx == 1)
                                    {
                                        if (dx != x)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((dx, y));
                                        }
                                        this[x, y] = 0;
                                        this[dx, y] = currentVal;
                                        break;
                                    }
                                    if (this[dx - 1, y] == 0)
                                    {
                                        dx--;
                                    }
                                    else if (this[dx - 1, y] == currentVal)
                                    {
                                        changed.Add((x, y));
                                        changed.Add((dx - 1, y));
                                        this[x, y] = 0;
                                        this[dx - 1, y] = currentVal * 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (dx != x)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((dx, y));
                                            this[x, y] = 0;
                                            this[dx, y] = currentVal;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.RIGHT:
                    {
                        for (int y = 1; y <= 4; y++)
                        {
                            for (int x = 4; x >= 1; x--)
                            {
                                int currentVal = this[x, y];
                                if (currentVal == 0) continue;

                                int dx = x;
                                while (true)
                                {
                                    if (dx == 4)
                                    {
                                        if (dx != x)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((dx, y));
                                        }
                                        this[x, y] = 0;
                                        this[dx, y] = currentVal;
                                        break;
                                    }
                                    if (this[dx + 1, y] == 0)
                                    {
                                        dx++;
                                    }
                                    else if (this[dx + 1, y] == currentVal)
                                    {
                                        changed.Add((x, y));
                                        changed.Add((dx + 1, y));
                                        this[x, y] = 0;
                                        this[dx + 1, y] = currentVal * 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (dx != x)
                                        {
                                            changed.Add((x, y));
                                            changed.Add((dx, y));
                                            this[x, y] = 0;
                                            this[dx, y] = currentVal;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public (int x, int y) RandomSpawn()
        {
            if (Full) throw new InvalidProgramException();

            // It's not effective.
            // I will change this part.
            while (true)
            {
                var random_x = UnityEngine.Random.Range(1, 4 + 1);
                var random_y = UnityEngine.Random.Range(1, 4 + 1);
                if (this[random_x, random_y] == 0)
                {
                    this[random_x, random_y] = 2;
                    return (random_x, random_y);
                }
            }
        }

        public bool Full
        {
            get
            {
                for (int x = 1; x <= 4; x++)
                {
                    for (int y = 1; y <= 4; y++)
                    {
                        if (this[x, y] == 0) return false;
                    }
                }
                return true;
            }
        }

        public object Clone()
        {
            var board = new BlockBoard();
            for(int x = 1; x <= 4; x++)
            {
                for(int y = 1; y <= 4; y++)
                {
                    board.SetValue(x, y, this.GetValue(x, y));
                }
            }
            return board;
        }
    }

    public enum MoveDirection
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
}
