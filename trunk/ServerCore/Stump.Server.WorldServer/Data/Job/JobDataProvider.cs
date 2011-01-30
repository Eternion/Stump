// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.Job
{
    public static class JobDataProvider
    {
        private static Dictionary<int, JobTemplate> m_jobsTemplates;


        [StageStep(Stages.One, "Loaded Jobs Template")]
        public static void LoadJobsTemplates()
        {
            var jobs = DataLoader.LoadData<DofusProtocol.D2oClasses.Job>();
            var skills = DataLoader.LoadData<Skill>();

            m_jobsTemplates = new Dictionary<int, JobTemplate>(jobs.Count());
            foreach (var job in jobs)
            {
                var template = new JobTemplate
                                   {
                                       JobId = job.id,
                                       JobToolIds = job.toolIds,
                                       SpecialisationOfId = job.specializationOfId,
                                       Skills = new List<JobSkill>()
                                   };

                template.Skills = skills.Where(s => s.parentJobId == job.id).Select(s =>
                    new JobSkill
                      {
                          SkillId = s.id,
                          Job = template,
                          InteractiveId = s.interactiveId,
                          ModifiableItemType = (ItemTypeEnum)s.modifiableItemType,
                          GatheredRessource = s.gatheredRessourceItem,
                          IsRepair = s.isRepair,
                          IsForgemagus = s.isForgemagus,
                          AvailableInHouse = s.availableInHouse,
                          CraftableItemIds = s.craftableItemIds
                      }).ToList();
                m_jobsTemplates.Add(job.id, template);
            }
        }


        public static JobTemplate GetJobTemplate(int jobId)
        {
            if (m_jobsTemplates.ContainsKey(jobId))
                return m_jobsTemplates[jobId];
            return null;
        }
    }
}