﻿#region License GNU GPL
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
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.WorldServer.Core.IPC
{
    public interface IIPCRequest
    {
        Guid Guid
        {
            get;
            set;
        }

        TimerEntry TimeoutTimer
        {
            get;
            set;
        }

        IPCMessage RequestMessage
        {
            get;
            set;
        }

        bool ProcessMessage(IPCMessage message);
    }

    public class IPCRequest<T> : IIPCRequest where T : IPCMessage
    {
        public IPCRequest(IPCMessage requestMessage, Guid guid, IPCAccessor.RequestCallbackDelegate<T> callback, IPCAccessor.RequestCallbackErrorDelegate errorCallback,
            IPCAccessor.RequestCallbackDefaultDelegate defaultCallback, TimerEntry timeoutTimer)
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

        public Guid Guid
        {
            get;
            set;
        }

        public TimerEntry TimeoutTimer
        {
            get;
            set;
        }

        public IPCAccessor.RequestCallbackDelegate<T> Callback
        {
            get;
            set;
        }

        public IPCAccessor.RequestCallbackErrorDelegate ErrorCallback
        {
            get;
            set;
        }

        public IPCAccessor.RequestCallbackDefaultDelegate DefaultCallback
        {
            get;
            set;
        }

        public bool ProcessMessage(IPCMessage message)
        {
            TimeoutTimer.Stop();

            if (message is T)
                Callback(message as T);
            else if (message is IPCErrorMessage)
                ErrorCallback(message as IPCErrorMessage);
            else
                DefaultCallback(message);

            return true;
        }
    }
}