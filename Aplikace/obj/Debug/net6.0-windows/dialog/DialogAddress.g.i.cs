﻿#pragma checksum "..\..\..\..\dialog\DialogAddress.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5FB2E868E9AD0A9C76713D3EDB7E242104EFA4AE"
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
    /// DialogAddress
    /// </summary>
    public partial class DialogAddress : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgAddress;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spForm;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCity;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPostalCode;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStreetNumber;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCountry;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\dialog\DialogAddress.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStreet;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\dialog\DialogAddress.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Aplikace;component/dialog/dialogaddress.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\dialog\DialogAddress.xaml"
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
            this.dgAddress = ((System.Windows.Controls.DataGrid)(target));
            
            #line 25 "..\..\..\..\dialog\DialogAddress.xaml"
            this.dgAddress.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgAddress_SelectionChanged_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.spForm = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.txtCity = ((System.Windows.Controls.TextBox)(target));
            
            #line 38 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtCity.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtCity_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtPostalCode = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtPostalCode.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtPostalCode_TextChanged);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtPostalCode.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtPostalCode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtStreetNumber = ((System.Windows.Controls.TextBox)(target));
            
            #line 44 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtStreetNumber.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtStreetNumber_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtCountry = ((System.Windows.Controls.TextBox)(target));
            
            #line 47 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtCountry.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtCountry_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtStreet = ((System.Windows.Controls.TextBox)(target));
            
            #line 50 "..\..\..\..\dialog\DialogAddress.xaml"
            this.txtStreet.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtStreet_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 54 "..\..\..\..\dialog\DialogAddress.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNew_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 55 "..\..\..\..\dialog\DialogAddress.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Modify_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 56 "..\..\..\..\dialog\DialogAddress.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Delete_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.closeButton = ((System.Windows.Controls.Image)(target));
            
            #line 59 "..\..\..\..\dialog\DialogAddress.xaml"
            this.closeButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.closeButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

