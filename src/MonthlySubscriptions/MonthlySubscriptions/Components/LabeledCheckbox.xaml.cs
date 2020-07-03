using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions.Components 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabeledCheckbox : Grid
    {

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(TitleText), typeof(string), typeof(LabeledCheckbox), propertyChanged: TitleChanged);

        public static readonly BindableProperty DescriptionTextProperty = BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(LabeledCheckbox), default(string), propertyChanged: DescriptionChanged);

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(LabeledCheckbox), default(bool), propertyChanged: IsCheckedChanged);

        public static readonly BindableProperty CheckedCommandProperty = BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(LabeledCheckbox), null);
        
        private readonly TapGestureRecognizer _tapGestureRecognizer;

        public ICommand CheckedCommand
        {
            get { return (ICommand)GetValue(CheckedCommandProperty); }
            set { SetValue(CheckedCommandProperty, value); }
        }

        public string TitleText { get => titleLbl.Text; set => titleLbl.Text = value; }

        public string DescriptionText { get => descriptionLbl.Text; set => descriptionLbl.Text = value; }

        public bool IsChecked { get => checkbox.IsChecked; set => checkbox.IsChecked = value; }

        public LabeledCheckbox()
        {
            _tapGestureRecognizer = new TapGestureRecognizer();
            GestureRecognizers.Add(_tapGestureRecognizer);
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent is null)
                _tapGestureRecognizer.Tapped -= OnTapped;
            else
                _tapGestureRecognizer.Tapped += OnTapped;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
            if (CheckedCommand?.CanExecute(IsChecked) == true) CheckedCommand.Execute(IsChecked);
        }

        private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((LabeledCheckbox)bindable).TitleText = newValue.ToString();
        }

        private static void DescriptionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((LabeledCheckbox)bindable).DescriptionText = newValue.ToString();
        }

        private static void IsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((LabeledCheckbox)bindable).IsChecked = (bool)newValue;
        }

    }
}