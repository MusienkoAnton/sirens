using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Net;
using System.Timers;
using System.Windows.Forms;

namespace sirens
{
    public partial class Form1 : Form
    {


        void changeBackColor(Color color)
        {
            this.BackColor = color;
        }

        public Form1()
        {
            InitializeComponent();
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = Int32.Parse(textBox1.Text) * 1000;
            aTimer.Enabled = true;

        }
        private void Form1_Load(object sender, EventArgs e)
        {


            var url = "https://sirens.in.ua/api/v1/";





            using (WebClient webClient = new WebClient())
            {

                var r = webClient.DownloadString(url);
                dynamic dynObj = JsonConvert.DeserializeObject(r);

                JObject obj = JObject.Parse(r);               // Parse the JSON to a JObject
                foreach (var item in obj)
                {
                    checkedListBox1.Items.Add(item.Key);
                }
                foreach (var item in obj)
                {
                    if (item.Value.ToString() != "full" && item.Value.ToString() != "no_data")
                    {
                        Console.WriteLine("Область: {0}, статус: {1}", item.Key, "Відсутня");
                    }
                    else if (item.Value.ToString() == "full")
                    {
                        Console.WriteLine("Область: {0}, статус: {1}", item.Key, "Тривога");
                    }
                    else if (item.Value.ToString() == "partial")
                    {
                        Console.WriteLine("Область: {0}, статус: {1}", item.Key, "Часткова тривога");
                    }
                    else if (item.Value.ToString() == "no_data")
                    {
                        Console.WriteLine("Область: {0}, статус: {1}", item.Key, "Нема даних");
                    }
                }

            }
            //Console.WriteLine(ress);


        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(checkedListBox1.SelectedItem);
          
                this.BackColor = Color.LawnGreen;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //MessageBox.Show(textBox1.Text.ToString(), "Час оновлення", MessageBoxButtons.OK);
            Console.WriteLine("Час оновлення: {0}", textBox1.Text.ToString());
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = Int32.Parse(textBox1.Text) * 1000;
            aTimer.Enabled = true;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            
            Console.WriteLine("Таймер працює");
            var url = "https://sirens.in.ua/api/v1/";
            if (!checkBox3.Checked)
            {
                this.BackColor = Color.LawnGreen;
            }
            using (WebClient webClient = new WebClient())
            {
               
                var r = webClient.DownloadString(url);
                dynamic dynObj = JsonConvert.DeserializeObject(r);

                JObject obj = JObject.Parse(r);
                foreach (var item in obj)
                {
                    try
                    {

                        foreach (var item2 in checkedListBox1.CheckedItems)
                        {
                            if (item2.ToString() == item.Key && (item.Value.ToString() == "full" || item.Value.ToString() == "partial"))
                            {
                                //  try
                                //  {
                                //      changeBackColor(Color.Maroon);
                                //  }
                                //  catch (FormatException exp)
                                //  {
                                //      Console.WriteLine(exp.Message);
                                //  }
                                if (checkBox1.Checked)
                                {
                                    System.Media.SystemSounds.Asterisk.Play();
                                }
                                if (checkBox3.Checked)
                                {
                                    this.BackColor = Color.Red;
                                }
                                if (!checkBox3.Checked)
                                {
                                    this.BackColor = Color.LawnGreen;
                                }
                                if (checkBox2.Checked)
                                {
                                    MessageBox.Show("В регіоні: " + item.Key + " , оголошена ТРИВОГА! Прямуйте до найближчого укриття! Будьте обачні!", "В вибраному регіоні тривога!!!", MessageBoxButtons.OKCancel);

                                }

                            }



                            // else changeBackColor(Color.LawnGreen);
                        }
                    }
                    catch (FormatException exp)
                    {
                        Console.WriteLine(exp.Message);
                    }
                }



           







            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
            {
                this.BackColor = Color.LawnGreen;
            }
        }
    }
}

