
#region NameSpaces

using MSNPSharp.Core;
using MSNPSharp.DataTransfer;
using MSNPSharp;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Timers;
using System.Drawing;

#endregion

namespace Stump.Modules.Messenger
{
    /*public class MessengerClient : IDisposable
    {

        #region Propriétés

        /// <summary>
        /// Notre Client Messenger
        /// </summary>
        private Messenger client = new Messenger();

        /// <summary>
        /// Notre Timer pour garder en activité le Client
        /// </summary>
        private Timer pingTimer = new Timer(50000);

        /// <summary>
        /// Liste d'adresse autorisé à communiquer
        /// </summary>
        private List<string> authorizedList = new List<string>();

        /// <summary>
        /// Indique si le Client ne peut parler qu'aux personnes autorisé
        /// </summary>
        private Boolean restricted = false;

        /// <summary>
        /// Indique si on est connecté ou non
        /// </summary>
        private Boolean connected = false;

        public Boolean isConnected
        { get { return connected; } }

        private List<Conversation> conversationList = new List<Conversation>();


        #endregion

        #region Constructeur Destructeur

        /// <summary>
        /// Initialise une nouvelle instance de la Class
        /// </summary>
        /// <param name="login">Adresse email</param>
        /// <param name="pass">Mot de Passe</param>
        public MessengerClient(string login, string pass)
        {
            this.client.Credentials = new Credentials(login, pass, MsnProtocol.MSNP18);
            AddEvents();
        }

        /// <summary>
        /// Libère les ressources utilisé par la Class
        /// </summary>
        public void Dispose()
        {
            this.pingTimer.Elapsed -= _pingTimer_Elapsed;
            this.client.Nameserver.PingAnswer -= Nameserver_PingAnswer;
            this.pingTimer.Dispose();
            this.pingTimer = null;
            this.client = null;
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Lance le Client Messenger
        /// </summary>
        public void Connect()
        {
            if (!this.connected)
            {
                client.Connect();
            }
            else
            {
                this.connected = false;
                client.Disconnect();
                client.Connect();
            }
            pingTimer.Start();
        }

        /// <summary>
        /// Lance le Client Messenger
        /// </summary>
        /// <param name="restricted">Indique si seul certains personnes sont autorisé à communiquer avec le client</param>
        public void Connect(Boolean restricted)
        {
            this.restricted = restricted;
            if (!connected) client.Connect();
            else
            {
                connected = false;
                client.Disconnect();
                client.Connect();
            }
            pingTimer.Start();
        }

        /// <summary>
        /// Ajoute les Events nécessaires
        /// </summary>
        private void AddEvents()
        {
            //PING
            pingTimer.Elapsed += new ElapsedEventHandler(_pingTimer_Elapsed);
            client.Nameserver.PingAnswer += new EventHandler<PingAnswerEventArgs>(Nameserver_PingAnswer);
            client.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            client.Nameserver.SignedOff += new EventHandler<SignedOffEventArgs>(Nameserver_SignedOff);
        }

        #endregion

        #region Ping Timer

        /// <summary>
        /// Quand le timer d'activité est écoulé, on envoit une réponse au serveur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            client.Nameserver.SendPing();
        }

        /// <summary>
        /// Le serveur répond a notre ping, en spécifiant le prochain intervalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nameserver_PingAnswer(object sender, PingAnswerEventArgs e)
        {
            this.pingTimer.Interval = e.SecondsToWait * 1000;
            this.pingTimer.Start();
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Autorise toute personne à communiquer avec
        /// </summary>
        public void AuthorizedAll()
        {
            restricted = false;
        }

        /// <summary>
        /// Efface la liste des Personnes authorisé
        /// </summary>
        public void ClearAuthorizedList()
        {
            authorizedList.Clear();
        }

        /// <summary>
        /// Ajoute une adresse authorisé
        /// </summary>
        /// <param name="Adress"></param>
        public void AddAuthorizedAdress(string Adress)
        {
            if (!Adress.Contains("@"))
            {
                throw new Exception("Format de L'adresse invalide !");
            }

            authorizedList.Add(Adress);
            this.restricted = true;
        }

        /// <summary>
        /// Charge une list xml d'adresse authorisé
        /// </summary>
        /// <param name="Adress">Adresse du fichier Xml</param>
        public void AddAuthorizedAdressList(string Uri)
        {
            if (!System.IO.File.Exists(Uri))
            {
                Console.WriteLine("Le fichier est introuvable à l'emplacement {0}", Uri);
                return;
            }

            XDocument doc = XDocument.Load(new System.IO.StreamReader(Uri), LoadOptions.None);
            XElement list = doc.Root;

            foreach (XElement email in list.Elements("email"))
            {
                if (!email.Value.Contains("@"))
                    Console.WriteLine("Format de L'adresse invalide ! ( {0} )", email.Value);
                else
                    authorizedList.Add(email.Value);
            }
            this.restricted = true;
        }

        #endregion

        #region Connection

        public delegate void Connected();

        public event Connected OnConnected;

        public delegate void Disconnected();

        public event Disconnected OnDisconnected;

        /// <summary>
        /// Quand on est déconnecté
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nameserver_SignedOff(object sender, SignedOffEventArgs e)
        {
            this.connected = false;
            client.ConversationCreated -= _client_ConversationCreated;
            conversationList.Clear();
            if (this.OnDisconnected != null) this.OnDisconnected();
        }

        /// <summary>
        /// Quand on est connecté
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nameserver_SignedIn(object sender, EventArgs e)
        {
            this.connected = true;
            this.client.Owner.Status = PresenceStatus.Online;
            this.client.ConversationCreated += new EventHandler<ConversationCreatedEventArgs>(_client_ConversationCreated);
            if (this.OnConnected != null) this.OnConnected();
        }

        #endregion

        #region Conversation

        public delegate void NewMessage(String Message, Conversation sender);

        public event NewMessage OnNewMessage;

        /// <summary>
        /// Quand quelqu'un nous parle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _client_ConversationCreated(object sender, ConversationCreatedEventArgs e)
        {
            e.Conversation.TextMessageReceived += new EventHandler<TextMessageEventArgs>(Conversation_TextMessageReceived);
            e.Conversation.ContactJoined += new EventHandler<ContactEventArgs>(Conversation_ContactJoined);
            e.Conversation.ContactLeft += new EventHandler<ContactEventArgs>(Conversation_ContactLeft);
            e.Conversation.ConversationEnded += new EventHandler<ConversationEndEventArgs>(Conversation_ConversationEnded);
            conversationList.Add(e.Conversation);
        }

        /// <summary>
        /// Un contact quitte la conversation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conversation_ContactLeft(object sender, ContactEventArgs e)
        {
            (sender as Conversation).SendTextMessage(new TextMessage("Tchaow " + e.Contact.Name));
        }

        /// <summary>
        /// Un contact rejoins la conversation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conversation_ContactJoined(object sender, ContactEventArgs e)
        {
            Conversation conv = sender as Conversation;
            if (conv.Contacts.Count > 1)
            {
                conv.SendTextMessage(new TextMessage("Coucou " + e.Contact.Name));
            }
        }

        /// <summary>
        /// Fin de la conversation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conversation_ConversationEnded(object sender, ConversationEndEventArgs e)
        {
            //On enlève les events
            e.Conversation.TextMessageReceived -= Conversation_TextMessageReceived;
            e.Conversation.ContactJoined -= Conversation_ContactJoined;
            e.Conversation.ContactLeft -= Conversation_ContactLeft;
            e.Conversation.ConversationEnded -= Conversation_ConversationEnded;
            conversationList.Remove(e.Conversation);
        }

        /// <summary>
        /// Process les Messages recus si ils sont authorisé à l'etre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conversation_TextMessageReceived(object sender, TextMessageEventArgs e)
        {
            Conversation conv = sender as Conversation;
            if (this.restricted) //Resitriction activé
            {
                if (this.authorizedList.Contains(e.Sender.Mail)) //Il est authorisé
                {
                    if (this.OnNewMessage != null)
                    {
                        this.OnNewMessage(e.Message.Text, conv);
                    }
                }
                else //Il est pas authorisé
                {
                    this.client.ContactService.BlockContact(client.ContactList.GetContactByCID((long)e.Sender.CID));
                    conv.End();
                }
            }
            else //Pas de restriction
            {
                if (this.OnNewMessage != null)
                {
                    this.OnNewMessage(e.Message.Text, conv);
                }
            }
        }


        /// <summary>
        /// Envoye un message à toutes les adresses dans la liste des authorisé
        /// </summary>
        /// <param name="message">Message</param>
        public void Send(string message)
        {
            foreach (string Email in authorizedList)
            {
                foreach (Conversation conv in this.conversationList)
                {
                    if (conv.HasContact(client.ContactList.GetContact(Email)))
                    {
                        conv.SendTextMessage(new TextMessage(message));
                        return;
                    }
                }
                Conversation Conversation = client.CreateConversation();
                Conversation.Invite(Email, ClientType.PassportMember);
                Conversation.Invite(Email, ClientType.PassportMember);
                Conversation.SendTextMessage(new TextMessage(message));
                Conversation.End();
            }
        }

        /// <summary>
        /// Envoie un memssage à un Contact particulier
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="email">Contact</param>
        public void Send(string message, string email)
        {
            foreach (Conversation conv in this.conversationList)
            {
                if (conv.HasContact(client.ContactList.GetContact(email)))
                {
                    conv.SendTextMessage(new TextMessage(message));
                    return;
                }
            }
            Conversation Conversation = client.CreateConversation();
            Conversation.Invite(email, ClientType.PassportMember);
            Conversation.Invite(email, ClientType.PassportMember);
            Conversation.SendTextMessage(new TextMessage(message));
            Conversation.End();
        }

        /// <summary>
        /// Message formaté
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public TextMessage RichText(string message, Color color, TextDecorations dec)
        {
            TextMessage current = new TextMessage(message);
            current.Decorations = dec;
            current.Color = color;
            return current;
        }

        #endregion

        #region Profile

        /// <summary>
        /// Change le status du Client Messenger
        /// </summary>
        /// <param name="status"></param>
        public void ChangeStatus(PresenceStatus status)
        {
            client.Owner.Status = status;
        }

        /// <summary>
        /// Change le message perso affiché
        /// </summary>
        /// <param name="message"></param>
        public void ChangeMessage(String message)
        {
            client.Owner.PersonalMessage = new PersonalMessage(message, MediaType.None, null, NSMessageHandler.MachineGuid);
        }

        /// <summary>
        /// Change le nom affiché
        /// </summary>
        /// <param name="name"></param>
        public void ChangeName(String name)
        {
            client.Owner.Name = name;
        }

        /// <summary>
        /// Change l'image du compte
        /// </summary>
        /// <param name="image"></param>
        public void ChangeImage(String uri)
        {
            if (uri.Contains("http:") || uri.Contains("www."))
            {
                try
                {
                    System.Net.WebRequest wr = System.Net.FileWebRequest.Create(uri);
                    wr.BeginGetResponse(ChangeImageCallBack, (object)wr);
                }
                catch (UriFormatException)
                {
                }
            }
            else
            {
                if (System.IO.File.Exists(uri))
                    this.client.StorageService.UpdateProfile(Image.FromFile(uri), uri.GetHashCode().ToString());
            }
        }

        private void ChangeImageCallBack(IAsyncResult ar)
        {
            Image img = Image.FromStream((ar.AsyncState as System.Net.WebRequest).EndGetResponse(ar).GetResponseStream());
            this.client.StorageService.UpdateProfile(img, img.GetHashCode().ToString());
        }

        #endregion

    }*/
}
