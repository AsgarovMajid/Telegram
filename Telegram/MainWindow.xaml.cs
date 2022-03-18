
using AIMLbot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace Telegram
{
   
    public partial class MainWindow : Window
    {
        public class Human
        {

            public Human(string name, string time, string imageurl)
            {
                this.name = name;
                this.time = time;
                this.imageurl = imageurl;
            }

            public string name { get; set; }
            public string time { get; set; }
            public List<string> data { get; set; }
            public string imageurl { get; set; }

        }
        List<Human> contact = new List<Human>();
        string copyimage2;
        DateTime date = new DateTime();

      

        public MainWindow()
        {
            InitializeComponent();
            string value = date.ToShortTimeString();
            if (File.Exists("contact.json") == true)
            {

                contact.Add(new Human("Kamil","6:30 AM", "https://kidzartworx.com.au/wp-content/uploads/2017/01/circle-man.png"));
                contact.Add(new Human("Murad", "11:03 AM", "http://tricky-photoshop.com/wp-content/uploads/2017/08/final-1.png"));
                contact.Add(new Human("Musa", "9:43 AM", "https://pngimage.net/wp-content/uploads/2019/05/man-profile-png-2.png"));
                contact.Add(new Human("Anar", "8:21 pM", "http://logan-marshall.com/wp-content/uploads/2016/08/Circle-Profile-No-Background-PNG-120dpi-page001-768x768.png"));
                contact.Add(new Human("Elsen", "6:05 AM", "https://upload.wikimedia.org/wikipedia/commons/d/d7/Walker_Circle.png"));
                contact.Add(new Human("Zamin", "3:44 pM", "https://upload.wikimedia.org/wikipedia/commons/e/eb/Rubio_Circle.png"));
                contact.Add(new Human("Mecid", "7:11 AM", "https://www.pngkit.com/png/full/639-6399637_henry-circle-gentleman.png"));
            }
            else
            {
                var dialog = File.ReadAllText($"contact.json");
                contact = JsonConvert.DeserializeObject<List<Human>>(dialog);

            }
        
            humans.ItemsSource = contact;

            DataContext = this;
        }
      
        List<string> zapas = new List<string>();
        string gelenmesaj;
        string qarsiinsan;
        DispatcherTimer timer = new DispatcherTimer();
       
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
             
        
            Label gonderiren=new Label();
            
            Color c = Colors.White;
            


            gonderiren.Background=new SolidColorBrush(c);
            gonderiren.Content = $" You : {mesaj.Text}";
            zapas.Add($" You : {mesaj.Text}");
            Chat.HorizontalAlignment = HorizontalAlignment.Stretch;
            gonderiren.HorizontalContentAlignment= HorizontalAlignment.Right;

            Chat.Items.Add(gonderiren);
    
            timer.Start();

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            mesaj.IsEnabled = false;

           

            var contactjson = JsonConvert.SerializeObject(contact, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("contact.json", contactjson);

        }
        void timer_Tick(object sender, EventArgs e)
        {
            Color c = Colors.Transparent;
            Bot AI = new Bot();

 
            AI.loadSettings();
            AI.loadAIMLFromFiles();

            AI.isAcceptingUserInput = false;

            User myUser = new User("Username", AI);

            AI.isAcceptingUserInput = true;

            Request r = new Request(mesaj.Text, myUser, AI);

            Result res = AI.Chat(r);

            gelenmesaj =$"{qarsiinsan}: {res.Output}" ;


            Label GELEN = new Label();
            GELEN.Content = gelenmesaj;

            GELEN.HorizontalContentAlignment = HorizontalAlignment.Left;
            
            zapas.Add($"{gelenmesaj}");
            
            GELEN.Background = new SolidColorBrush(c);
            Chat.Items.Add(GELEN);
         
            timer.Stop();

            mesaj.Text = null;
            mesaj.IsEnabled = true;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            send.IsEnabled = true;
            string value = date.ToShortTimeString();
            
            basliq.Content = (sender as Button).Tag.ToString();
            Chat.Items.Clear();
            qarsiinsan = (sender as Button).Tag.ToString();
            foreach (Human human in contact)
            {
                copyimage2 = (human as Human).imageurl;
                if ((human as Human).name == qarsiinsan  && human.data!=null)
                {
                    (human as Human).time = value;
                   

                    foreach (var item in human.data)
                    {
                        Label gonderiren = new Label();
                        Color c = Colors.Cyan;


                        gonderiren.Background = new SolidColorBrush(c);
                        gonderiren.Content= item;
                        Chat.Items.Add(gonderiren);

                    }
                    break;
                }
                
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(copyimage2, UriKind.Absolute);
            bitmap.EndInit();
            copyimage.Source = bitmap;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            send.IsEnabled = false;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {    
            emojis.Visibility= Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mesaj.Text += (sender as Button).Content;
        }

        private void exitemoji_Click(object sender, RoutedEventArgs e)
        {
            emojis.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Chat.Items.Clear();
            foreach (Human human in contact)
            {
                if ((human is Human a))
                {
                    if (a.name == qarsiinsan )
                    {
                        a.data = null;                
                    }

                }
            }

            var contactjson = JsonConvert.SerializeObject(contact, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("contact.json", contactjson);
        }


    }
}
