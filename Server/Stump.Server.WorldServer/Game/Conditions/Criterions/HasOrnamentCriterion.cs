using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class HasOrnamentCriterion : Criterion
    {
        public const string Identifier = "HO";

        public short Ornament
        {
            get;
            set;
        }

        public override bool Eval(Character character)
            => Operator == ComparaisonOperatorEnum.EQUALS ? character.HasOrnament((ushort)Ornament) : !character.HasOrnament((ushort)Ornament);

        public override void Build()
        {
            short ornament;

            if (!short.TryParse(Literal, out ornament))
                throw new Exception(string.Format("Cannot build HasOrnamentCriterion, {0} is not a valid ornament", Literal));

            Ornament = ornament;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
