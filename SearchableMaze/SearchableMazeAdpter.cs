using System;
using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace SearchableMaze
{

    /// <summary>
    /// Adpter of Maze as Searchable interface.
    /// </summary>
    /// <seealso cref="SearchAlgorithmsLib.ISearchable{MazeLib.Position}" />
    public class SearchableMazeAdpter : ISearchable<Position>
    {
        private Maze maze;

        /// <summary>
        /// Constractor.
        /// </summary>
        /// <param name="maz"></param>
        public SearchableMazeAdpter(Maze maz)
        {
            this.maze = maz;
        }


        /// <summary>
        /// get all the passiable states from s state.
        /// responsible to set ComeFrom of States to s state!
        /// </summary>
        /// <param name="s">origin state</param>
        /// <returns>list of state objects</returns>
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

        /// <summary>
        /// return new state from origin state with updated cost and CameFrom fields.
        /// </summary>
        /// <param name="father">origin state</param>
        /// <param name="rowDif">change in the rows</param>
        /// <param name="colDif">change in the colums</param>
        /// <returns></returns>
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

        /// <summary>
        /// gets the end state of the Maze
        /// </summary>
        /// <returns>state object</returns>
        public State<Position> GetGoalState()
        {
            return new State<Position> (maze.GoalPos);
        }

        /// <summary>
        /// gets the start state of the Maze
        /// </summary>
        /// <returns>state object</returns>
        public State<Position> GetInitialState()
        {
            return new State<Position>(maze.InitialPos);
        }

        /// <summary>
        /// console print of the Maze.
        /// start Mark with 'S', and the goal with 'E'.
        /// </summary>
        public void Print()
        {
            Position start = maze.InitialPos;
            Position end = maze.GoalPos;
            Console.ForegroundColor = ConsoleColor.White;
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('S');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        if ((end.Col == col) && (end.Row == row))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('E');
                            Console.ForegroundColor = ConsoleColor.White;
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
