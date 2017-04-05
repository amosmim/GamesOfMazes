

namespace SearchAlgorithmsLib
{
    public interface ISearcher<T>
    {
        // the search method
        Solution<T> Search(ISearchable<T> searchable);
        // get how many nodes were evaluated by the algorithm
        int getNumberOfNodesEvaluated();

    }
}
