using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class AuthClientAdapter : DuplexClientBase<IRemoteAuthOperations>, IRemoteAuthOperations
    {
        public AuthClientAdapter(object callbackInstance) : base(callbackInstance)
        {
        }

        public AuthClientAdapter(object callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
        {
        }

        public AuthClientAdapter(object callbackInstance, string endpointConfigurationName, string remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(object callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(object callbackInstance, Binding binding, EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress)
        {
        }

        public AuthClientAdapter(object callbackInstance, ServiceEndpoint endpoint) : base(callbackInstance, endpoint)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance) : base(callbackInstance)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress)
        {
        }

        public AuthClientAdapter(InstanceContext callbackInstance, ServiceEndpoint endpoint) : base(callbackInstance, endpoint)
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

        public RegisterResultEnum RegisterWorld(WorldServerData serverData)
        {
            try
            {
                return Channel.RegisterWorld(serverData);
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