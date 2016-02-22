#region License GNU GPL
// RequestBox.cs
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

using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Dialogs
{
    public abstract class RequestBox
    {
        protected RequestBox(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        public virtual bool IsExchangeRequest => false;

        public Character Source
        {
            get;
            protected set;
        }

        public Character Target
        {
            get;
            protected set;
        }

        public void Open()
        {
            Source.OpenRequestBox(this);
            Target.OpenRequestBox(this);

            OnOpen();
        }

        protected virtual void OnOpen()
        {

        }

        public void Accept()
        {
            OnAccept();
            Close();
        }

        protected virtual void OnAccept()
        {

        }

        public void Deny()
        {
            OnDeny();

            Close();
        }

        protected virtual void OnDeny()
        {

        }

        public void Cancel()
        {
            OnCancel();
            Close();
        }

        protected virtual void OnCancel()
        {

        }

        protected void Close()
        {
            Source.ResetRequestBox();
            Target.ResetRequestBox();
        }
    }
}