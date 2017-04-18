using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
	/// Searchable interface: graph that should be solved.
	/// </summary>
    public interface ISearchable<T>
    {
        /// <summary>
        /// gets the start state of the graph
        /// </summary>
        /// <returns>state object</returns>
        State<T> GetInitialState();

        /// <summary>
        /// gets the end state of the graph
        /// </summary>
        /// <returns>state object</returns>
        State<T> GetGoalState();

        /// <summary>
        /// get all the passiable states from s state.
        /// responsible to set ComeFrom of States to s state!
        /// </summary>
        /// <param name="s">origin state</param>
        /// <returns>list of state objects</returns>
        List<State<T>> GetAllPossibleStates(State<T> s); 

    }
}