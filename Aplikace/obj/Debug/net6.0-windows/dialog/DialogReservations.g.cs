﻿#pragma checksum "..\..\..\..\dialog\DialogReservations.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F21CA36179C60FF9F2B1AE3523151C2411FA7D09"
//------------------------------------------------------------------------------
// <auto-generated>
//     Tento kód byl generován nástrojem.
//     Verze modulu runtime:4.0.30319.42000
//
//     Změny tohoto souboru mohou způsobit nesprávné chování a budou ztraceny,
//     dojde-li k novému generování kódu.
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
    /// DialogReservations
    /// </summary>
    public partial class DialogReservations : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgReservations;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDate;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNotes;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbPatient;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbEmployee;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\dialog\DialogReservations.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image closeButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Aplikace;component/dialog/dialogreservations.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\dialog\DialogReservations.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\dialog\DialogReservations.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgReservations = ((System.Windows.Controls.DataGrid)(target));
            
            #line 24 "..\..\..\..\dialog\DialogReservations.xaml"
            this.dgReservations.SourceUpdated += new System.EventHandler<System.Windows.Data.DataTransferEventArgs>(this.dgReservations_SourceUpdated);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dpDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.txtNotes = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.cmbPatient = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.cmbEmployee = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            
            #line 51 "..\..\..\..\dialog\DialogReservations.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNew_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 52 "..\..\..\..\dialog\DialogReservations.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Modify_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 53 "..\..\..\..\dialog\DialogReservations.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Delete_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.closeButton = ((System.Windows.Controls.Image)(target));
            
            #line 57 "..\..\..\..\dialog\DialogReservations.xaml"
            this.closeButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

