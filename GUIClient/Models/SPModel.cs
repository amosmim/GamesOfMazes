using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.ComponentModel;

namespace GUIClient
{
    class SPModel : INotifyPropertyChanged
    {
        private Socket server;

        public SPModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
