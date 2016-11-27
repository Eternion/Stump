#region License GNU GPL

// SubActorLook.cs
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

using System.Text;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Actors.Look
{
    public class SubActorLook
    {
        private SubEntityBindingPointCategoryEnum m_bindingCategory;
        private sbyte m_bindingIndex;
        private ActorLook m_look;

        public SubActorLook(sbyte index, SubEntityBindingPointCategoryEnum category, ActorLook look)
        {
            m_bindingIndex = index;
            m_bindingCategory = category;
            Look = look;
            m_subEntity = new ObjectValidator<SubEntity>(BuildSubEntity);
        }

        public sbyte BindingIndex
        {
            get { return m_bindingIndex; }
            set
            {
                m_bindingIndex = value;
                m_subEntity.Invalidate();
            }
        }

        public SubEntityBindingPointCategoryEnum BindingCategory
        {
            get { return m_bindingCategory; }
            set
            {
                m_bindingCategory = value;
                m_subEntity.Invalidate();
            }
        }

        public ActorLook Look
        {
            get { return m_look; }
            private set
            {
                if (m_look != null)
                    m_look.EntityLookValidator.ObjectInvalidated -= OnLookInvalidated;

                m_look = value;
                m_look.EntityLookValidator.ObjectInvalidated += OnLookInvalidated;
            }
        }

        private void OnLookInvalidated(ObjectValidator<EntityLook> obj)
        {
            m_subEntity.Invalidate();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append((sbyte)BindingCategory);
            result.Append("@");
            result.Append(BindingIndex);
            result.Append("=");
            result.Append(Look);

            return result.ToString();
        }

        #region SubEntity

        private readonly ObjectValidator<SubEntity> m_subEntity;

        public ObjectValidator<SubEntity> SubEntityValidator
        {
            get { return m_subEntity; }
        }

        private SubEntity BuildSubEntity()
        {
            return new SubEntity((sbyte) BindingCategory, BindingIndex, Look.GetEntityLook());
        }

        public SubEntity GetSubEntity()
        {
            return m_subEntity;
        }

        #endregion
    }
}