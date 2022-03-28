using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KanashNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        public News(String label, String text, String img)
        {
            InitializeComponent();
            init(label, text, img);
        }
        private void init(String label, String text, String img)
        {
            Image image = new Image();
            image.Source = img;
            image.Aspect = Aspect.AspectFill;

            Label l = new Label();
            l.Text = label;
            l.FontSize = 30;

            StackLayout st = new StackLayout();
            String[] sp = text.Split('\n');
            for(int i = 0; i < sp.Length; i++)
            {
                Label t = new Label();
                t.Text = sp[i];
                t.HorizontalOptions = LayoutOptions.Center;
                st.Children.Add(t);
            }

            nws.Children.Add(image);
            nws.Children.Add(l);
            nws.Children.Add(st);
        }
    }
}