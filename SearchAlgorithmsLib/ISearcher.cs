

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// search algoritms interface
    /// </summary>
    /// <typeparam name="T">type of the states in the searchable graph</typeparam>
    public interface ISearcher<T>
    {
        /// <summary>
        ///  the search method.
        ///  should restart the NumberOfNodesEvaluated counter!
        /// </summary>
        /// <param name="searchable">searchable graph to solve</param>
        /// <returns>Solution object</returns>
        Solution<T> Search(ISearchable<T> searchable);

        /// <summary>
        /// get how many nodes were evaluated by the algorithm in the last Search.
        /// </summary>
        /// <returns>number of the nodes that evaluated by the algorithm</returns>
        int GetNumberOfNodesEvaluated();

    }
}
