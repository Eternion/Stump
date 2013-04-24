using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleExemple
{
    [Serializable]
    public class InteractiveObject
    {
        /// <summary>
        /// Map oû est présent l'élement
        /// </summary>
        public int MapId;
        /// <summary>
        /// Numéro de l'élement
        /// </summary>
        public int ElementId;
        /// <summary>
        /// Compétence utilisé
        /// </summary>
        public int SkillIId;
        /// <summary>
        /// Uid de la compétence
        /// </summary>
        public int SkillInstanceUid;
        /// <summary>
        /// Durée d'utilisation de la compétence
        /// </summary>
        public int Duration;
        /// <summary>
        /// Map de destination
        /// </summary>
        public int DestMapId;
        /// <summary>
        /// Cellule de Destination
        /// </summary>
        public int DestCellId;

        public InteractiveObject()
        {
            
        }
    }
}
