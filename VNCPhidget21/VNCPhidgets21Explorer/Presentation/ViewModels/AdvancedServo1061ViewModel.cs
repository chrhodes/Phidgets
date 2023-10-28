﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

using DevExpress.XtraRichEdit.Commands;

using Phidgets;
using Phidgets.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class AdvancedServo1061ViewModel : EventViewModelBase, IAdvancedServo1061ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public AdvancedServo1061ViewModel(
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

            // Turn on logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = true;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);
            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            ConfigFileName = "phidgetconfig.json";
            LoadUIConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);
                
            Message = "AdvancedServo1061ViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(ConfigFileName);

            Resources.PhidgetConfig? phidgetConfig = JsonSerializer.Deserialize<Resources.PhidgetConfig>(jsonString);
            this.Hosts = phidgetConfig.Hosts.ToList();
            //this.Sensors2 = phidgetConfig.Sensors.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private string _ConfigFileName;

        public string ConfigFileName
        {
            get => _ConfigFileName;
            set
            {
                if (_ConfigFileName == value) return;
                _ConfigFileName = value;
                OnPropertyChanged();
            }
        }

        private Resources.PhidgetConfig _phidgetConfig;
        public Resources.PhidgetConfig PhidgetConfig
        {
            get => _phidgetConfig;
            set
            {
                if (_phidgetConfig == value)
                    return;
                _phidgetConfig = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.Host> _Hosts;
        public IEnumerable<Resources.Host> Hosts
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
                OnPropertyChanged();
            }
        }

        private Resources.Host _selectedHost;
        public Resources.Host SelectedHost
        {
            get => _selectedHost;
            set
            {
                if (_selectedHost == value)
                    return;
                _selectedHost = value;
                AdvancedServos = _selectedHost.AdvancedServos.ToList<Resources.AdvancedServo>();
                OnPropertyChanged();
            }
        }

        private IEnumerable<Resources.AdvancedServo> _AdvancedServos;
        public IEnumerable<Resources.AdvancedServo> AdvancedServos
        {
            get
            {
                if (null == _AdvancedServos)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_InterfaceKits =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
                    //    select new InterfaceKit(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _AdvancedServos;
            }

            set
            {
                _AdvancedServos = value;
                OnPropertyChanged();
            }
        }

        private Resources.AdvancedServo _selectedAdvancedServo;
        public Resources.AdvancedServo SelectedAdvancedServo
        {
            get => _selectedAdvancedServo;
            set
            {
                if (_selectedAdvancedServo == value)
                    return;
                _selectedAdvancedServo = value;

                OpenAdvancedServoCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private AdvancedServoEx _activeAdvancedServo;
        public AdvancedServoEx ActiveAdvancedServo
        {
            get => _activeAdvancedServo;
            set
            {
                if (_activeAdvancedServo == value)
                    return;
                _activeAdvancedServo = value;

                if (_activeAdvancedServo is not null)
                {
                    PhidgetDevice = _activeAdvancedServo.AdvancedServo;
                }
                else
                {
                    // TODO(crhodes)
                    // PhidgetDevice = null ???
                    // Will need to declare Phidgets.Phidget?
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private Phidgets.Phidget _phidgetDevice;
        public Phidgets.Phidget PhidgetDevice
        {
            get => _phidgetDevice;
            set
            {
                if (_phidgetDevice == value)
                    return;
                _phidgetDevice = value;
                OnPropertyChanged();
            }
        }

        private bool? _deviceAttached;
        public bool? DeviceAttached
        {
            get => _deviceAttached;
            set
            {
                if (_deviceAttached == value)
                    return;
                _deviceAttached = value;
                OnPropertyChanged();
            }
        }

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

        #region AdvancedServo Properties

        #region Servo S0

        private Double _currentS0;
        public Double Current_S0
        {
            get => _currentS0;
            set
            {
                if (_currentS0 == value)
                    return;
                _currentS0 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionMax_S0;
        public Double PositionMax_S0
        {
            get => _positionMax_S0;
            set
            {
                if (_positionMax_S0 == value)
                    return;
                _positionMax_S0 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionS0;
        public Double Position_S0
        {
            get => _positionS0;
            set
            {
                if (_positionS0 == value)
                    return;
                _positionS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[0].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[0].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[0].Position = value;
                }
            }
        }

        private Double _positionMin_S0;
        public Double PositionMin_S0
        {
            get => _positionMin_S0;
            set
            {
                if (_positionMin_S0 == value)
                    return;
                _positionMin_S0 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS0;
        public Double VelocityMin_S0
        {
            get => _velocityMinS0;
            set
            {
                if (_velocityMinS0 == value)
                    return;
                _velocityMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS0;
        public Double Velocity_S0
        {
            get => _velocityS0;
            set
            {
                if (_velocityS0 == value)
                    return;
                _velocityS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS0;
        public Double VelocityLimit_S0
        {
            get => _velocityLimitS0;
            set
            {
                if (_velocityLimitS0 == value)
                    return;
                _velocityLimitS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[0].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[0].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[0].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS0;
        public Double VelocityMax_S0
        {
            get => _velocityMaxS0;
            set
            {
                if (_velocityMaxS0 == value)
                    return;
                _velocityMaxS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS0;
        public Double AccelerationMin_S0
        {
            get => _accelerationMinS0;
            set
            {
                if (_accelerationMinS0 == value)
                    return;
                _accelerationMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS0;
        public Double Acceleration_S0
        {
            get => _accelerationS0;
            set
            {
                if (_accelerationS0 == value)
                    return;
                _accelerationS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration = AccelerationMax_S0;
                }
            }
        }

        private Double _accelerationMaxS0;
        public Double AccelerationMax_S0
        {
            get => _accelerationMaxS0;
            set
            {
                if (_accelerationMaxS0 == value)
                    return;
                _accelerationMaxS0 = value;
                OnPropertyChanged();
            }
        }


        private bool? _engagedS0;
        public bool? Engaged_S0
        {
            get => _engagedS0;
            set
            {
                if (_engagedS0 == value)
                    return;
                _engagedS0 = value;

                ActiveAdvancedServo.AdvancedServo.servos[0].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS0;
        public bool? SpeedRamping_S0
        {
            get => _speedRampingS0;
            set
            {
                if (_speedRampingS0 == value)
                    return;
                _speedRampingS0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS0;
        public bool? Stopped_S0
        {
            get => _stoppedS0;
            set
            {
                if (_stoppedS0 == value)
                    return;
                _stoppedS0 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S1

        private Double _currentS1;
        public Double Current_S1
        {
            get => _currentS1;
            set
            {
                if (_currentS1 == value)
                    return;
                _currentS1 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionMax_S1;
        public Double PositionMax_S1
        {
            get => _positionMax_S1;
            set
            {
                if (_positionMax_S1 == value)
                    return;
                _positionMax_S1 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionS1;
        public Double Position_S1
        {
            get => _positionS1;
            set
            {
                if (_positionS1 == value)
                    return;
                _positionS1 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[1].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[1].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[1].Position = value;
                }
            }
        }

        private Double _positionMin_S1;
        public Double PositionMin_S1
        {
            get => _positionMin_S1;
            set
            {
                if (_positionMin_S1 == value)
                    return;
                _positionMin_S1 = value;

                OnPropertyChanged();
            }
        }



        private Double _velocityMinS1;
        public Double VelocityMin_S1
        {
            get => _velocityMinS1;
            set
            {
                if (_velocityMinS1 == value)
                    return;
                _velocityMinS1 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS1;
        public Double Velocity_S1
        {
            get => _velocityS1;
            set
            {
                if (_velocityS1 == value)
                    return;
                _velocityS1 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS1;
        public Double VelocityLimit_S1
        {
            get => _velocityLimitS1;
            set
            {
                if (_velocityLimitS1 == value)
                    return;
                _velocityLimitS1 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[1].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[1].VelocityLimit = value;
                    }
                }
                catch (Exception ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[1].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS1;
        public Double VelocityMax_S1
        {
            get => _velocityMaxS1;
            set
            {
                if (_velocityMaxS1 == value)
                    return;
                _velocityMaxS1 = value;
                OnPropertyChanged();
            }
        }



        private Double _accelerationMinS1;
        public Double AccelerationMin_S1
        {
            get => _accelerationMinS1;
            set
            {
                if (_accelerationMinS1 == value)
                    return;
                _accelerationMinS1 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS1;
        public Double Acceleration_S1
        {
            get => _accelerationS1;
            set
            {
                if (_accelerationS1 == value)
                    return;
                _accelerationS1 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[1].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[1].Acceleration = value;
                    }
                }
                catch (Exception ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[1].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[1].Acceleration = AccelerationMax_S1;
                }
            }
        }

        private Double _accelerationMaxS1;
        public Double AccelerationMax_S1
        {
            get => _accelerationMaxS1;
            set
            {
                if (_accelerationMaxS1 == value)
                    return;
                _accelerationMaxS1 = value;
                OnPropertyChanged();
            }
        }


        private bool? _engagedS1;
        public bool? Engaged_S1
        {
            get => _engagedS1;
            set
            {
                if (_engagedS1 == value)
                    return;
                _engagedS1 = value;

                ActiveAdvancedServo.AdvancedServo.servos[1].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS1;
        public bool? SpeedRamping_S1
        {
            get => _speedRampingS1;
            set
            {
                if (_speedRampingS1 == value)
                    return;
                _speedRampingS1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS1;
        public bool? Stopped_S1
        {
            get => _stoppedS1;
            set
            {
                if (_stoppedS1 == value)
                    return;
                _stoppedS1 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S2

        private Double _currentS2;
        public Double Current_S2
        {
            get => _currentS2;
            set
            {
                if (_currentS2 == value)
                    return;
                _currentS2 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionMax_S2;
        public Double PositionMax_S2
        {
            get => _positionMax_S2;
            set
            {
                if (_positionMax_S2 == value)
                    return;
                _positionMax_S2 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionS2;
        public Double Position_S2
        {
            get => _positionS2;
            set
            {
                if (_positionS2 == value)
                    return;
                _positionS2 = value;
                OnPropertyChanged();

                // TODO(crhodes)
                // Need to bounds check the new value

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[2].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[2].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[2].Position = value;
                }
            }
        }

        private Double _positionMin_S2;
        public Double PositionMin_S2
        {
            get => _positionMin_S2;
            set
            {
                if (_positionMin_S2 == value)
                    return;
                _positionMin_S2 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS2;
        public Double VelocityMin_S2
        {
            get => _velocityMinS2;
            set
            {
                if (_velocityMinS2 == value)
                    return;
                _velocityMinS2 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS2;
        public Double Velocity_S2
        {
            get => _velocityS2;
            set
            {
                if (_velocityS2 == value)
                    return;
                _velocityS2 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS2;
        public Double VelocityLimit_S2
        {
            get => _velocityLimitS2;
            set
            {
                if (_velocityLimitS2 == value)
                    return;
                _velocityLimitS2 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[2].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[2].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[2].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS2;
        public Double VelocityMax_S2
        {
            get => _velocityMaxS2;
            set
            {
                if (_velocityMaxS2 == value)
                    return;
                _velocityMaxS2 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS2;
        public Double AccelerationMin_S2
        {
            get => _accelerationMinS2;
            set
            {
                if (_accelerationMinS2 == value)
                    return;
                _accelerationMinS2 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS2;
        public Double Acceleration_S2
        {
            get => _accelerationS2;
            set
            {
                if (_accelerationS2 == value)
                    return;
                _accelerationS2 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[2].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[2].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[2].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[2].Acceleration = AccelerationMax_S2;
                }
            }
        }

        private Double _accelerationMaxS2;
        public Double AccelerationMax_S2
        {
            get => _accelerationMaxS2;
            set
            {
                if (_accelerationMaxS2 == value)
                    return;
                _accelerationMaxS2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS2;
        public bool? Engaged_S2
        {
            get => _engagedS2;
            set
            {
                if (_engagedS2 == value)
                    return;
                _engagedS2 = value;

                ActiveAdvancedServo.AdvancedServo.servos[2].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS2;
        public bool? SpeedRamping_S2
        {
            get => _speedRampingS2;
            set
            {
                if (_speedRampingS2 == value)
                    return;
                _speedRampingS2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS2;
        public bool? Stopped_S2
        {
            get => _stoppedS2;
            set
            {
                if (_stoppedS2 == value)
                    return;
                _stoppedS2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S3

        private Double _currentS3;
        public Double Current_S3
        {
            get => _currentS3;
            set
            {
                if (_currentS3 == value)
                    return;
                _currentS3 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionMax_S3;
        public Double PositionMax_S3
        {
            get => _positionMax_S3;
            set
            {
                if (_positionMax_S3 == value)
                    return;
                _positionMax_S3 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionS3;
        public Double Position_S3
        {
            get => _positionS3;
            set
            {
                if (_positionS3 == value)
                    return;
                _positionS3 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[3].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[3].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[3].Position = value;
                }
            }
        }

        private Double _positionMin_S3;
        public Double PositionMin_S3
        {
            get => _positionMin_S3;
            set
            {
                if (_positionMin_S3 == value)
                    return;
                _positionMin_S3 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS3;
        public Double VelocityMin_S3
        {
            get => _velocityMinS3;
            set
            {
                if (_velocityMinS3 == value)
                    return;
                _velocityMinS3 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS3;
        public Double Velocity_S3
        {
            get => _velocityS3;
            set
            {
                if (_velocityS3 == value)
                    return;
                _velocityS3 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS3;
        public Double VelocityLimit_S3
        {
            get => _velocityLimitS3;
            set
            {
                if (_velocityLimitS3 == value)
                    return;
                _velocityLimitS3 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[3].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[3].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[3].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS3;
        public Double VelocityMax_S3
        {
            get => _velocityMaxS3;
            set
            {
                if (_velocityMaxS3 == value)
                    return;
                _velocityMaxS3 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS3;
        public Double AccelerationMin_S3
        {
            get => _accelerationMinS3;
            set
            {
                if (_accelerationMinS3 == value)
                    return;
                _accelerationMinS3 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS3;
        public Double Acceleration_S3
        {
            get => _accelerationS3;
            set
            {
                if (_accelerationS3 == value)
                    return;
                _accelerationS3 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[3].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[3].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[3].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[3].Acceleration = AccelerationMax_S3;
                }
            }
        }

        private Double _accelerationMaxS3;
        public Double AccelerationMax_S3
        {
            get => _accelerationMaxS3;
            set
            {
                if (_accelerationMaxS3 == value)
                    return;
                _accelerationMaxS3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS3;
        public bool? Engaged_S3
        {
            get => _engagedS3;
            set
            {
                if (_engagedS3 == value)
                    return;
                _engagedS3 = value;

                ActiveAdvancedServo.AdvancedServo.servos[3].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS3;
        public bool? SpeedRamping_S3
        {
            get => _speedRampingS3;
            set
            {
                if (_speedRampingS3 == value)
                    return;
                _speedRampingS3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS3;
        public bool? Stopped_S3
        {
            get => _stoppedS3;
            set
            {
                if (_stoppedS3 == value)
                    return;
                _stoppedS3 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S4

        private Double _currentS4;
        public Double Current_S4
        {
            get => _currentS4;
            set
            {
                if (_currentS4 == value)
                    return;
                _currentS4 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionMax_S4;
        public Double PositionMax_S4
        {
            get => _positionMax_S4;
            set
            {
                if (_positionMax_S4 == value)
                    return;
                _positionMax_S4 = value;
                OnPropertyChanged();
            }
        }


        private Double _positionS4;
        public Double Position_S4
        {
            get => _positionS4;
            set
            {
                if (_positionS4 == value)
                    return;
                _positionS4 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[4].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[4].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[4].Position = value;
                }
            }
        }

        private Double _positionMin_S4;
        public Double PositionMin_S4
        {
            get => _positionMin_S4;
            set
            {
                if (_positionMin_S4 == value)
                    return;
                _positionMin_S4 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS4;
        public Double VelocityMin_S4
        {
            get => _velocityMinS4;
            set
            {
                if (_velocityMinS4 == value)
                    return;
                _velocityMinS4 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS4;
        public Double Velocity_S4
        {
            get => _velocityS4;
            set
            {
                if (_velocityS4 == value)
                    return;
                _velocityS4 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS4;
        public Double VelocityLimit_S4
        {
            get => _velocityLimitS4;
            set
            {
                if (_velocityLimitS4 == value)
                    return;
                _velocityLimitS4 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[4].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[4].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[4].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS4;
        public Double VelocityMax_S4
        {
            get => _velocityMaxS4;
            set
            {
                if (_velocityMaxS4 == value)
                    return;
                _velocityMaxS4 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS4;
        public Double AccelerationMin_S4
        {
            get => _accelerationMinS4;
            set
            {
                if (_accelerationMinS4 == value)
                    return;
                _accelerationMinS4 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS4;
        public Double Acceleration_S4
        {
            get => _accelerationS4;
            set
            {
                if (_accelerationS4 == value)
                    return;
                _accelerationS4 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[4].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[4].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[4].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[4].Acceleration = AccelerationMax_S4;
                }
            }
        }

        private Double _accelerationMaxS4;
        public Double AccelerationMax_S4
        {
            get => _accelerationMaxS4;
            set
            {
                if (_accelerationMaxS4 == value)
                    return;
                _accelerationMaxS4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS4;
        public bool? Engaged_S4
        {
            get => _engagedS4;
            set
            {
                if (_engagedS4 == value)
                    return;
                _engagedS4 = value;

                ActiveAdvancedServo.AdvancedServo.servos[4].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS4;
        public bool? SpeedRamping_S4
        {
            get => _speedRampingS4;
            set
            {
                if (_speedRampingS4 == value)
                    return;
                _speedRampingS4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS4;
        public bool? Stopped_S4
        {
            get => _stoppedS4;
            set
            {
                if (_stoppedS4 == value)
                    return;
                _stoppedS4 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S5

        private Double _currentS5;
        public Double Current_S5
        {
            get => _currentS5;
            set
            {
                if (_currentS5 == value)
                    return;
                _currentS5 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionMax_S5;
        public Double PositionMax_S5
        {
            get => _positionMax_S5;
            set
            {
                if (_positionMax_S5 == value)
                    return;
                _positionMax_S5 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionS5;
        public Double Position_S5
        {
            get => _positionS5;
            set
            {
                if (_positionS5 == value)
                    return;
                _positionS5 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[5].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[5].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[5].Position = value;
                }
            }
        }

        private Double _positionMin_S5;
        public Double PositionMin_S5
        {
            get => _positionMin_S5;
            set
            {
                if (_positionMin_S5 == value)
                    return;
                _positionMin_S5 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS5;
        public Double VelocityMin_S5
        {
            get => _velocityMinS5;
            set
            {
                if (_velocityMinS5 == value)
                    return;
                _velocityMinS5 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS5;
        public Double Velocity_S5
        {
            get => _velocityS5;
            set
            {
                if (_velocityS5 == value)
                    return;
                _velocityS5 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS5;
        public Double VelocityLimit_S5
        {
            get => _velocityLimitS5;
            set
            {
                if (_velocityLimitS5 == value)
                    return;
                _velocityLimitS5 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[5].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[5].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[5].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS5;
        public Double VelocityMax_S5
        {
            get => _velocityMaxS5;
            set
            {
                if (_velocityMaxS5 == value)
                    return;
                _velocityMaxS5 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS5;
        public Double AccelerationMin_S5
        {
            get => _accelerationMinS5;
            set
            {
                if (_accelerationMinS5 == value)
                    return;
                _accelerationMinS5 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS5;
        public Double Acceleration_S5
        {
            get => _accelerationS5;
            set
            {
                if (_accelerationS5 == value)
                    return;
                _accelerationS5 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[5].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[5].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[5].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[5].Acceleration = AccelerationMax_S5;
                }
            }
        }

        private Double _accelerationMaxS5;
        public Double AccelerationMax_S5
        {
            get => _accelerationMaxS5;
            set
            {
                if (_accelerationMaxS5 == value)
                    return;
                _accelerationMaxS5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS5;
        public bool? Engaged_S5
        {
            get => _engagedS5;
            set
            {
                if (_engagedS5 == value)
                    return;
                _engagedS5 = value;

                ActiveAdvancedServo.AdvancedServo.servos[5].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS5;
        public bool? SpeedRamping_S5
        {
            get => _speedRampingS5;
            set
            {
                if (_speedRampingS5 == value)
                    return;
                _speedRampingS5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS5;
        public bool? Stopped_S5
        {
            get => _stoppedS5;
            set
            {
                if (_stoppedS5 == value)
                    return;
                _stoppedS5 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S6

        private Double _currentS6;
        public Double Current_S6
        {
            get => _currentS6;
            set
            {
                if (_currentS6 == value)
                    return;
                _currentS6 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionMax_S6;
        public Double PositionMax_S6
        {
            get => _positionMax_S6;
            set
            {
                if (_positionMax_S6 == value)
                    return;
                _positionMax_S6 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionS6;
        public Double Position_S6
        {
            get => _positionS6;
            set
            {
                if (_positionS6 == value)
                    return;
                _positionS6 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[6].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[6].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[6].Position = value;
                }
            }
        }

        private Double _positionMin_S6;
        public Double PositionMin_S6
        {
            get => _positionMin_S6;
            set
            {
                if (_positionMin_S6 == value)
                    return;
                _positionMin_S6 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS6;
        public Double VelocityMin_S6
        {
            get => _velocityMinS6;
            set
            {
                if (_velocityMinS6 == value)
                    return;
                _velocityMinS6 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS6;
        public Double Velocity_S6
        {
            get => _velocityS6;
            set
            {
                if (_velocityS6 == value)
                    return;
                _velocityS6 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS6;
        public Double VelocityLimit_S6
        {
            get => _velocityLimitS6;
            set
            {
                if (_velocityLimitS6 == value)
                    return;
                _velocityLimitS6 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[6].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[6].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[6].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS6;
        public Double VelocityMax_S6
        {
            get => _velocityMaxS6;
            set
            {
                if (_velocityMaxS6 == value)
                    return;
                _velocityMaxS6 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS6;
        public Double AccelerationMin_S6
        {
            get => _accelerationMinS6;
            set
            {
                if (_accelerationMinS6 == value)
                    return;
                _accelerationMinS6 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS6;
        public Double Acceleration_S6
        {
            get => _accelerationS6;
            set
            {
                if (_accelerationS6 == value)
                    return;
                _accelerationS6 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[6].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[6].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[6].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[0].Acceleration = AccelerationMax_S6;
                }
            }
        }

        private Double _accelerationMaxS6;
        public Double AccelerationMax_S6
        {
            get => _accelerationMaxS6;
            set
            {
                if (_accelerationMaxS6 == value)
                    return;
                _accelerationMaxS6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS6;
        public bool? Engaged_S6
        {
            get => _engagedS6;
            set
            {
                if (_engagedS6 == value)
                    return;
                _engagedS6 = value;

                ActiveAdvancedServo.AdvancedServo.servos[6].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS6;
        public bool? SpeedRamping_S6
        {
            get => _speedRampingS6;
            set
            {
                if (_speedRampingS6 == value)
                    return;
                _speedRampingS6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS6;
        public bool? Stopped_S6
        {
            get => _stoppedS6;
            set
            {
                if (_stoppedS6 == value)
                    return;
                _stoppedS6 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Servo S7

        private Double _currentS7;
        public Double Current_S7
        {
            get => _currentS7;
            set
            {
                if (_currentS7 == value)
                    return;
                _currentS7 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionMax_S7;
        public Double PositionMax_S7
        {
            get => _positionMax_S7;
            set
            {
                if (_positionMax_S7 == value)
                    return;
                _positionMax_S7 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionS7;
        public Double Position_S7
        {
            get => _positionS7;
            set
            {
                if (_positionS7 == value)
                    return;
                _positionS7 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[7].Position != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[7].Position = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[7].Position = value;
                }
            }
        }

        private Double _positionMin_S7;
        public Double PositionMin_S7
        {
            get => _positionMin_S7;
            set
            {
                if (_positionMin_S7 == value)
                    return;
                _positionMin_S7 = value;

                OnPropertyChanged();
            }
        }

        private Double _velocityMinS7;
        public Double VelocityMin_S7
        {
            get => _velocityMinS7;
            set
            {
                if (_velocityMinS7 == value)
                    return;
                _velocityMinS7 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS7;
        public Double Velocity_S7
        {
            get => _velocityS7;
            set
            {
                if (_velocityS7 == value)
                    return;
                _velocityS7 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS7;
        public Double VelocityLimit_S7
        {
            get => _velocityLimitS7;
            set
            {
                if (_velocityLimitS7 == value)
                    return;
                _velocityLimitS7 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[7].VelocityLimit != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[7].VelocityLimit = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    ActiveAdvancedServo.AdvancedServo.servos[7].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS7;
        public Double VelocityMax_S7
        {
            get => _velocityMaxS7;
            set
            {
                if (_velocityMaxS7 == value)
                    return;
                _velocityMaxS7 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationMinS7;
        public Double AccelerationMin_S7
        {
            get => _accelerationMinS7;
            set
            {
                if (_accelerationMinS7 == value)
                    return;
                _accelerationMinS7 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS7;
        public Double Acceleration_S7
        {
            get => _accelerationS7;
            set
            {
                if (_accelerationS7 == value)
                    return;
                _accelerationS7 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveAdvancedServo.AdvancedServo.servos[7].Acceleration != value)
                    {
                        ActiveAdvancedServo.AdvancedServo.servos[7].Acceleration = value;
                    }
                }
                catch (PhidgetException ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    ActiveAdvancedServo.AdvancedServo.servos[7].Acceleration = value;
                    //ActiveAdvancedServo.AdvancedServo.servos[7].Acceleration = AccelerationMax_S7;
                }
            }
        }

        private Double _accelerationMaxS7;
        public Double AccelerationMax_S7
        {
            get => _accelerationMaxS7;
            set
            {
                if (_accelerationMaxS7 == value)
                    return;
                _accelerationMaxS7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _engagedS7;
        public bool? Engaged_S7
        {
            get => _engagedS7;
            set
            {
                if (_engagedS7 == value)
                    return;
                _engagedS7 = value;

                ActiveAdvancedServo.AdvancedServo.servos[7].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }

        private bool? _speedRampingS7;
        public bool? SpeedRamping_S7
        {
            get => _speedRampingS7;
            set
            {
                if (_speedRampingS7 == value)
                    return;
                _speedRampingS7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS7;
        public bool? Stopped_S7
        {
            get => _stoppedS7;
            set
            {
                if (_stoppedS7 == value)
                    return;
                _stoppedS7 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Commands

        public string ConfigFileNameToolTip { get; set; }

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        #endregion

        #region OpenAdvancedServo Command

        public DelegateCommand OpenAdvancedServoCommand { get; set; }
        public string OpenAdvancedServoContent { get; set; } = "Open";
        public string OpenAdvancedServoToolTip { get; set; } = "OpenAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        //public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        public void OpenAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAdvancedServo";

            ActiveAdvancedServo = new AdvancedServoEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedAdvancedServo.SerialNumber,
                SelectedAdvancedServo.Enable,
                SelectedAdvancedServo.Embedded);

            ActiveAdvancedServo.AdvancedServo.Attach += ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach += ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.AdvancedServo.CurrentChange += ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange += ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange += ActiveAdvancedServo_VelocityChange;

            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The AdvancedServoEx attaches to these events also.
            // Itlogs the changes if xxx is set to true.

            //ActiveAdvancedServo.OutputChange += ActiveAdvancedServo_OutputChange;
            //ActiveAdvancedServo.InputChange += ActiveAdvancedServo_InputChange;

            //// NOTE(crhodes)
            //// Let's do see if we can watch some analog data stream in.

            //ActiveAdvancedServo.SensorChange += ActiveAdvancedServo_SensorChange;

            ActiveAdvancedServo.Open();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish(
            //      new OpenAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Subscribe(OpenAdvancedServo);

            // End Cut Four

            //OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            //CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveAdvancedServo_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var velocity = e.Velocity;

            // TODO(crhodes)
            // Wrap in LogVelocityChange

            Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);

            switch (e.Index)
            {
                case 0:
                    Velocity_S0 = e.Velocity;
                    break;

                case 1:
                    Velocity_S1 = e.Velocity;
                    break;

                case 2:
                    Velocity_S2 = e.Velocity;
                    break;

                case 3:
                    Velocity_S3 = e.Velocity;
                    break;

                case 4:
                    Velocity_S4 = e.Velocity;
                    break;

                case 5:
                    Velocity_S5 = e.Velocity;
                    break;

                case 60:
                    Velocity_S6 = e.Velocity;
                    break;

                case 7:
                    Velocity_S7 = e.Velocity;
                    break;

                default:
                    Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveAdvancedServo_PositionChange(object sender, PositionChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var position = e.Position;

            // TODO(crhodes)
            // Wrap in LogPositionChange

            Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);

            switch (e.Index)
            {
                case 0:
                    Position_S0 = e.Position;
                    break;

                case 1:
                    Position_S1 = e.Position;
                    break;

                case 2:
                    Position_S2 = e.Position;
                    break;

                case 3:
                    Position_S3 = e.Position;
                    break;

                case 4:
                    Position_S4 = e.Position;
                    break;

                case 5:
                    Position_S5 = e.Position;
                    break;

                case 60:
                    Position_S6 = e.Position;
                    break;

                case 7:
                    Position_S7 = e.Position;
                    break;

                default:
                    Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveAdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var current = e.Current;

            switch (e.Index)
            {
                case 0:
                    Current_S0 = e.Current;
                    break;

                case 1:
                    Current_S1 = e.Current;
                    break;

                case 2:
                    Current_S2 = e.Current;
                    break;

                case 3:
                    Current_S3 = e.Current;
                    break;

                case 4:
                    Current_S4 = e.Current;
                    break;

                case 5:
                    Current_S5 = e.Current;
                    break;

                case 60:
                    Current_S6 = e.Current;
                    break;

                case 7:
                    Current_S7 = e.Current;
                    break;

                default:
                    // NOTE(crhodes)
                    // Constant stream of this.
                    // Do we really need to monitor current?

                    //Log.Trace($"CurrentChange index:{index} value:{current}", Common.LOG_CATEGORY);
                    break;
            }
        }

        public bool OpenAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedAdvancedServo is not null)
            {
                if (DeviceAttached is not null)
                    return !(Boolean)DeviceAttached;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseAdvancedServo Command

        public DelegateCommand CloseAdvancedServoCommand { get; set; }
        public string CloseAdvancedServoContent { get; set; } = "Close";
        public string CloseAdvancedServoToolTip { get; set; } = "CloseAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string CloseAdvancedServoContent { get; set; } = "ViewName_CloseAdvancedServoContent";
        //public string CloseAdvancedServoToolTip { get; set; } = "ViewName_CloseAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseAdvancedServoContent">CloseAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_CloseAdvancedServoContentToolTip">CloseAdvancedServo ToolTip</system:String>  

        private void ClearDigitalInputsAndOutputs()
        {
            //DI0 = DO0 = null;
            //DI1 = DO1 = null;
            //DI2 = DO2 = null;
            //DI3 = DO3 = null;
            //DI4 = DO4 = null;
            //DI5 = DO5 = null;
            //DI6 = DO6 = null;
            //DI7 = DO7 = null;
            //DI8 = DO8 = null;
            //DI9 = DO9 = null;
            //DI10 = DO10 = null;
            //DI11 = DO11 = null;
            //DI12 = DO12 = null;
            //DI13 = DO13 = null;
            //DI14 = DO14 = null;
            //DI15 = DO15 = null;
        }

        public void CloseAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAdvancedServo";

            ActiveAdvancedServo.AdvancedServo.Attach -= ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach -= ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.Close();
            UpdateAdvancedServoProperties();
            ActiveAdvancedServo = null;
            ClearDigitalInputsAndOutputs();

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish(
            //      new CloseAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Subscribe(CloseAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #endregion

        #region Event Handlers

        private void ActiveAdvancedServo_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveAdvancedServo_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();
        }

        private void UpdateAdvancedServoProperties()
        {

            if (ActiveAdvancedServo.AdvancedServo.Attached)
            {
                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;

                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
                Phidgets.AdvancedServoServo servo = null;

                for (int i = 0; i < servos.Count; i++)
                {
                    servo = servos[i];

                    // TODO(crhodes)
                    // Should probably set some defaults here

                    switch (i)
                    {
                        case 0:
                            Stopped_S0 = servo.Stopped;
                            Engaged_S0 = servo.Engaged;

                            Current_S0 = servo.Current;

                            AccelerationMin_S0 = servo.AccelerationMin;

                            Acceleration_S0 = servo.Acceleration = 20;
                            //Acceleration_S0 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S0 = servo.AccelerationMax;

                            VelocityMin_S0 = servo.VelocityMin;

                            Velocity_S0 = servo.Velocity;

                            VelocityLimit_S0 = servo.VelocityLimit = 20;
                            //VelocityLimit_S0 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S0 = servo.VelocityMax;

                            PositionMin_S0 = servo.PositionMin;

                            Position_S0 = servo.Position = 90;
                            //Position_S0 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S0 = servo.PositionMax;

                            break;

                        case 1:
                            Stopped_S1 = servo.Stopped;
                            Engaged_S1 = servo.Engaged;

                            Current_S1 = servo.Current;

                            AccelerationMin_S1 = servo.AccelerationMin;

                            Acceleration_S1 = servo.Acceleration = 20;
                            //Acceleration_S1 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S1 = servo.AccelerationMax;

                            VelocityMin_S1 = servo.VelocityMin;

                            Velocity_S1 = servo.Velocity;

                            VelocityLimit_S1 = servo.VelocityLimit = 20;
                            //VelocityLimit_S1 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S1 = servo.VelocityMax;

                            PositionMin_S1 = servo.PositionMin;

                            Position_S1 = servo.Position = 90;
                            //Position_S1 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S1 = servo.PositionMax;

                            break;

                        case 2:
                            Stopped_S2 = servo.Stopped;
                            Engaged_S2 = servo.Engaged;

                            Current_S2 = servo.Current;

                            AccelerationMin_S2 = servo.AccelerationMin;

                            Acceleration_S2 = servo.Acceleration = 20;
                            //Acceleration_S2 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S2 = servo.AccelerationMax;

                            VelocityMin_S2 = servo.VelocityMin;

                            Velocity_S2 = servo.Velocity;

                            VelocityLimit_S2 = servo.VelocityLimit = 20;
                            //VelocityLimit_S2 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S2 = servo.VelocityMax;

                            PositionMin_S2 = servo.PositionMin;

                            Position_S2 = servo.Position = 90;
                            //Position_S2 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S2 = servo.PositionMax;

                            break;

                        case 3:
                            Stopped_S3 = servo.Stopped;
                            Engaged_S3 = servo.Engaged;

                            Current_S3 = servo.Current;

                            AccelerationMin_S3 = servo.AccelerationMin;

                            Acceleration_S3 = servo.Acceleration = 20;
                            //Acceleration_S3 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S3 = servo.AccelerationMax;

                            VelocityMin_S3 = servo.VelocityMin;

                            Velocity_S3 = servo.Velocity;

                            VelocityLimit_S3 = servo.VelocityLimit = 20;
                            //VelocityLimit_S3 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S3 = servo.VelocityMax;

                            PositionMin_S3 = servo.PositionMin;

                            Position_S3 = servo.Position = 90;
                            //Position_S3 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S3 = servo.PositionMax;

                            break;

                        case 4:
                            Stopped_S4 = servo.Stopped;
                            Engaged_S4 = servo.Engaged;

                            Current_S4 = servo.Current;

                            AccelerationMin_S4 = servo.AccelerationMin;

                            Acceleration_S4 = servo.Acceleration = 20;
                            //Acceleration_S4 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S4 = servo.AccelerationMax;

                            VelocityMin_S4 = servo.VelocityMin;

                            Velocity_S4 = servo.Velocity;

                            VelocityLimit_S4 = servo.VelocityLimit = 20;
                            //VelocityLimit_S4 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S4 = servo.VelocityMax;

                            PositionMin_S4 = servo.PositionMin;

                            Position_S4 = servo.Position = 90;
                            //Position_S4 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S4 = servo.PositionMax;

                            break;

                        case 5:
                            Stopped_S5 = servo.Stopped;
                            Engaged_S5 = servo.Engaged;

                            Current_S5 = servo.Current;

                            AccelerationMin_S5 = servo.AccelerationMin;

                            Acceleration_S5 = servo.Acceleration = 20;
                            //Acceleration_S5 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S5 = servo.AccelerationMax;

                            VelocityMin_S5 = servo.VelocityMin;

                            Velocity_S5 = servo.Velocity;

                            VelocityLimit_S5 = servo.VelocityLimit = 20;
                            //VelocityLimit_S5 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S5 = servo.VelocityMax;

                            PositionMin_S5 = servo.PositionMin;

                            Position_S5 = servo.Position = 90;
                            //Position_S5 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S5 = servo.PositionMax;

                            break;

                        case 6:
                            Stopped_S6 = servo.Stopped;
                            Engaged_S6 = servo.Engaged;

                            Current_S6 = servo.Current;

                            AccelerationMin_S6 = servo.AccelerationMin;

                            Acceleration_S6 = servo.Acceleration = 20;
                            //Acceleration_S6 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S6 = servo.AccelerationMax;

                            VelocityMin_S6 = servo.VelocityMin;

                            Velocity_S6 = servo.Velocity;

                            VelocityLimit_S6 = servo.VelocityLimit = 20;
                            //VelocityLimit_S6 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S6 = servo.VelocityMax;

                            PositionMin_S6 = servo.PositionMin;

                            Position_S6 = servo.Position = 90;
                            //Position_S6 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S6 = servo.PositionMax;

                            break;

                        case 7:
                            Stopped_S7 = servo.Stopped;
                            Engaged_S7 = servo.Engaged;

                            Current_S7 = servo.Current;

                            AccelerationMin_S7 = servo.AccelerationMin;

                            Acceleration_S7 = servo.Acceleration = 20;
                            //Acceleration_S7 = servo.Acceleration; // Throws exception if not set before reading

                            AccelerationMax_S7 = servo.AccelerationMax;

                            VelocityMin_S7 = servo.VelocityMin;

                            Velocity_S7 = servo.Velocity;

                            VelocityLimit_S7 = servo.VelocityLimit = 20;
                            //VelocityLimit_S7 = servo.VelocityLimit; // Throws exception if not set before reading

                            VelocityMax_S7 = servo.VelocityMax;

                            PositionMin_S7 = servo.PositionMin;

                            Position_S7 = servo.Position = 90;
                            //Position_S7 = servo.Position;         // Throws exception if not set before reading

                            PositionMax_S7 = servo.PositionMax;

                            break;

                        default:
                            Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                            break;

                    }
                }
            }
            else
            {
                DeviceAttached = null;
            }


        }

        private void ActiveAdvancedServo_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;

                Log.Trace($"ActiveAdvancedServo_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveAdvancedServo to null.  Clear UI
                UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

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
