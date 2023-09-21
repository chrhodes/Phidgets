using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
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

            OpenInterfaceKitCommand = new DelegateCommand(OpenInterfaceKit, OpenInterfaceKitCanExecute);
            CloseInterfaceKitCommand = new DelegateCommand(CloseInterfaceKit, CloseInterfaceKitCanExecute);

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);
                
            Message = "InterfaceKitViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        //public ICommand SayHelloCommand { get; private set; }
        
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

        private InterfaceKitEx _IK;
        public InterfaceKitEx IK
        {
            get { return _IK; }
            set { _IK = value; }
        }

        private Dictionary<string, InterfaceKitEx> _InterfaceKitsD;
        public Dictionary<string, InterfaceKitEx> InterfaceKitsD
        {
            get
            {
                if (null == _InterfaceKitsD)
                {
                    _InterfaceKitsD = new Dictionary<string, InterfaceKitEx>();
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

        #region Commands

        #region OpenInterfaceKit Command

        public DelegateCommand OpenInterfaceKitCommand { get; set; }
        public string OpenInterfaceKitContent { get; set; } = "OpenInterfaceKit";
        public string OpenInterfaceKitToolTip { get; set; } = "OpenInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string OpenInterfaceKitContent { get; set; } = "ViewName_OpenInterfaceKitContent";
        //public string OpenInterfaceKitToolTip { get; set; } = "ViewName_OpenInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenInterfaceKitContent">OpenInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_OpenInterfaceKitContentToolTip">OpenInterfaceKit ToolTip</system:String>  

        public void OpenInterfaceKit()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenInterfaceKit";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish(
            //      new OpenInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Subscribe(OpenInterfaceKit);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region CloseInterfaceKit Command

        public DelegateCommand CloseInterfaceKitCommand { get; set; }
        public string CloseInterfaceKitContent { get; set; } = "CloseInterfaceKit";
        public string CloseInterfaceKitToolTip { get; set; } = "CloseInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string CloseInterfaceKitContent { get; set; } = "ViewName_CloseInterfaceKitContent";
        //public string CloseInterfaceKitToolTip { get; set; } = "ViewName_CloseInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseInterfaceKitContent">CloseInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_CloseInterfaceKitContentToolTip">CloseInterfaceKit ToolTip</system:String>  

        public void CloseInterfaceKit()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseInterfaceKit";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish(
            //      new CloseInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Subscribe(CloseInterfaceKit);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        // End Cut One


        #endregion

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
