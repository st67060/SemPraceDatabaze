﻿#pragma checksum "..\..\..\..\dialog\DialogVisits.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FC005EFE6D7DD4FAEDA9E6320FC7DDD6D7687E41"
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
    /// DialogVisits
    /// </summary>
    public partial class DialogVisits : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\dialog\DialogVisits.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgVisits;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\dialog\DialogVisits.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDate;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\dialog\DialogVisits.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDetails;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\dialog\DialogVisits.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbPatient;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\dialog\DialogVisits.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image closeButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Aplikace;component/dialog/dialogvisits.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\dialog\DialogVisits.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.dgVisits = ((System.Windows.Controls.DataGrid)(target));
            
            #line 24 "..\..\..\..\dialog\DialogVisits.xaml"
            this.dgVisits.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgVisits_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dpDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.txtDetails = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cmbPatient = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            
            #line 46 "..\..\..\..\dialog\DialogVisits.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNew_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 47 "..\..\..\..\dialog\DialogVisits.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Modify_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 48 "..\..\..\..\dialog\DialogVisits.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Delete_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.closeButton = ((System.Windows.Controls.Image)(target));
            
            #line 51 "..\..\..\..\dialog\DialogVisits.xaml"
            this.closeButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.closeButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

