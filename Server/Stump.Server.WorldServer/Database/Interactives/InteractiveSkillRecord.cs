﻿using Stump.Core.Reflection;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives
{
    [TableName("interactives_skills")]
    public class InteractiveSkillRecord : ParameterizableRecord, IAutoGeneratedRecord
    {
        public const int DEFAULT_TEMPLATE = 184;
        private ConditionExpression m_conditionExpression;
        private int? m_customTemplateId;
        private InteractiveSkillTemplate m_template;

        [PrimaryKey("Id")]
        public int Id { get; set; }

        public string Type { get; set; }

        public int Duration { get; set; }

        public int? CustomTemplateId
        {
            get { return m_customTemplateId; }
            set
            {
                m_customTemplateId = value;
                m_template = null;
            }
        }

        protected virtual int GenericTemplateId => DEFAULT_TEMPLATE;

        [Ignore]
        public virtual InteractiveSkillTemplate Template
        {
            get
            {
                InteractiveSkillTemplate arg_43_0;
                if ((arg_43_0 = m_template) == null)
                    arg_43_0 = (m_template = Singleton<InteractiveManager>.Instance.GetSkillTemplate(CustomTemplateId.HasValue ? (short)CustomTemplateId.Value : (short)GenericTemplateId));
                return arg_43_0;
            }
        }

        [NullString]
        public string Condition { get; set; }

        [Ignore]
        public ConditionExpression ConditionExpression
        {
            get
            {
                ConditionExpression result;
                if (string.IsNullOrEmpty(Condition) || Condition == "null")
                {
                    result = null;
                }
                else
                {
                    ConditionExpression arg_47_0;
                    if ((arg_47_0 = m_conditionExpression) == null)
                    {
                        arg_47_0 = (m_conditionExpression = ConditionExpression.Parse(Condition));
                    }
                    result = arg_47_0;
                }
                return result;
            }
            set
            {
                m_conditionExpression = value;
                Condition = value.ToString();
            }
        }

        public bool IsConditionFilled(Character character)
        {
            return ConditionExpression == null || ConditionExpression.Eval(character);
        }

        public virtual Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return Singleton<DiscriminatorManager<Skill>>.Instance.Generate(Type, id, this, interactiveObject);
        }
    }
}