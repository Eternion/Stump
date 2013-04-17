#region License GNU GPL
// PatchTask.cs
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
using System.Xml.Serialization;

namespace Uplauncher.Patcher
{
    [XmlInclude(typeof(AddFileTask))]
    [XmlInclude(typeof(RemoveFileTask))]
    [XmlType("Task")]
    public abstract class PatchTask
    {
        public abstract bool CanApply();
        public abstract void Apply(UplauncherModelView uplauncher);

        public event Action<PatchTask> Applied;

        protected void OnApplied()
        {
            Action<PatchTask> handler = Applied;
            if (handler != null) handler(this);
        }
    }
}