﻿#pragma checksum "..\..\..\..\dialog\DialogPrescriptions.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "294BA6C64F55D6AA87D0BCC6DA97F4614091AC0E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Aplikace.dialog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Aplikace.dialog {
    
    
    /// <summary>
    /// DialogPrescriptions
    /// </summary>
    public partial class DialogPrescriptions : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgPrescription;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDrugName;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSupplement;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbPatient;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbEmployee;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDate;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\dialog\DialogPrescriptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image closeButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Aplikace;component/dialog/dialogprescriptions.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.dgPrescription = ((System.Windows.Controls.DataGrid)(target));
            
            #line 24 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            this.dgPrescription.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgPrescription_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtDrugName = ((System.Windows.Controls.TextBox)(target));
            
            #line 60 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            this.txtDrugName.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtDrugName_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtSupplement = ((System.Windows.Controls.TextBox)(target));
            
            #line 63 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            this.txtSupplement.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtSupplement_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.cmbPatient = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.cmbEmployee = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.dpDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            
            #line 78 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNew_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 79 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Modify_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 80 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Delete_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.closeButton = ((System.Windows.Controls.Image)(target));
            
            #line 83 "..\..\..\..\dialog\DialogPrescriptions.xaml"
            this.closeButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.closeButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

