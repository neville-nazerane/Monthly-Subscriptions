﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectionView : RelativeLayout
    {

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(TitleText), typeof(string), typeof(SectionView), propertyChanged: TitleTextChanged);

        private View _title;
        private string _titleText;
        private View _body;

        public View Title { get => _title; set => SetTitle(value); }

        public View Body { get => _body; set => SetBody(value); }

        public Thickness BodyPadding { get => bodyContainer.Padding; set => bodyContainer.Padding = value; }

        public string TitleText { get => _titleText; set => SetTitleText(value); }

        public SectionView()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent is null)
                bodyFrame.SizeChanged -= BodyFrame_SizeChanged;
            else
                bodyFrame.SizeChanged += BodyFrame_SizeChanged;
        }

        private void BodyFrame_SizeChanged(object sender, EventArgs e)
        {
            var padding = bodyFrame.Padding;
            padding.Top = bodyFrame.Y;
            bodyFrame.Padding = padding;
        }

        private void SetTitle(View title)
        {
            _title = title;
            titleFrame.Content = title;
        }

        private void SetBody(View body)
        {
            _body = body;
            bodyContainer.Content = body;
        }

        private void SetTitleText(string text)
        {
            _titleText = text;
            titleLbl.Text = text;
        }

        private static void TitleTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SectionView)bindable).SetTitleText(newValue.ToString());
        }

    }
}