using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ChampionSelection
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StackPanel _mainPanel = new StackPanel();
            this.Content = _mainPanel;
            this.DataContext = new MainWindowViewModel(_mainPanel);
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
