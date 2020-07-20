using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Генератор_данных
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateFIO();

        }

        private void GenerateFIO()
        {
            Thread myThread = new Thread(ReqFIO);
            myThread.Start();
        }

        private void ReqFIO()
        {
            List<string> text;
            string famReq = String.Empty;
            char[] letter = "АБВГДЕИКЛМНОПРСТФЭ".ToCharArray();
            Random rand = new Random();
            string URI = @"http://megagenerator.ru/namefio/";
            string count = textBox1.Text;
            string Parameters = "test[]=1&test[]=fam&quantity=" + count + "&select=russia";
            try
            {
                famReq = POST(URI, Parameters);
            }
            catch (Exception e)
            {
                richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.AppendText(e.Message)));
            }

            famReq = famReq.Replace("br>", "");
            text = famReq.Split('<').ToList();
            text.RemoveAt(text.Count - 1);

            if (radioButton1.Checked == true)
            {
                int massLength = letter.Length;
                string s = string.Empty;

                for (int i = 0; i < text.Count; i++)
                {
                    s = " " + letter[rand.Next(massLength)] + "." + letter[rand.Next(massLength)] + ".";

                    Thread.Sleep(30);

                    text[i] = text[i] + s;
                }

                richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Lines = text.ToArray()));
            }

            if (radioButton2.Checked == true)
            {
                richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Lines = text.ToArray()));
            }

            if (radioButton3.Checked == true)
            {
            }

            if (radioButton6.Checked == true)
            {
                GenerateName();
            }
        }

        void GenerateName()
        {
            richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Clear()));
            int c = Convert.ToInt32(textBox1.Text);
            string url = "http://znachenie-tajna-imeni.ru/top-50-muzhskih-imen/";
            string html = GetCode(url);
            List<string> names = new List<string>();
            Regex regex = new Regex(@"/'>\D*</a></td>", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(html);
            string temp = string.Empty;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    temp += match.Value;
                    temp = Regex.Replace(temp, @"[\r\n\t>\<a><td>']", "");
                
                }
                names = temp.Split('/').ToList();
            }
            var uniq = names.Distinct();
            names = uniq.ToList();
            names.RemoveAt(0);
            List<string> names2 = new List<string>();
            Random rand = new Random();
            for (int i = 0; i < c; i++)
            {
                names2.Add(names[rand.Next(names.Count)]);
            }


            richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Lines = names2.ToArray()));


        }

        private string POST(string Url, string Data)
        {
            WebRequest req = WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 3000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
            //Кодировка указывается в зависимости от кодировки ответа сервера
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PhoneGenerate();
        }

        private void PhoneGenerate()
        {
            int count = Convert.ToInt32(textBox4.Text);
            string[] pref = { "928", "938", "908", "951", "918", "905" };
            string s;
            Random rand = new Random();
            List<string> list = new List<string>();
            for (int i = 0; i < count; i++)
            {
                if (textBox3.Text == "* * *")
                {
                    s = textBox2.Text + pref[rand.Next(pref.Length)] + (rand.Next(8900000) + 1000000);
                }
                else
                {
                    s = textBox2.Text + textBox3.Text + (rand.Next(8900000) + 1000000);
                }

                list.Add(s);
            }

            richTextBox2.Lines = list.ToArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetStreet();
        }

        private void GetStreet()
        {
            richTextBox3.Clear();
            string url = "https://ru.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D1%83%D0%BB%D0%B8%D1%86_%D0%91%D1%80%D0%B0%D1%82%D1%81%D0%BA%D0%B0";
            string html = GetCode(url);
            List<string> street = new List<string>();
            Regex regex = new Regex(@"<li>\D*, у", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(html);
            string temp = string.Empty;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    temp += match.Value;
                    temp = Regex.Replace(temp, @"[\r\n\tli><]", "");
                }
                street = temp.Split('/').ToList();

                for (int i = 0; i < street.Count; i++)
                {
                     if (street[i].IndexOf(',') != -1)
                        street[i] = street[i].Remove(street[i].IndexOf(','));
                }
            }


            Random rand = new Random();
            if (radioButton4.Checked == true) { }
            

            if (radioButton5.Checked == true)
            {
                
                for (int i = 0; i < street.Count; i++)
                {
                    street[i] = street[i] + ", " +(rand.Next(200) + 1);
                }

            }
            for (int i = 0; i < Convert.ToInt32(textBox5.Text); i++)
            {
                richTextBox3.AppendText(street[rand.Next(street.Count)] + Environment.NewLine);
            }
        }

        private String GetCode(string urlAddress)
        {
            //string urlAddress = "http://google.com";
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GenerateDate();
        }

        void GenerateDate()
        {
            int a = Convert.ToInt32(textBox6.Text);
            int b = Convert.ToInt32(textBox7.Text);
            int c = Convert.ToInt32(textBox8.Text);
            List<string> d = new List<string>();
            Random rand = new Random();
            string mou, day, year, fulldate;

            for (int i = 0; i < c; i++)
            {
                mou = "" + (rand.Next(11) + 1);
                if (Convert.ToInt32(mou) < 10)
                {
                    mou = "0" + mou;
                }

                day = "" + (rand.Next(28) + 1);
                if (Convert.ToInt32(day) < 10)
                {
                    day = "0" + day;
                }

                year = "" + (rand.Next(b-a) + a);

                fulldate = day + "." + mou + "." + year;

                d.Add(fulldate);
            }
            richTextBox4.Lines = d.ToArray();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            GenerateNumber();
        }

        void GenerateNumber()
        {
            int a = Convert.ToInt32(textBox9.Text);
            int b = Convert.ToInt32(textBox10.Text);
            int c = Convert.ToInt32(textBox11.Text);
            List<string> d = new List<string>();
            Random rand = new Random();

            for (int i = 0; i < c; i++)
            {
                d.Add(Convert.ToString(rand.Next(b - a) + a));
            }
            richTextBox5.Lines = d.ToArray();
        }
    }
}