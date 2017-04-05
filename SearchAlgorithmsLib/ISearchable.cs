using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public interface ISearchable<T>
    {
        State<T> GetInitialState();
        int getMaxNodes();
        State<T> GetGoalState();
        List<State<T>> GetAllPossibleStates(State<T> s); // need to set ComeFrom of State to the first one!!!

    }
}