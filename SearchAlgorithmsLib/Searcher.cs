using Priority_Queue;


namespace SearchAlgorithmsLib
{
    public abstract class Searcher<T> : ISearcher<T>
    {

        private int maxNudes;
       
        private FastPriorityQueue<State<T>> openList;
        private int evaluatedNodes;
        public Searcher()
        {
            //this priority need max node size, its shold be override with UpdateMaxNodes()
            maxNudes = 100;
            openList = new FastPriorityQueue<State<T>>(maxNudes);         

            evaluatedNodes = 0;
        }
        protected State<T> PopOpenList()
        {
            if(openList.Count+1 >= this.maxNudes)
            {
                maxNudes *= 2;
                openList.Resize(this.maxNudes);
            }
            evaluatedNodes++;
            return openList.Dequeue();
        }
        protected void AddToOpenList(State<T> state)
        {
              openList.Enqueue(state, state.Cost);
            
        }

        protected void UpdateCost(State<T> state)
        {
            openList.UpdatePriority(state, state.Cost);
        }
        
       
        protected State<T> GetStateFromOpenList(T state)
        {
            foreach(State<T> s in openList)
            {
                if (s.GetTypeValue().Equals(state))
                {
                    return s;
                }
            }
            return null;
        }

        protected bool OpenListContains (State<T> state)
        {
            return openList.Contains(state);
        }
        // a property of openList
        public int OpenListSize
        { // it is a read-only property :)
            get { return openList.Count; }
        }
        // ISearcher’s methods:
        public virtual int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
        public abstract Solution<T> Search(ISearchable<T> searchable);

    }
}
