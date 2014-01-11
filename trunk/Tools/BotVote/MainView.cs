using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using DeathByCaptcha;

namespace BotVote
{
    public partial class MainView : Form
    {
        readonly Queue proxyStack = new Queue();
        private int nbVotes;

        public MainView()
        {
            InitializeComponent();

            var threadProxy = new Thread(LoadProxyList);
            threadProxy.Start();

            var threadBrowser = new Thread(BrowserLoop);
            threadBrowser.Start();
        }

        private void BrowserLoop()
        {
            while (true)
            {
                if (proxyStack.Count > 0)
                {
                    runBrowserThread(new Uri("http://www.rpg-paradize.com/?page=vote&vote=42689"));
                }
            }
        }

        private void LoadProxyList()
        {
            var proxyList = new List<string>();

            using (var r = new StreamReader("ProxyList.txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Contains("http"))
                        proxyList.Add(line);
                    else
                        proxyList.Add("http://" + line);
                }
            }

            Action action = () => progressBar1.Maximum = proxyList.Count;
            Invoke(action);

            foreach (var threadProxy in proxyList.Select(proxy => new Thread(() => ProxyCheck(proxy))))
            {
                threadProxy.Start();
            }
        }

        private void ProxyCheck(string proxy)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                request.Proxy = new WebProxy(proxy);
                request.Timeout = 10000;
                request.GetResponse();

                proxyStack.Enqueue(proxy);

                WriteBotMSG(0, string.Format("Proxy({0}) added !", proxy));

                Action action = () => { progressBar1.Value++; proxyCount.Text = string.Format("{0} / {1}", progressBar1.Value, progressBar1.Maximum); };
                Invoke(action);
            }
            catch (WebException ex)
            {
                WriteBotMSG(1, string.Format("Proxy({0}) error({1}) !", proxy, ex.Message));
            }
        }

        private void runBrowserThread(Uri url)
        {
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += Browser_DocumentCompleted;
                br.Navigate(url);
                //Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void Browser_DocumentCompleted(Object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (proxyStack.Count <= 0)
            {
                WriteBotMSG(1, "End of Proxy List");
                Application.ExitThread();
                return;
            }

            var Browser = sender as WebBrowser;

            if (Browser.Url != e.Url)
            {
                Application.ExitThread();
                return;
            }

            var doc = Browser.Document;
            var recaptchaImage = doc.GetElementById("recaptcha_image");
            var recaptchaChallenge = doc.GetElementById("recaptcha_challenge_field");

            if (recaptchaImage == null || recaptchaChallenge == null)
                return;

            var captchaChallenge = recaptchaChallenge.GetAttribute("value");
            var captchaLink = recaptchaImage.Children[0].GetAttribute("src");

            WriteBotMSG(0, "Find new Captcha");

            var client = (Client)new SocketClient("", "");

            var webClient = new WebClient();
            var imageBytes = webClient.DownloadData(captchaLink);

            var captcha = client.Decode(imageBytes, Client.DefaultTimeout);

            if (captcha == null)
                return;

            var captchaText = captcha.Text;
            WriteBotMSG(0, string.Format("Captcha Decoded({0}) - Your balance is {1:F2} US cents", captchaText, client.Balance));

            //http://www.rpg-paradize.com/?page=vote2
            //submitvote = 42689
            //recaptcha_challenge_field
            //recaptcha_response_field
            //submit = Voter

            var proxyURL = proxyStack.Dequeue().ToString();
            WriteBotMSG(0, string.Format("Use Proxy: {0}", proxyURL));

            var response = HttpPost("http://www.rpg-paradize.com/?page=vote2", proxyURL, "submitvote=42689&recaptcha_challenge_field=" + captchaChallenge + "&recaptcha_response_field=" + captchaText + "&submit=Voter");
            if (!response.Contains("<title>~Autre serveur privé</title>"))
            {
                WriteBotMSG(1, string.Format("Vote error({0}) -> Report Captcha", response));
                client.Report(captcha);
                Application.ExitThread();
                return;
            }

            WriteBotMSG(0, "Vote Success");
            nbVotes += 1;

            voteCount.Text = "Votes: " + nbVotes;

            Application.ExitThread();
        }

        private static string HttpPost(string URI, string proxyString, string Parameters)
        {
            try
            {
                var req = WebRequest.Create(URI);
                req.Proxy = new WebProxy(proxyString, true);

                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";

                var bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
                req.ContentLength = bytes.Length;
                var os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length); //Push it out there
                os.Close();

                var resp = req.GetResponse();
                var sr = new StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (SystemException ex)
            {
                return ex.Message;
            }
        }

        private void WriteBotMSG(int type, string message)
        {
            if (type == 1)
            {
                Action action = () => errorConsole.AppendText(string.Format("[{0}] - {1}\n", DateTime.Now.ToShortTimeString(), message));
                Invoke(action);
            }
            else
            {
                Action action = () => infoConsole.AppendText(string.Format("[{0}] - {1}\n", DateTime.Now.ToShortTimeString(), message));
                Invoke(action);
            }
        }
    }
}
