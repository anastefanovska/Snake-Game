
namespace Snake
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Position ChangePosition(Direction direction)
        {
            return new Position(Row + direction.RowOffset, Column + direction.ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
