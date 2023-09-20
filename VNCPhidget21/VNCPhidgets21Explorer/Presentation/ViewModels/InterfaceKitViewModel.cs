using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

using DevExpress.XtraRichEdit.Commands;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;
using VNC.Core.Services;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class InterfaceKitViewModel : EventViewModelBase, IInterfaceKitViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public InterfaceKitViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //
            
            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);
                
            Message = "InterfaceKitViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        public ICommand SayHelloCommand { get; private set; }
        
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #region Hosts

        private Host _HOST;
        public Host HOST
        {
            get { return _HOST; }
            set { _HOST = value; }
        }

        private Dictionary<string, Host> _HostD;
        public Dictionary<string, Host> HostD
        {
            get
            {
                if (_HostD == null)
                {
                    _HostD = new Dictionary<string, Host>();
                }
                return _HostD;
            }
            set
            {
                _HostD = value;
            }
        }


        private IEnumerable<Host> _Hosts;
        public IEnumerable<Host> Hosts
        {
            get
            {
                if (null == _Hosts)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Hosts =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Elements("Host")
                    //    select new Host(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Hosts;
            }

            set
            {
                _Hosts = value;
            }
        }

        #endregion

        #region InterfaceKits

        private InterfaceKit _IK;
        public InterfaceKit IK
        {
            get { return _IK; }
            set { _IK = value; }
        }

        private Dictionary<string, InterfaceKit> _InterfaceKitsD;
        public Dictionary<string, InterfaceKit> InterfaceKitsD
        {
            get
            {
                if (null == _InterfaceKitsD)
                {
                    _InterfaceKitsD = new Dictionary<string, InterfaceKit>();
                }
                return _InterfaceKitsD;
            }
            set
            {
                _InterfaceKitsD = value;
            }
        }

        private Collection<string> _InterfaceKits;
        public Collection<string> InterfaceKits
        {
            get
            {
                if (null == _InterfaceKits)
                {
                    _InterfaceKits = new Collection<string>();
                }
                return _InterfaceKits;
            }
            set
            {
                _InterfaceKits = value;
            }
        }

        #endregion

        #endregion

        #region Event Handlers



        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        private bool SayHelloCanExecute()
        {
            return true;
        }
        
        #endregion
        
        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
