using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using MazeWebAPI.Models;
using MazeLib;

namespace MazeWebAPI.Controllers
{
    public class MazeController : ApiController
    {
        private SinglePlayerModel model;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MazeController()
        {
            this.model = new SinglePlayerModel();
        }

        /// <summary>
        /// Generate a new a maze.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        /// <returns>maze</returns>
        [HttpGet()]
        [Route("api/Maze/Generate/{name}/{rows}/{cols}")]
        public JObject Generate(string name, int rows, int cols)
        {
            Maze maze = this.model.Generate(name, rows, cols);
            JObject mazeJS = JObject.Parse(maze.ToJSON());
            return mazeJS;
        }

        /// <summary>
        /// Solve a specific maze.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="algo">searcher</param>
        /// <returns>solution</returns>
        [HttpGet()]
        [Route("api/Maze/Solve/{name}/{algo}")]
        public JObject Solve(string name, int algo)
        {
            string str = this.model.Solve(name, algo);
            JObject solution = JObject.Parse(str);
            return solution;
        }
    }
}
