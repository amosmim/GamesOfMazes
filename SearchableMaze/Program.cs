using MazeGeneratorLib;
using MazeLib;
using System;
using SearchAlgorithmsLib;


namespace MazeAdapter
    {
    class Program
    {
        static void Main(string[] args)
        {
            CompareSolvers();
          
        }
        private static void CompareSolvers() { 
            IMazeGenerator g = new DFSMazeGenerator();

            Maze maze = g.Generate(35, 150);
            SearchableMaze shMaze = new SearchableMaze(maze);

            shMaze.Print();
            Console.Write("Please enlarge the window to the maximum size.\n");
            ISearcher<Position> searcher = new BFS<Position>();
            try
            {
                searcher.Search(shMaze);
                Console.WriteLine("Number of Evaluate in BFS:" + searcher.getNumberOfNodesEvaluated().ToString());
            }
            catch (NotSolvableException exp)
            {
                Console.WriteLine("failed to Solve with BFS...\n" + exp.Message);
            }


            searcher = new DFS<Position>();
            try
            {
                searcher.Search(shMaze);
                Console.WriteLine("Number of Evaluate in DFS:" + searcher.getNumberOfNodesEvaluated().ToString());
            }
            catch (NotSolvableException exp)
            {
                Console.WriteLine("failed to Solve with DFS...\n" + exp.Message);

            }
            Console.Write("Press any key to quit.");
            Console.Read();
        }
    }
}
