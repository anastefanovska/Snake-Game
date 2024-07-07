using System.Collections.Generic;
using System;

namespace Snake
{
    public class State
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public GridValue[,] Grid { get; set; }
        public Direction Direction { get; set; }
        public int Score { get; set; }
        public bool GameOver { get; set; }
        public GameMode Mode { get; set; }

        private LinkedList<Direction> directionChanges = new LinkedList<Direction>();
        private LinkedList<Position> snakePositions = new LinkedList<Position>();
        private static Random random = new Random();

        public State(int rows, int columns, GameMode mode)
        {
            Rows = rows;
            Columns = columns;
            Grid = new GridValue[rows, columns];
            Direction = Direction.Right;
            Mode = mode;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            int middleRow = Rows / 2;
            for (int column = 1; column <= 3; column++)
            {
                Grid[middleRow, column] = GridValue.Snake;
                snakePositions.AddFirst(new Position(middleRow, column));
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> emptyPositions = new List<Position>(EmptyPositions());
            if (emptyPositions.Count == 0) return;

            Position position = emptyPositions[random.Next(emptyPositions.Count)];
            Grid[position.Row, position.Column] = GridValue.Food;
        }

        private void AddPoison()
        {
            List<Position> emptyPositions = new List<Position>(EmptyPositions());
            if (emptyPositions.Count == 0) return;

            Position position = emptyPositions[random.Next(emptyPositions.Count)];
            Grid[position.Row, position.Column] = GridValue.Poison;
        }

        public Position HeadPosition() => snakePositions.First.Value;

        public Position TailPosition() => snakePositions.Last.Value;

        public IEnumerable<Position> SnakePositions() => snakePositions;

        private void AddHead(Position position)
        {
            snakePositions.AddFirst(position);
            Grid[position.Row, position.Column] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            if (directionChanges.Count == 0)
            {
                return Direction;
            }
            return directionChanges.Last.Value;
        }

        private bool CanChangeDirection(Direction newDirection)
        {
            if (directionChanges.Count == 2)
            {
                return false;
            }

            Direction lastDirection = GetLastDirection();

            return newDirection != lastDirection && newDirection != lastDirection.OppositeDirection();
        }

        public void ChangeDirection(Direction direction)
        {
            if (CanChangeDirection(direction)) //so the game doesn't end when you press a direction that is opposite from the current one 
            {
                directionChanges.AddLast(direction);
            }
        }


        private bool OutsideGrid(Position position)
        {
            return position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns;
        }

        private GridValue WhatWasHit(Position newHead)
        {
            if (Mode == GameMode.Borderless && OutsideGrid(newHead))
            {
                return GridValue.Empty; // In Borderless mode, hitting the border is not game over
            }

            if (OutsideGrid(newHead)) return GridValue.Out;
            if (newHead == TailPosition()) return GridValue.Empty;
            return Grid[newHead.Row, newHead.Column];
        }

        private Position GetWrappedPosition(Position position)
        {
            int wrappedRow = (position.Row + Rows) % Rows;
            int wrappedColumn = (position.Column + Columns) % Columns;
            return new Position(wrappedRow, wrappedColumn);
        }


        //generally: move the head, delete the tail
        //if there's food: move only the head
        //game over: if it eats poison, it runs into a wall or into itself
        public void Move()
        {
            if (directionChanges.Count > 0)
            {
                Direction = directionChanges.First.Value;
                directionChanges.RemoveFirst();
            }

            Position newHead = HeadPosition().ChangePosition(Direction);
            if (Mode == GameMode.Borderless)
            {
                newHead = GetWrappedPosition(newHead);
            }

            GridValue hit = WhatWasHit(newHead);

            if (hit == GridValue.Out || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHead);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHead);
                Score++;
                AddFood();
                if (Mode == GameMode.PoisonedApple)
                {
                    AddPoison();
                }
            }
            else if (hit == GridValue.Poison)
            {
                GameOver = true;
            }
        }
    }
}