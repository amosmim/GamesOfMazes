using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using MazeWebAPI.Models;

namespace MazeWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private UsersModel model;
        /// <summary>
        /// Login data.
        /// </summary>
        public class LogData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// Registration Data.
        /// </summary>
        public class RegData
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        /// <summary>
        /// Multyplayer Game Data.
        /// </summary>
        public class GameData
        { 
            public string Player1 { get; set; }
            public string Player2 { get; set; }
            public string Winner { get; set; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UsersController()
        {
            this.model = new UsersModel();
        }

        /// <summary>
        /// Get all users ranking.
        /// </summary>
        /// <returns>User rankings</returns>
        // GET: api/Users/5
        [HttpGet()]
        [Route("api/Users/GetUsersRanking")]
        public IHttpActionResult GetUsersRanking()
        {
            JArray results = this.model.GetGameResults();

            return Ok(results);
        }

        /// <summary>
        /// User login method.
        /// </summary>
        /// <param name="data">login data</param>
        /// <returns>true if success otherwise error</returns>
        // POST: api/Users
        [HttpPost()]
        [Route("api/Users/Login")]
        public IHttpActionResult Login(LogData data)
        {
            bool results = this.model.Login(data.Username, data.Password);

            return Ok(new { isLogged = results });
        }

        /// <summary>
        /// A new user's registration.
        /// </summary>
        /// <param name="data">registration data</param>
        /// <returns>True if success otherwise error</returns>
        // POST: api/Users
        [HttpPost()]
        [Route("api/Users/Register")]
        public IHttpActionResult Register(RegData data)
        {
            int results = this.model.Register(data.Username, data.Password, data.Email);

            // in case of error
            if (results == -2)
            {
                return InternalServerError();
            } 

            return Ok(new { registerCode = results });
        }

        /// <summary>
        /// Add game results to DB.
        /// </summary>
        /// <param name="data">multiplayer game data</param>
        /// <returns>true if success otherwise error</returns>
        // POST: api/Users
        [HttpPost()]
        [Route("api/Users/RegisterGame")]
        public IHttpActionResult RegisterGame(GameData data)
        {
            bool results = this.model.RegisterGameResults(data.Player1, data.Player2, data.Winner);

            // in case of error
            if (results == false)
            {
                return InternalServerError();
            }

            return Ok(new { registerCode = results });
        }

    }
}
