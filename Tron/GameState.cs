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
        public Direction DirOrange { get; private set; }
        public Direction DirGreen { get; private set; }
        public int ScoreOrange { get; private set; }
        public int ScoreGreen { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirOrangeChanges = new LinkedList<Direction>();
        private readonly LinkedList<Direction> dirGreenChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> orangePlayerPos = new LinkedList<Position>();
        private readonly LinkedList<Position> greenPlayerPos = new LinkedList<Position>();

        private ScoreWriter scoreWriter;

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            DirOrange = Direction.Right;
            DirGreen = Direction.Right;

            scoreWriter = new ScoreWriter();

            AddOrangePlayer();
            AddGreenPlayer();
        }

        private void AddOrangePlayer()
        {
            int r = Rows / 4;

            for (int c = 1; c <= 9; c++)
            {
                Grid[r, c] = GridValue.OrangePlayer;
                orangePlayerPos.AddFirst(new Position(r, c));
            }
        }

        private void AddGreenPlayer()
        {
            int r = Rows / 4 * 3;

            for (int c = 1; c <= 9; c++)
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

        private Direction GetOrangeLastDirection()
        {
            if (dirOrangeChanges.Count == 0)
            {
                return DirOrange;
            }
            return dirOrangeChanges.Last.Value;
        }

        private Direction GetGreenLastDirection()
        {
            if (dirGreenChanges.Count == 0)
            {
                return DirGreen;
            }
            return dirGreenChanges.Last.Value;
        }

        private bool CanOrangeChangeDirection(Direction newDirOrange)
        {
            if (dirOrangeChanges.Count == 2)
            {
                return false;
            }

            Direction lastDirOrange = GetOrangeLastDirection();
            return newDirOrange != lastDirOrange && newDirOrange != lastDirOrange.Opposite();
        }

        private bool CanGreenChangeDirection(Direction newDirGreen)
        {
            if (dirGreenChanges.Count == 2)
            {
                return false;
            }

            Direction lastDirGreen = GetGreenLastDirection();
            return newDirGreen != lastDirGreen && newDirGreen != lastDirGreen.Opposite();
        }

        public void ChangeOrangeDirection(Direction dirOrange)
        {
            if (CanOrangeChangeDirection(dirOrange))
            {
                dirOrangeChanges.AddLast(dirOrange);
            }
        }

        public void ChangeGreenDirection(Direction dirGreen)
        {
            if (CanGreenChangeDirection(dirGreen))
            {
                dirGreenChanges.AddLast(dirGreen);
            }
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

            if (newHeadPos == GetOrangeHeadPos() || newHeadPos == GetGreenHeadPos())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void MoveOrange()
        {
            if (dirOrangeChanges.Count > 0)
            {
                DirOrange = dirOrangeChanges.First.Value;
                dirOrangeChanges.RemoveFirst();
            }

            Position newOrangeHeadPos = GetOrangeHeadPos().Translate(DirOrange);
            GridValue hitOr = WillHit(newOrangeHeadPos);

            if (hitOr == GridValue.Outside || hitOr == GridValue.OrangePlayer || hitOr == GridValue.GreenPlayer)
            {
                GameOver = true;
                scoreWriter.writeWiner(ScoreWriter.PATH, ScoreWriter.GREEN);
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
            if (dirGreenChanges.Count > 0)
            {
                DirGreen = dirGreenChanges.First.Value;
                dirGreenChanges.RemoveFirst();
            }

            Position newGreenHeadPos = GetGreenHeadPos().Translate(DirGreen);
            GridValue hitGr = WillHit(newGreenHeadPos);

            if (hitGr == GridValue.Outside || hitGr == GridValue.OrangePlayer || hitGr == GridValue.GreenPlayer)
            {
                GameOver = true;
                scoreWriter.writeWiner(ScoreWriter.PATH, ScoreWriter.ORANGE);
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
