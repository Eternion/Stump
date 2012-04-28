using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class AuthClientAdapter : ClientBase<IRemoteAuthOperations>, IRemoteAuthOperations
    {
        public AuthClientAdapter()
        {
        }

        public AuthClientAdapter(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public AuthClientAdapter(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public event Action<Exception> Error;

        private void OnError(Exception ex)
        {
            Action<Exception> handler = Error;
            if (handler != null)
                handler(ex);
        }
        
        #region IRemoteAuthOperations Members

        public RegisterResultEnum RegisterWorld(WorldServerData serverData, string remoteIpcAddress)
        {
            try
            {
                return Channel.RegisterWorld(serverData, remoteIpcAddress);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return RegisterResultEnum.AuthServerUnreachable;
            }
        }

        public void UnRegisterWorld()
        {
            try
            {
                Channel.UnRegisterWorld();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public void ChangeState(ServerStatusEnum state)
        {
            try
            {
                Channel.ChangeState(state);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public void UpdateConnectedChars(int value)
        {
            try
            {
                Channel.UpdateConnectedChars(value);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public AccountData GetAccountByTicket(string ticket)
        {
            try
            {
                return Channel.GetAccountByTicket(ticket);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return null;
            }
        }

        public AccountData GetAccountByNickname(string nickname)
        {
            try
            {
                return Channel.GetAccountByNickname(nickname);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return null;
            }
        }

        public bool UpdateAccount(AccountData modifiedRecord)
        {
            try
            {
                return Channel.UpdateAccount(modifiedRecord);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool CreateAccount(AccountData accountData)
        {
            try
            {
                return Channel.CreateAccount(accountData);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool DeleteAccount(string accountname)
        {
            try
            {
                return Channel.DeleteAccount(accountname);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool AddAccountCharacter(uint accountId, uint characterId)
        {
            try
            {
                return Channel.AddAccountCharacter(accountId, characterId);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool DeleteAccountCharacter(uint accountId, uint characterId)
        {
            try
            {
                return Channel.DeleteAccountCharacter(accountId, characterId);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool BlamAccountFrom(uint victimAccountId, uint bannerAccountId, TimeSpan duration, string reason)
        {
            try
            {
                return Channel.BlamAccountFrom(victimAccountId, bannerAccountId, duration, reason);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public bool BlamAccount(uint victimAccountId, TimeSpan duration, string reason)
        {
            try
            {
                return Channel.BlamAccount(victimAccountId, duration, reason);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        public
            bool BanIp
            (string ipToBan, uint bannerAccountId, TimeSpan duration, string reason)
        {
            try
            {
                return Channel.BanIp(ipToBan, bannerAccountId, duration, reason);
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }

        #endregion
    }
}