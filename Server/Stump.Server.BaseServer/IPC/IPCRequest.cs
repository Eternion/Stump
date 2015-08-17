#region License GNU GPL
// IPCRequest.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using NLog;
using Stump.Core.Timers;

namespace Stump.Server.BaseServer.IPC
{
    public interface IIPCRequest
    {
        Guid Guid
        {
            get;
            set;
        }

        TimedTimerEntry TimeoutTimer
        {
            get;
            set;
        }

        IPCMessage RequestMessage
        {
            get;
            set;
        }

        bool TimedOut
        {
            get;
            set;
        }

        void Cancel(string from = "");
        bool ProcessMessage(IPCMessage message);
    }

    public class IPCRequest<T> : IIPCRequest where T : IPCMessage
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IPCRequest(IPCMessage requestMessage, Guid guid, RequestCallbackDelegate<T> callback, RequestCallbackErrorDelegate errorCallback,
            RequestCallbackDefaultDelegate defaultCallback, TimedTimerEntry timeoutTimer)
        {
            RequestMessage = requestMessage;
            Guid = guid;
            Callback = callback;
            ErrorCallback = errorCallback;
            DefaultCallback = defaultCallback;
            TimeoutTimer = timeoutTimer;
        }

        public IPCMessage RequestMessage
        {
            get;
            set;
        }

        public bool TimedOut
        {
            get;
            set;
        }

        public Guid Guid
        {
            get;
            set;
        }

        public TimedTimerEntry TimeoutTimer
        {
            get;
            set;
        }

        public RequestCallbackDelegate<T> Callback
        {
            get;
            set;
        }

        public RequestCallbackErrorDelegate ErrorCallback
        {
            get;
            set;
        }

        public RequestCallbackDefaultDelegate DefaultCallback
        {
            get;
            set;
        }

        public void Cancel(string from = "")
        {
            if (TimeoutTimer != null)
                TimeoutTimer.Dispose();

            ErrorCallback(new IPCErrorMessage("Cancelled" + (!string.IsNullOrEmpty(from) ? " from " + from : "")));
        }

        public bool ProcessMessage(IPCMessage message)
        {
            if (TimeoutTimer != null)
                TimeoutTimer.Dispose();

            if (message is T)
                Callback(message as T);
            else if (message is IIPCErrorMessage)
            {
                logger.Warn("IPC Error on message recv {0}", message);
                ErrorCallback(message as IIPCErrorMessage);
            }
            else
                DefaultCallback(message);

            return true;
        }
    }
}