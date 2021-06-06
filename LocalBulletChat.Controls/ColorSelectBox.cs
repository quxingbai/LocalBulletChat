using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalBulletChat.Controls
{
    public class ColorSelectBox : ListBox
    {




        public Double ColorItemWidth
        {
            get { return (Double)GetValue(ColorItemWidthProperty); }
            set { SetValue(ColorItemWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorItemWidthProperty =
            DependencyProperty.Register("ColorItemWidth", typeof(Double), typeof(ColorSelectBox), new PropertyMetadata(70.0));



        public Double ColorItemHeight
        {
            get { return (Double)GetValue(ColorItemHeightProperty); }
            set { SetValue(ColorItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorItemHeightProperty =
            DependencyProperty.Register("ColorItemHeight", typeof(Double), typeof(ColorSelectBox), new PropertyMetadata(50.0));


        public String Colors
        {
            get { return (String)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Colors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register("Colors", typeof(String), typeof(ColorSelectBox), new PropertyMetadata("Red,Black,Yellow,Green"));

        public Brush SelectedBrush { get =>new BrushConverter().ConvertFromString((SelectedItem??"Black").ToString()) as Brush; }
        static ColorSelectBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSelectBox), new FrameworkPropertyMetadata(typeof(ColorSelectBox)));
        }
        public override void OnApplyTemplate()
        {
            ItemsSource = Colors.Split(',');
            base.OnApplyTemplate();
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ColorsProperty == e.Property)
            {
                ItemsSource = Colors.Split(',');
            }
            base.OnPropertyChanged(e);
        }
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            ScrollIntoView(SelectedItem);
            base.OnSelectionChanged(e);
        }

    }
}
