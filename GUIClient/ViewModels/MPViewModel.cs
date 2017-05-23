using System;
using System.Collections.Generic;
using System.ComponentModel;
using GUIClient.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIClient.ViewModels
{
    public class MPViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action VMRavilWin;
        public event Action VMRavilQuit;
        private MPModel model;

        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">model</param>
        public MPViewModel(MPModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                this.OnPropertyChanged("VM" + e.PropertyName);
            };
            this.model.RivalWin += delegate ()
            {
                this.VMRavilWin?.Invoke();
            };
            this.model.RivalQuit += delegate ()
            {
                this.VMRavilQuit?.Invoke();
            };
            
        }
        
        /// <summary>
        /// VMSerializedGame Property.
        /// </summary>
        public string VMSerializedGame
        {
            get { return this.model.SerializedGame; }
        }

        internal List<string> getGameList()
        {
            return this.model.GetGameList();
        }

        /// <summary>
        /// VMName Property.
        /// </summary>
        public string VMName
        {
            get { return this.model.Name; }
        }

        

        /// <summary>
        /// VMInitialPos Property.
        /// </summary>
        public string VMInitialPos
        {
            get { return this.model.InitialPos; }
        }

        /// <summary>
        /// VMGoalPos Property.
        /// </summary>
        public string VMGoalPos
        {
            get { return this.model.GoalPos; }
        }

        /// <summary>
        /// VMRows Property.
        /// </summary>
        public int VMRows
        {
            get { return this.model.Rows; }
        }

        /// <summary>
        /// VMCols Property.
        /// </summary>
        public int VMCols
        {
            get { return this.model.Cols; }
        }

        public bool VMIsStart
        {
            get { return this.model.IsStart; }
        }

        /// <summary>
        /// VMPlayerPosition Property.
        /// </summary>
        public string VMPlayerPosition
        {
            get { return this.model.PlayerPosition; }
        }

        /// <summary>
        /// VMRavilPlayerPosition Property.
        /// </summary>
        public string VMRavilPlayerPosition
        {
            get { return this.model.RavilPlayerPosition; }
        }
      

        /// <summary>
        /// Initiate move checking algorithm in the model.
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns>0 - not valid move
        ///          1 - valid move
        ///          2 - win</returns>
        public int CheckMove(string direction)
        {
            return this.model.CheckMove(direction);
        }
        

        /// <summary>
        /// Starting the single player game.
        /// </summary>
        /// <param name="name">maze name</param>
        /// <param name="rows">rows</param>
        /// <param name="cols">cols</param>
        /// <returns>true if success else false</returns>
        public bool StartGame(string name, int rows, int cols)
        {
            string command = "start " + name + " " + rows + " " + cols;
            return this.model.StartGame(command);
        }

        public bool JoinToGame(string name)
        {
            return this.model.JoinToGame(name);
        }

        internal void CloseGame()
        {
            this.model.CloseGame();
        }


        /// <summary>
        /// Notify on property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool UpdateGame()
        {
            return this.model.StartUpdateThreads();
        }
    }
}
