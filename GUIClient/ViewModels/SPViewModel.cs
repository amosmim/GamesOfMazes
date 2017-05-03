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

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
