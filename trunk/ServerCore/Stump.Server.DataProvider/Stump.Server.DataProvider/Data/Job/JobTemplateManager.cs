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
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Server.DataProvider.Data.Job
{
    public class JobTemplateManager : DataManager<int,JobTemplate>
    {
        protected override JobTemplate GetData(int id)
        {
            var job = D2OLoader.LoadData<DofusProtocol.D2oClasses.Job>(id);
            var skills = D2OLoader.LoadData<Skill>();

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
                return template;
            }
        
        protected override Dictionary<int, JobTemplate> GetAllData()
        {
            var jobs = D2OLoader.LoadData<DofusProtocol.D2oClasses.Job>();
            var skills = D2OLoader.LoadData<Skill>();

            var jobsTemplates = new Dictionary<int, JobTemplate>(jobs.Count());
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
                jobsTemplates.Add(job.id, template);
            }
            return jobsTemplates;
        }
    }
}