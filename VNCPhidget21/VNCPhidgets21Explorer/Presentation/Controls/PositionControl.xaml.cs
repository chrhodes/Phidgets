﻿using System;
using System.Windows;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using DevExpress.Xpf.Editors;

namespace VNCPhidgets21Explorer.Presentation.Controls
{
    public partial class PositionControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public PositionControl()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();
            
            lgMain.DataContext = this;

			// Expose ViewModel
						
            // If View First with ViewModel in Xaml

            // ViewModel = (IPositionControlViewModel)DataContext;

            // Can create directly
            // ViewModel = PositionControlViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        // public PositionControl(IPositionControlViewModel viewModel)
        // {
            // Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            // InstanceCountV++;
            // InitializeComponent();

            // ViewModel = viewModel;
            
            // InitializeView();

            // Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        // }
        
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

        public Double? Current
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }
        public Double? Min
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public Double? Max
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMaxChanged), new CoerceValueCallback(OnCoerceMax)));

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMinChanged), new CoerceValueCallback(OnCoerceMin)));

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCurrentChanged), new CoerceValueCallback(OnCoerceCurrent)));
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


        private static object OnCoerceCurrent(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceCurrent(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnCurrentChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceMin(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMin((Double?)value);
            else
                return value;
        }

        private static void OnMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnMinChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMin(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceMax(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMax((Double?)value);
            else
                return value;
        }

        private static void OnMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnMaxChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMax(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }


        #endregion

        #region IInstanceCount

        private static int _instanceCountV;

        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        #endregion

        private void TrackBarEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var centerPosition = (Max - Min) / 2 + Min;
            ((TrackBarEdit)sender).Value = (Double)centerPosition;
        }
    }
}