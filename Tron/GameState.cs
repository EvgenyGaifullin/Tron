using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tron
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int ScoreOrange { get; private set; }
        public int ScoreGreen { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Position> orangePlayerPos = new LinkedList<Position>();
        private readonly LinkedList<Position> greenPlayerPos = new LinkedList<Position>();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            AddOrangePlayer();
            AddGreenPlayer();
        }

        private void AddOrangePlayer()
        {
            int r = Rows / 4 ;

            for (int c = 1; c <= 7; c++)
            {
                Grid[r, c] = GridValue.OrangePlayer;
                orangePlayerPos.AddFirst(new Position(r, c));
            }
        }

        private void AddGreenPlayer()
        {
            int r = (Rows / 4) * 3;

            for (int c = 1; c <= 7; c++)
            {
                Grid[r, c] = GridValue.GreenPlayer;
                greenPlayerPos.AddFirst(new Position(r, c));
            }
        }

        public Position GetOrangeHeadPos()
        {
            return orangePlayerPos.First.Value;
        }
        
        public Position GetOrangeTailPos()
        {
            return orangePlayerPos.Last.Value;
        }

        public Position GetGreenHeadPos()
        {
            return greenPlayerPos.First.Value;
        }

        public Position GetGreenTailPos()
        {
            return greenPlayerPos.Last.Value;
        }

        public IEnumerable<Position> GetOrangePositionS()
        {
            return orangePlayerPos;
        }

        public IEnumerable<Position> GetGreenPositionS()
        {
            return greenPlayerPos;
        }

        private void AddOrangeHead(Position pos)
        {
            orangePlayerPos.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.OrangePlayer;
        }

        private void AddGreenHead(Position pos)
        {
            greenPlayerPos.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.GreenPlayer;
        }

        private void RemoveOrangeTail()
        {
            Position tail = orangePlayerPos.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            orangePlayerPos.RemoveLast();
        }

        private void RemoveGreenTail()
        {
            Position tail = greenPlayerPos.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            greenPlayerPos.RemoveLast();
        }

        public void ChangeDirection(Direction dir)
        {
            Dir = dir;
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Col < 0 || pos.Row >= Rows || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == GetOrangeHeadPos() || newHeadPos == GetGreenHeadPos())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void MoveOrange()
        {
            Position newOrangeHeadPos = GetOrangeHeadPos().Translate(Dir);
            GridValue hitOr = WillHit(newOrangeHeadPos);

            if (hitOr == GridValue.Outside || hitOr == GridValue.OrangePlayer || hitOr == GridValue.GreenPlayer)
            {
                GameOver = true;
                ScoreGreen++;
            } 
            else if (hitOr == GridValue.Empty)
            {
                RemoveOrangeTail();
                AddOrangeHead(newOrangeHeadPos);
            }
        }

        public void MoveGreen()
        {
            Position newGreenHeadPos = GetGreenHeadPos().Translate(Dir);
            GridValue hitGr = WillHit(newGreenHeadPos);

            if (hitGr == GridValue.Outside || hitGr == GridValue.OrangePlayer || hitGr == GridValue.GreenPlayer)
            {
                GameOver = true;
                ScoreOrange++;
            }
            else if (hitGr == GridValue.Empty)
            {
                RemoveGreenTail();
                AddGreenHead(newGreenHeadPos);
            }
        }
    }
}
