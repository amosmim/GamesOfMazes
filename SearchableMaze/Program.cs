using MazeGeneratorLib;
using MazeLib;
using System;
using SearchAlgorithmsLib;


namespace SearchableMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            CompareSolvers();
          
        }

        /// <summary>
        /// tester of BFS & DFS on Maze.
        /// </summary>
        private static void CompareSolvers() { 
            IMazeGenerator g = new DFSMazeGenerator();

            Maze maze = g.Generate(35, 140);
            SearchableMazeAdpter shMaze = new SearchableMazeAdpter(maze);
            Solution<Position> solution;
            //MazeSolutionTranslator translator = new MazeSolutionTranslator();

            shMaze.Print();
            Console.Write("Please enlarge the window to the maximum size.\n");
            ISearcher<Position> searcher = new BFS<Position>();
            try
            {
                solution = searcher.Search(shMaze);
                //Console.WriteLine("Solve with BFS: " + translator.SolutionToString(solution));
                Console.WriteLine("Number of Evaluate in BFS:" + searcher.GetNumberOfNodesEvaluated().ToString());
            }
            catch (NotSolvableException exp)
            {
                Console.WriteLine("failed to Solve with BFS...\n" + exp.Message);
            }


            searcher = new DFS<Position>();
            try
            {
                solution = searcher.Search(shMaze);
               // Console.WriteLine("Solve with DFS: " + translator.SolutionToString(solution));
                Console.WriteLine("Number of Evaluate in DFS:" + searcher.GetNumberOfNodesEvaluated().ToString());
            }
            catch (NotSolvableException exp)
            {
                Console.WriteLine("failed to Solve with DFS...\n" + exp.Message);

            }
            Console.Write("Press Enter to exit.");
            Console.Read();
        }
    }
}
