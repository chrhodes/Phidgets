using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using dxe = DevExpress.Xpf.Editors;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class InterfaceKit1018 : ViewBase, IInterfaceKit, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public InterfaceKit1018()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            //ViewModel = (IInterfaceKitViewModel)DataContext;

            // Can create directly
            // ViewModel = InterfaceKitViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        public InterfaceKit1018(IInterfaceKitViewModel viewModel)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            ViewModel = viewModel;
            
            InitializeView();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
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

        //private void btn_LoadShow_Click(object sender, RoutedEventArgs e)
        //{
        //    // TODO(crhodes): Parse XML file and build Dictionary of Hosts and Interface Kits
        //    string filePath = ((dxe.ComboBoxEditItem)cbe_ShowLocations.SelectedItem).Content.ToString();

        //    string filePath2 = cbe_ShowLocations.SelectedItem.ToString();
        //    //string fileName = cbe_ShowNames.SelectedItem.ToString();

        //    //LoadShowFromFile(string.Format("{0}\\{1}", filePath, fileName));
        //    LoadShowFromFile(filePath);
        //}

        private void cbe_ShowLocations_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // TODO(crhodes): Populate cbe_ShowNames with files matching pattern
            //cbe_ShowNames.Clear();
            //cbe_ShowNames.Items.BeginUpdate();
            //cbe_ShowNames.Items.Add("Show1.xml");
            //cbe_ShowNames.Items.Add("Show2.xml");
            //cbe_ShowNames.Items.Add("Show3.xml");
            //cbe_ShowNames.Items.EndUpdate();
        }

        private void cbe_ShowName_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // TODO(crhodes): Not sure we need this
        }

        //private void cbeAdvancedServos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    string serialNumber = ((dxe.ComboBoxEdit)sender).SelectedItem.ToString();

        //    //AS = AdvancedServosD[serialNumber];
        //    //teNbrServos.EditValue = AS.servos.Count;
        //    //lgAdvancedServos.DataContext = AS;
        //    cbeAdvancedServos.SelectedIndex = 0;
        //}

        private void cbeInterfaceKits_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - cbeInterfaceKits_SelectedIndexChanged",
                System.Threading.Thread.CurrentThread.ManagedThreadId));

            string serialNumber = ((dxe.ComboBoxEdit)sender).SelectedItem.ToString();

            //IK = InterfaceKitsD[serialNumber];
            //lgInterfaceKits.DataContext = IK;
            //lgAnalogInputs_A0.DataContext = IK.Sensors[0];
            //lgAnalogInputs_A1.DataContext = IK.Sensors[1];
            //lgAnalogInputs_A2.DataContext = IK.Sensors[2];
            //lgAnalogInputs_A3.DataContext = IK.Sensors[3];
            //lgAnalogInputs_A4.DataContext = IK.Sensors[4];
            //lgAnalogInputs_A5.DataContext = IK.Sensors[5];
            //lgAnalogInputs_A6.DataContext = IK.Sensors[6];
            //lgAnalogInputs_A7.DataContext = IK.Sensors[7];
        }

        private void cbeServos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //InitializeServos(AS);
        }

        private void cbeServoTypes_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ceD0A_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());

            //foreach (var item in InterfaceKits)
            //{
            //    string ikName = item.ToString();
            //    InterfaceKitsD[ikName].outputs[index] = true;
            //}
        }

        private void ceD0A_UnChecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());

            //foreach (var item in InterfaceKit)
            //{
            //    string ikName = item.ToString();
            //    InterfaceKitsD[ikName].outputs[index] = false;
            //}
        }

        private void ceDI_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ceDI_UnChecked(object sender, RoutedEventArgs e)
        {

        }

        private void ceDO_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            //InterfaceKitsD[cbeInterfaceKits.Text].outputs[index] = true;
        }

        private void ceDO_UnChecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            //InterfaceKitsD[cbeInterfaceKits.Text].outputs[index] = false;
        }

        private void ceEnableCurrentEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.CurrentChange += advancedServo_CurrentChange;            	
            }
            else
            {
                //AS.CurrentChange -= advancedServo_CurrentChange
            }
        }

        private void ceEnablePositionEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.PositionChange += advancedServo_PositionChange;
            }
            else
            {
                //AS.PositionChange -= advancedServo_PositionChange;
            }
        }

        private void ceEnableVelocityEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.VelocityChange += advancedServo_VelocityChange;
            }
            else
            {
                //AS.VelocityChange -= advancedServo_VelocityChange;
            }
        }

        private void ceEngaged_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Engaging: " + index.ToString());

            //AS.servos[index].Engaged = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());

            //foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //{
            //    int index = int.Parse(item.Content.ToString());
            //    Trace.WriteLine("Engaging:" + index.ToString());
            //    AS.servos[index].Engaged = (bool)ceEngaged.IsChecked;
            //}
        }

        private void ceEngaged_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("DisEngaging: " + index.ToString());

            //AS.servos[index].Engaged = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());

        }

        private void ceSpeedRamping_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Enable SpeedRamping: " + index.ToString());

            //AS.servos[index].SpeedRamping = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());
        }

        private void ceSpeedRamping_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Disable SpeedRamping: " + index.ToString());

            //AS.servos[index].SpeedRamping = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());
        }

