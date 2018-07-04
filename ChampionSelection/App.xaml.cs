using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.Markup.Xaml.Data;
using System.Collections.Generic;
using Avalonia.Logging.Serilog;


namespace ChampionSelection
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
