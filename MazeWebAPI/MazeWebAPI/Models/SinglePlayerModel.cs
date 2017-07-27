using System;
using MazeLib;
using MazeGeneratorLib;
using System.Collections.Generic;
using SearchableMaze;
using SearchAlgorithmsLib;
using Newtonsoft.Json.Linq;

namespace MazeWebAPI.Models
{
    public class SinglePlayerModel
    {
        private static Dictionary<string, GameObject> singlePlayer = new Dictionary<string, GameObject>();
        /// <summary>
        /// Initializes a new instance of the <see cref="T:WebServiceAPI.Models.SinglePlayerModel"/> class.
        /// </summary>
        public SinglePlayerModel()
        {
        }

		/// <summary>
		/// Generates the maze.
		/// </summary>
		/// <returns>The maze.</returns>
		/// <param name="name">Maze name.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="cols">Cols.</param>
		public Maze Generate(string name, int rows, int cols) 
        {
			DFSMazeGenerator generator = new DFSMazeGenerator();

            // override old maze
            if (singlePlayer.ContainsKey(name)) {
                singlePlayer.Remove(name);
            }

			Maze newMaze = generator.Generate(rows, cols);
			newMaze.Name = name;

			// creating a GameObject that represents a single maze map
			GameObject gameObj = new GameObject();
			gameObj.maze = newMaze;
			gameObj.dfsSolution = null;
			gameObj.bfsSolution = null;

			singlePlayer.Add(name, gameObj);

			return newMaze;
        }

		/// <summary>
		/// Solve the specified maze with the specified algorithm.
		/// </summary>
		/// <returns>a solution to a maze or error if game doesn't exist</returns>
		/// <param name="name">Maze.</param>
		/// <param name="algo">Algorithm. 0 - BFS 1 - DFS</param>
		public string Solve(string name, int algo)
        {
			GameObject go = singlePlayer[name];
			string solution;

			// check whether the solution exists in the server
			if (algo == 0)
			{
				// game exists but solution doesn't
				if (go.bfsSolution == null)
				{
					solution = Solver(name, algo);
					go.bfsSolution = solution;
				}
				else
				{
					return go.bfsSolution;
				}
			}
			else
			{
				// game exists but solution doesn't
				if (go.dfsSolution == null)
				{
					solution = Solver(name, algo);
					go.dfsSolution = solution;
				}
				else
				{
					return go.dfsSolution;
				}
			}

			// because this dictionary is Read Only
			singlePlayer.Remove(name);
			singlePlayer.Add(name, go);

			return solution;
        }

		/// <summary>
		/// Helper method for Solve method.
		/// </summary>
		/// <returns>a solution to a maze</returns>
		/// <param name="maze">Maze.</param>
		/// <param name="algorithm">Algorithm.</param>
		private string Solver(string maze, int algorithm)
		{
			Maze mazeGame = singlePlayer[maze].maze;
			SearchableMazeAdpter shMaze = new SearchableMazeAdpter(mazeGame);
			Solution<Position> solution;
			MazeSolutionTranslator translator = new MazeSolutionTranslator();
			ISearcher<Position> searcher;

			if (algorithm == 0)
			{
				searcher = new BFS<Position>();
			}
			else
			{
				searcher = new DFS<Position>();
			}

			// try solving the maze
			try
			{
				solution = searcher.Search(shMaze);
			}
			catch (NotSolvableException)
			{
				// unable to solve
				JObject err = new JObject();
				err["Name"] = maze;
				err["Solution"] = "Maze is unsolvable";
				err["NodesEvaluated"] = searcher.GetNumberOfNodesEvaluated().ToString();
				return err.ToString();
			}

			JObject msg = new JObject();
			msg["Name"] = maze;
			msg["Solution"] = translator.SolutionToString(solution);
			msg["NodesEvaluated"] = searcher.GetNumberOfNodesEvaluated().ToString();

			return msg.ToString();
		}

    }
}
