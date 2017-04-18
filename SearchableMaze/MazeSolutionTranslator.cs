
using System.Collections.Generic;

using MazeLib;
using SearchAlgorithmsLib;
namespace SearchableMaze
{
    public class MazeSolutionTranslator
    {
        /// <summary>
        /// translate Solution object to string of {0,1,2,3}
        /// </summary>
        public MazeSolutionTranslator() { }

        public string SolutionToString(Solution<Position> solution)
        {
            string result = "";
            Stack<State<Position>> solCopy = solution.StackCopy();
            State<Position> state;
            while (solCopy.Count > 0)
            {
                state = solCopy.Pop();
                result = ComeFromInString(state) + result;

            }
            return result;
        }



        private  string ComeFromInString(State<Position> state)
        {

            Position from = state.CameFrom.GetTypeValue();
            Position here = state.GetTypeValue();
            if (from.Row == here.Row)
            {
                if (from.Col > here.Col)
                {
                    return "0"; // Left
                }
                return "1"; // right
            }
            else
            {
                if (from.Row > here.Row)
                {
                    return "2"; //up
                }
                return "3"; // down
            }

        }
    }
}
