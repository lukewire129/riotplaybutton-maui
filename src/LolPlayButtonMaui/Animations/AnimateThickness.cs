using AlohaKit.Animations;
using AlohaKit.Animations.Helpers;

namespace LolPlayButtonMaui.Animations
{
    public interface IAnimate
    {
        Task Invoke(Type sender);
    }
    public abstract class AnimateBase<T> : IAnimate
    {        
        public T From { get; set; }
        public T To { get; set; }
        public uint Duration { get; set; } = 1000u;
        public int Delay { get; set; }
        public EasingType Easing { get; set; } = EasingType.Linear;
        public BindableProperty TargetProperty { get; set; }

        public abstract Task Invoke(Type sender);
    }

    [ContentProperty(nameof(Animations))]
    public class StoryBoardAnimate: BindableObject
    {
        public static readonly BindableProperty TargetProperty = BindableProperty.Create ("Target", typeof (Type), typeof (StoryBoardAnimate), null, BindingMode.TwoWay);
        public Type Target
        {
            get
            {
                return (Type)GetValue (TargetProperty);
            }
            set
            {
                SetValue (TargetProperty, value);
            }
        }

        public List<IAnimate> Animations { get; }


        public StoryBoardAnimate()
        {
            Animations = new List<IAnimate> ();
        }

        public StoryBoardAnimate(List<IAnimate> animations)
        {
            Animations = animations;
        }

        protected async Task Begin()
        {
            foreach (IAnimate animation in Animations)
            {
                await animation.Invoke (Target);
            }
        }
    }
    public class AnimateThickness : AnimationBaseTrigger<Thickness>
    {
        protected override async void Invoke(VisualElement sender)
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException ("Null Target property.");
            }

            if (Delay > 0)
                await Task.Delay (Delay);

            SetDefaultFrom ((Thickness)sender.GetValue (TargetProperty));

            sender.Animate ($"AnimateThickness{TargetProperty.PropertyName}", new Animation ((progress) =>
            {
                sender.SetValue (TargetProperty, AnimationHelper.GetThicknessValue (From, To, progress));
            }),
            length: Duration,
            easing: EasingHelper.GetEasing (Easing));
        }
    }
}
