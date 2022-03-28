using Android.Util;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.CSharp;



namespace KanashNews
{
    public partial class MainPage : ContentPage
    {
        //Ссылка на сервер с json
        public int pl = 1;
        public String URL = "http://45.93.200.235/getnews/";
        public MainPage()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            GetAndpush(URL, true);




            Label label = new Label();
            label.Text = "Загрузка...";
            label.FontSize = 30;
            label.HorizontalOptions = LayoutOptions.Center;
            News.Children.Add(label);

            //append(prepare("О верных служителях Мельпомены", "http://kanashen.ru/wp-content/uploads/2022/03/u_Kuc9fKmwQ-1200x630.jpg"));
            //append(prepare("На старт вышли более 200 лыжников", "http://kanashen.ru/wp-content/uploads/2022/03/hd_004-cm44x3mx-1200x630.jpg"));
        }
        public Boolean boole = false;
        //Метод получает json с ссылки и сует в StackLayout
        public void GetAndpush(String url,Boolean clear)
        {
            Task.Run(() =>
            {
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url);
                    Log.Info("Json", "Json is downloaded");

                    dynamic news = JsonConvert.DeserializeObject(json);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if(clear)
                            News.Children.Clear();
                        int i = 0;
                        var nws = news[i];
                        while (true)
                        {
                            nws = news[i];
                            Log.Info("Load", Convert.ToString(nws.label) + " " + i);
                            append(prepare(Convert.ToString(nws.label), Convert.ToString(nws.img)));
                            i++;
                            try
                            {
                                nws = news[i];
                            }
                            catch (Exception)
                            {
                                break;
                            };
                        }
                        boole = true;
                        btn.Text = "Загружить еще новости";
                    });





                }
            }
            );
        }

        //Метод доблавляющий в Stacklayout новость
        public void append(StackLayout stack)
        {
            News.Children.Add(stack);
        }

      
        //Метод возращающий готовый StackLayout, делает умные вещи
        public StackLayout prepare(String text,String srcimg)
        {
            StackLayout stack = new StackLayout();
            stack.BackgroundColor = Color.LightCoral;
            stack.Orientation = StackOrientation.Horizontal;
            stack.Margin = 5;

            Frame frame = new Frame();
            frame.BackgroundColor = Color.LightBlue;
            Label label = new Label();
            label.Text = text;
            frame.Content = label;

            Image image = new Image();
            image.Source = srcimg;
            image.Aspect = Aspect.AspectFill;
            image.HeightRequest = 60;

            stack.Children.Add(frame);
            stack.Children.Add(image);

            return stack;
        }

        
       
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (boole)
            {
                Log.Info("Btn", "Start GetPush");
                btn.Text = "подождите";
                boole = false;
                pl++;
                GetAndpush(URL + pl, false);
            }
        }
    }
}
