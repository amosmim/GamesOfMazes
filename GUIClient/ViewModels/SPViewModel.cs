using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GUIClient
{
    class SPViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string vmSerializedGame;
        private string vmInitialPos;
        private string vmGoalPos;
        private int vmRows;
        private int vmCols;
        private string playerPos;
        private SPModel model;

        public SPViewModel(SPModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged("VM" + e.PropertyName);
            };
        }
       
        public string VMSerializedGame {
            get { return this.vmSerializedGame; }
            set {
                this.vmSerializedGame = value;
                this.OnPropertyChanged("VMSerializedGame");
                }
        }
        public string VMInitialPos {
            get { return this.vmInitialPos; }
            set
            {
                this.vmInitialPos = value;
                this.OnPropertyChanged("VMInitialPos");
            }
        }
        public string VMGoalPos {
            get { return this.vmGoalPos; }
            set
            {
                this.vmGoalPos = value;
                this.OnPropertyChanged("VMGoalPos");
            }
        }
        public int VMRows {
            get { return this.vmRows; }
            set
            {
                this.vmRows = value;
                this.OnPropertyChanged("VMRows");
            }
        }
        public int VMCols{
            get { return this.vmCols; }
            set
            {
                this.vmCols = value;
                this.OnPropertyChanged("VMCols");
            }
        }

        public string VMPlayerPosition { get { return this.playerPos; }
            set {
                this.playerPos = value;
                this.OnPropertyChanged("VMPlayerPositon");
            }
        }

        public bool CheckMove(string direction)
        {
            return true;
           //return this.model.CheckMove(direction);
        }

        public void StartGame(string name, int rows, int cols)
        {
            string command = "start " + name + " " + rows + " " + cols;
            this.model.StartGame(command);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
