
namespace LolPlayButtonMaui.Component
{
    public class ToggleButton : TemplatedView
    {
        public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(Switch), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((ToggleButton)bindable).Toggled?.Invoke(bindable, new ToggledEventArgs((bool)newValue));
            ((ToggleButton)bindable).ChangeVisualState();
            ((IView)bindable)?.Handler?.UpdateValue(nameof(ISwitch.TrackColor));

        }, defaultBindingMode: BindingMode.TwoWay);

        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        public event EventHandler<ToggledEventArgs> Toggled;

        TapGestureRecognizer _tapGestureRecognizer;
        public ToggleButton()
        {
            _tapGestureRecognizer = new TapGestureRecognizer();
            _tapGestureRecognizer.Tapped += SelectRadioButton;

            this.GestureRecognizers.Add (_tapGestureRecognizer);
        }

        void SelectRadioButton(object? sender, TappedEventArgs e)
        {
            IsToggled = !IsToggled;
        }
    }
}