//        private void DXWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//#if TRACE
//            //PLLog.Trace5("Start", PLLOG_APPNAME);
//            System.Diagnostics.Debug.WriteLine("DXWindow_Closing_1");
//#endif
//            //UnInitializeAdvancedServos();

//            //// Close all Phidgets we might have opened

//            //foreach (var item in InterfaceKitsD)
//            //{
//            //    ClosePhidget(item.Value);
//            //}
//        }

        private void SpinEdit_DataRate_Spin(object sender, dxe.SpinEventArgs e)
        {

            int currentValue = (int)((dxe.SpinEdit)sender).Value;

            Trace.WriteLine(string.Format("SpinUp:{0}-Value:{1}", e.IsSpinUp.ToString(), currentValue));

            if (e.IsSpinUp)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue * 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue + 8;
                }
            }
            else
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue / 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue - 8;
                }
            }
        }

        private void SpinEdit_Sensitivity_Spin(object sender, dxe.SpinEventArgs e)
        {
            int currentValue = (int)((dxe.SpinEdit)sender).Value;

            Trace.WriteLine(string.Format("SpinUp:{0}-Value:{1}", e.IsSpinUp.ToString(), currentValue));

            if (e.IsSpinUp)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue * 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue + 10;
                }
            }
            else
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue / 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue - 10;
                }
            }
        }

        private void tbeSetAcceleration_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Setting Acceleration:" + index.ToString());
            //        double acceleration = double.Parse(e.NewValue.ToString());
            //        AS.servos[index].Acceleration = acceleration;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }

        private void tbeSetVelocity_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Setting Velocity:" + index.ToString());
            //        double velocity = double.Parse(e.NewValue.ToString());
            //        AS.servos[index].VelocityLimit = velocity;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }

        private void tbeTargetPosition_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //// TODO(crhodes): Decide if want to disenage briefly then reengage.
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Positioning:" + index.ToString());
            //        double position = double.Parse(e.NewValue.ToString());

            //        switch (index)
            //        {
            //            case 0:
            //                position += (double)sePositionAdjustmentS0.Value;
            //                break;

            //            case 1:
            //                position += (double)sePositionAdjustmentS1.Value;
            //                break;

            //            case 2:
            //                position += (double)sePositionAdjustmentS2.Value;
            //                break;

            //            case 3:
            //                position += (double)sePositionAdjustmentS3.Value;
            //                break;

            //            case 4:
            //                position += (double)sePositionAdjustmentS4.Value;
            //                break;

            //            case 5:
            //                position += (double)sePositionAdjustmentS5.Value;
            //                break;

            //            case 6:
            //                position += (double)sePositionAdjustmentS6.Value;
            //                break;

            //            case 7:
            //                position += (double)sePositionAdjustmentS7.Value;
            //                break;
            //        }

            //        AS.servos[index].Position = position;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }
    }
}
