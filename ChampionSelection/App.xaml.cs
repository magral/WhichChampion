using Avalonia;
using Avalonia.Markup.Xaml;

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
