﻿#pragma checksum "..\..\..\..\Editors\D2I\D2IEditor.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7200CA4B57DC545FAB559CA4D1D646D2AFB226A8"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace WorldEditor.Editors.Files.D2I {
    
    
    /// <summary>
    /// D2IEditor
    /// </summary>
    public partial class D2IEditor : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition ExpanderRow;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander EditExpander;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid TextsGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WorldEditor;component/editors/d2i/d2ieditor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ExpanderRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 2:
            this.EditExpander = ((System.Windows.Controls.Expander)(target));
            
            #line 56 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
            this.EditExpander.Expanded += new System.Windows.RoutedEventHandler(this.Expander_Expanded);
            
            #line default
            #line hidden
            
            #line 56 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
            this.EditExpander.Collapsed += new System.Windows.RoutedEventHandler(this.Expander_Collapsed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TextsGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 91 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
            this.TextsGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TextsGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 110 "..\..\..\..\Editors\D2I\D2IEditor.xaml"
            ((System.Windows.Controls.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

