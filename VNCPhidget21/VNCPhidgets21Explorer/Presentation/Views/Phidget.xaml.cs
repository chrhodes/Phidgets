using System;
using System.Windows;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class Phidget : ViewBase, IPhidget, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public Phidget()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            lgPhidgetDevice.DataContext = this;

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IPhidgetViewModel)DataContext;

            // Can create directly
            // ViewModel = PhidgetViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public Phidget(IPhidgetViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountV++;
        //    InitializeComponent();

        //    ViewModel = viewModel;

        //    InitializeView();

        //    Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        private static object OnCoerceDevicePort(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDevicePort((Int32?)value);
            else
                return value;
        }

        private static void OnDevicePortChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDevicePortChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }

        protected virtual Int32? OnCoerceDevicePort(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDevicePortChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceDeviceVersion(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceVersion((Int32)value);
            else
                return value;
        }

        private static void OnDeviceVersionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceVersionChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceDeviceVersion(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceVersionChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceType(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceType((string)value);
            else
                return value;
        }

        private static void OnDeviceTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceTypeChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceDeviceType(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceTypeChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceSerialNumber(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceSerialNumber((Int32)value);
            else
                return value;
        }

        private static void OnDeviceSerialNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceSerialNumberChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceDeviceSerialNumber(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceSerialNumberChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceDeviceClass(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceClass((string)value);
            else
                return value;
        }

        private static void OnDeviceClassChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceClassChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceDeviceClass(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceClassChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceAddress(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceAddress((string)value);
            else
                return value;
        }

        private static void OnDeviceAddressChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceAddressChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceDeviceAddress(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceAddressChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceAttached(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceAttached((Boolean)value);
            else
                return value;
        }

        private static void OnDeviceAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceDeviceAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            DeviceName = PhidgetDevice.Name;
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceName(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoerceDeviceName((string)value);
            else
                return value;
        }

        private static void OnDeviceNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnDeviceNameChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceDeviceName(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceNameChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoercePhidgetDevice(DependencyObject o, object value)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                return phidget.OnCoercePhidgetDevice((Phidgets.Phidget)value);
            else
                return value;
        }

        private static void OnPhidgetDeviceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget phidget = o as Phidget;
            if (phidget != null)
                phidget.OnPhidgetDeviceChanged((Phidgets.Phidget)e.OldValue, (Phidgets.Phidget)e.NewValue);
        }

        protected virtual Phidgets.Phidget OnCoercePhidgetDevice(Phidgets.Phidget value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPhidgetDeviceChanged(Phidgets.Phidget oldValue, Phidgets.Phidget newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            // TODO(crhodes)
            // Put this in attach
            if (newValue.Attached)
            {
                DeviceAddress = newValue.Address;
                //newValue.AttachedToServer;
                DeviceClass = newValue.Class.ToString();
                DeviceName = newValue.Name;
                DevicePort = newValue.Port;
                DeviceSerialNumber = newValue.SerialNumber;
                DeviceType = newValue.Type;
                DeviceVersion = newValue.Version;
            }
            else
            {
                DeviceAddress = "";
                DeviceClass = "";
                DeviceName = "";
                DevicePort = null;

            }
        }

        private void InitializeView()
        {
            Int64 startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View

            Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)

        //public static readonly DependencyProperty PhidgetProperty = DependencyProperty.Register("Phidget", typeof(Phidgets.Phidget), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPhidgetChanged), new CoerceValueCallback(OnCoercePhidget)));

        public static readonly DependencyProperty PhidgetDeviceProperty = DependencyProperty.Register("PhidgetDevice", typeof(Phidgets.Phidget), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPhidgetDeviceChanged), new CoerceValueCallback(OnCoercePhidgetDevice)));

        public Int32? DevicePort
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DevicePortProperty);
            set => SetValue(DevicePortProperty, value);
        }
        public Int32? DeviceVersion
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DeviceVersionProperty);
            set => SetValue(DeviceVersionProperty, value);
        }
        public string DeviceType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceTypeProperty);
            set => SetValue(DeviceTypeProperty, value);
        }
        public Int32? DeviceSerialNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DeviceSerialNumberProperty);
            set => SetValue(DeviceSerialNumberProperty, value);
        }

        public string DeviceClass
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceClassProperty);
            set => SetValue(DeviceClassProperty, value);
        }
        public string DeviceAddress
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceAddressProperty);
            set => SetValue(DeviceAddressProperty, value);
        }
        public Boolean DeviceAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(DeviceAttachedProperty);
            set => SetValue(DeviceAttachedProperty, value);
        }
        public string DeviceName
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceNameProperty);
            set => SetValue(DeviceNameProperty, value);
        }
        public Phidgets.Phidget PhidgetDevice
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.Phidget)GetValue(PhidgetDeviceProperty);
            set => SetValue(PhidgetDeviceProperty, value);
        }

        public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(Phidget), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceNameChanged), new CoerceValueCallback(OnCoerceDeviceName)));

        public static readonly DependencyProperty DeviceAttachedProperty = DependencyProperty.Register("DeviceAttached", typeof(Boolean), typeof(Phidget), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnDeviceAttachedChanged), new CoerceValueCallback(OnCoerceDeviceAttached)));

        public static readonly DependencyProperty DeviceAddressProperty = DependencyProperty.Register("DeviceAddress", typeof(string), typeof(Phidget), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceAddressChanged), new CoerceValueCallback(OnCoerceDeviceAddress)));

        public static readonly DependencyProperty DeviceClassProperty = DependencyProperty.Register("DeviceClass", typeof(string), typeof(Phidget), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceClassChanged), new CoerceValueCallback(OnCoerceDeviceClass)));

        public static readonly DependencyProperty DeviceSerialNumberProperty = DependencyProperty.Register("DeviceSerialNumber", typeof(Int32?), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceSerialNumberChanged), new CoerceValueCallback(OnCoerceDeviceSerialNumber)));


        public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(string), typeof(Phidget), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceTypeChanged), new CoerceValueCallback(OnCoerceDeviceType)));
        public static readonly DependencyProperty DeviceVersionProperty = DependencyProperty.Register("DeviceVersion", typeof(Int32?), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceVersionChanged), new CoerceValueCallback(OnCoerceDeviceVersion)));



        public static readonly DependencyProperty DevicePortProperty = DependencyProperty.Register("DevicePort", typeof(Int32?), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDevicePortChanged), new CoerceValueCallback(OnCoerceDevicePort)));
        
        


        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods (None)


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion

        #region IInstanceCount

        private static int _instanceCountV;

        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        #endregion

    }
}
