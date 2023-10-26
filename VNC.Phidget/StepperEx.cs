﻿using System;

using Phidgets;
using Phidgets.Events;

namespace VNC.Phidget
{
    // TODO(crhodes)
    // Decide if this should be VNCInterfaceKit to distinguish from Phidgets.InterfaceKit

    public class StepperEx : Stepper
    {
        #region Constructors, Initialization, and Load

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public StepperEx(string ipAddress, int port, int serialNumber, bool enable, bool embedded)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            _hostIPAddress = ipAddress;
            _hostPort = port;
            _hostSerialNumber = serialNumber;
            _Embedded = embedded;
            _Enable = enable;

            IntitalizeStepperEx();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void IntitalizeStepperEx()
        {
            this.Attach += Stepper_Attach;
            this.Detach += Stepper_Detach;
            this.Error += Stepper_Error;
            //this.InputChange += Stepper_InputChange;
            //this.OutputChange += Stepper_OutputChange;
            //this.SensorChange += Stepper_SensorChange;
            this.ServerConnect += Stepper_ServerConnect;
            this.ServerDisconnect += Stepper_ServerDisconnect;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        //Phidgets.InterfaceKit interfaceKit = null;

        private bool _Embedded;

        public bool Embedded
        {
            get { return _Embedded; }
            set
            {
                _Embedded = value;
            }
        }

        private bool _Enable;

        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
            }
        }

        private int _hostSerialNumber;

        public int HostSerialNumber
        {
            get { return _hostSerialNumber; }
            set
            {
                _hostSerialNumber = value;
            }
        }

        private string _hostIPAddress;

        public string HostIPAddress
        {
            get { return _hostIPAddress; }
            set
            {
                _hostIPAddress = value;
            }
        }

        private int _hostPort;

        public int HostPort
        {
            get { return _hostPort; }
            set
            {
                _hostPort = value;
            }
        }

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        #endregion

        #region Commands (None)

        #endregion

        #region Event Handlers

        private void Stepper_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            try
            {
                var a = e;
                var b = e.GetType();
                Log.Trace("Stepper_ServerDisconnect", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Stepper_ServerConnect(object sender, ServerConnectEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)e.Device;
                //var b = e.GetType();
                //Log.Trace($"Stepper_ServerConnect {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                Log.Trace($"Stepper_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        //private void Stepper_SensorChange(object sender, SensorChangeEventArgs e)
        //{
        //    if (LogSensorChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    if (LogOutputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    if (LogInputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        private void Stepper_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Stepper_Error {ifk.Address},{ifk.Attached} - {e.Type} {e.Code} {e.Description}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Stepper_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Stepper_Detach {ifk.Address},{ifk.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Stepper_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                AdvancedServo device = (AdvancedServo)sender;
                //Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                Log.Trace($"Stepper_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Public Methods (None)

        public void Open()
        {
            try
            {
                //interfaceKit.open(_hostSerialNumber, _hostIPAddress, _hostPort);

                //interfaceKit.waitForAttachment();
                this.open(_hostSerialNumber, _hostIPAddress, _hostPort);

                this.waitForAttachment();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        public void Close()
        {
            //interfaceKit.close();

            try
            {
                this.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
           
        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        #endregion
    }
}
