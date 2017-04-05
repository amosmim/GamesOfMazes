using System;
using System.Collections.Generic;
using MazeLib;


namespace SearchAlgorithmsLib
{
    class SearchableMazeAdpter : ISearchable<Position>
    {
        private Maze maze;

        public SearchableMazeAdpter(Maze maz)
        {
            this.maze = maz;
        }

        public int getMaxNodes()
        {
            return (maze.Cols * maze.Rows);
        }
        public List<State<Position>> GetAllPossibleStates(State<Position> s)
        {
            List<State<Position>> list = new List<State<Position>>();
            Position p = s.GetTypeValue();
            
            if ((p.Row > 0) && (maze[p.Row - 1, p.Col] == CellType.Free))//try to add left Neighbor.
            {
                list.Add(CreateNewSon(s, -1, 0));
            }
            if ((p.Row < maze.Rows-1) && (maze[p.Row + 1, p.Col] == CellType.Free))//try to add right Neighbor.
            {
                list.Add(CreateNewSon(s,1,0));

            }
            if ((p.Col > 0) && (maze[p.Row, p.Col - 1] == CellType.Free))  //try to add down Neighbor.
            {
             
                list.Add(CreateNewSon(s, 0, -1));
            }
            if ((p.Col < maze.Cols-1 ) && (maze[p.Row, p.Col + 1] == CellType.Free))//try to add up Neighbor.
            {
                
                list.Add(CreateNewSon(s,0,1)); 
            }
            return list;
        }

        private State<Position> CreateNewSon(State<Position> father,  int rowDif, int colDif)
        {
            State<Position> son = new State<Position>(new Position(father.GetTypeValue().Row + rowDif,
                                                                   colDif + father.GetTypeValue().Col))
            {
                CameFrom = father,
                Cost = father.Cost + 1
            };
            return son;
        }

        public State<Position> GetGoalState()
        {
            return new State<Position> (maze.GoalPos);
        }

        public State<Position> GetInitialState()
        {
            return new State<Position>(maze.InitialPos);
        }

        public void Print()
        {
            Position start = maze.InitialPos;
            Position end = maze.GoalPos;
            for (int i = -2; i < maze.Cols; i++)
                Console.Write('-');
            Console.WriteLine();
            for(int row = 0; row < maze.Rows; row++)
            {
                Console.Write('|');
                for (int col = 0; col < maze.Cols; col++)
                {
                    if ((start.Col == col) && (start.Row == row))
                    {
                        Console.Write('?');
                    }
                    else
                    {
                        if ((end.Col == col) && (end.Row == row))
                        {
                            Console.Write('$');
                        }
                        else
                        {
                            if (maze[row, col] == CellType.Free)
                            {
                                Console.Write(' ');
                            }
                            else
                            {
                                Console.Write('#');
                            }
                        }
                    }
                }
                Console.WriteLine('|');
            }
            for (int i = -2; i < maze.Cols; i++)
                Console.Write('-');
            Console.WriteLine();

        }


        
    }  
}
