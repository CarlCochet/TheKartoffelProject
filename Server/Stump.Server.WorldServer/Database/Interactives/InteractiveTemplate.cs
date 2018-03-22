using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Interactives
{
    public class InteractiveTemplateRelator
    {
        public static string FetchQuery = "SELECT * FROM interactives_templates " +
                                          "LEFT JOIN interactives_skills_templates ON interactives_skills_templates.InteractiveId=interactives_templates.Id " +
                                          "LEFT JOIN interactives_templates_skills ON interactives_templates_skills.InteractiveTemplateId=interactives_templates.Id " +
                                          "LEFT JOIN interactives_skills ON interactives_skills.Id=interactives_templates_skills.SkillId ";


        private Dictionary<int, InteractiveTemplate> m_currentMapping = new Dictionary<int, InteractiveTemplate>();
        private InteractiveTemplate m_current;

        public InteractiveTemplate Map(InteractiveTemplate template, InteractiveSkillTemplate skillTemplate, InteractiveTemplateSkills binding,
                                       InteractiveCustomSkillRecord skill)
        {
            if (template == null)
                return m_current;

            InteractiveTemplate current;

            if (m_currentMapping.TryGetValue(template.Id, out current))
            {
                if (binding != null && binding.InteractiveTemplateId == m_current.Id && binding.SkillId == skill.Id &&
                    current.CustomSkills.All(x => x.Id != skill.Id))
                    current.CustomSkills.Add(skill);

                if (skillTemplate != null && m_current.TemplateSkills.All(x => x.Id != skillTemplate.Id))
                    current.TemplateSkills.Add(skillTemplate);

                return null;
            }

            var previous = m_current;

            m_current = template;
            m_currentMapping.Add(template.Id, template);
            if (binding != null && binding.InteractiveTemplateId == m_current.Id && binding.SkillId == skill.Id &&
                m_current.CustomSkills.All(x => x.Id != skill.Id))
                m_current.CustomSkills.Add(skill);

            if (skillTemplate != null && m_current.TemplateSkills.All(x => x.Id != skillTemplate.Id))
                m_current.TemplateSkills.Add(skillTemplate);

            return previous;
        }
    }

    [TableName("interactives_templates_skills")]
    public class InteractiveTemplateSkills : IAutoGeneratedRecord
    {
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        [Column("InteractiveTemplateId"), Index]
        public int InteractiveTemplateId
        {
            get;
            set;
        }

        [Column("SkillId")]
        public int SkillId
        {
            get;
            set;
        }
    }


    [TableName("interactives_templates")]
    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    public class InteractiveTemplate : IAssignedByD2O, IAutoGeneratedRecord
    {
        private string m_name;

        public InteractiveTemplate()
        {
        }

        [PrimaryKey("Id", false)]
        public int Id
        {
            get;
            set;
        }

        public InteractiveTypeEnum Type
        {
            get { return (InteractiveTypeEnum)Id; }
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public int ActionId
        {
            get;
            set;
        }

        public bool DisplayTooltip
        {
            get;
            set;
        }

        [Ignore]
        public List<InteractiveSkillTemplate> TemplateSkills
        {
            get;
            set;
        } = new List<InteractiveSkillTemplate>();

        [Ignore]
        public List<InteractiveCustomSkillRecord> CustomSkills
        {
            get;
            set;
        } = new List<InteractiveCustomSkillRecord>();

        public IEnumerable<ISkillRecord> GetSkills()
        {
            return TemplateSkills.Cast<ISkillRecord>().Concat(CustomSkills).ToList();
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var template = (Interactive)d2oObject;
            Id = template.id;
            NameId = template.nameId;
            ActionId = template.actionId;
            DisplayTooltip = template.displayTooltip;
        }

        #endregion

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }
    }
}