using System.Collections.Generic;

namespace Snake
{
    public class Direction
    {
        //The top left corner of the grid is (0,0)
        public static Direction Left = new Direction(0, -1);
        public static Direction Right = new Direction(0, 1);
        public static Direction Up = new Direction(-1, 0);
        public static Direction Down = new Direction(1, 0);

        public int RowOffset { get; set; }
        public int ColumnOffset { get; set; }

        private Direction(int row, int column)
        {
            RowOffset = row;
            ColumnOffset = column;
        }

        public Direction OppositeDirection()
        {
            return new Direction(-RowOffset, -ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColumnOffset == direction.ColumnOffset;
        }

        public override int GetHashCode()
        {
            int hashCode = 240067226;
            hashCode = hashCode * -1521134295 + RowOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + ColumnOffset.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}
