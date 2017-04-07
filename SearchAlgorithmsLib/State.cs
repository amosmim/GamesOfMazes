using Priority_Queue;
namespace SearchAlgorithmsLib
{
    public class State<T> : FastPriorityQueueNode
    {
        private T state; // the state represented by a string
        private float cost; // cost to reach this state (set by a setter)
        private State<T> cameFrom; // the state we came from to this state (setter)
        


        public State(T state) // CTOR
        {
            this.state = state;
            this.cameFrom = null;
            
            
        }

               
        public float Cost { get => cost; set => cost = value; }
        public State<T> CameFrom { get => cameFrom; set => cameFrom = value; }
       

        public override int GetHashCode()
        {
            return this.state.GetHashCode();
        }
        public override bool Equals(object s) // we overload Object's Equals method
        {
            return state.Equals((s as State<T>).state);
        }
       /* public static bool operator ==(State<T> first, State<T> second) { return first.Equals(second); }
        public static bool operator !=(State<T> first, State<T> second) { return !first.Equals(second); }*/
        public T GetTypeValue()
        {
            return state;
        }
       
    }

}