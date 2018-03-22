using Stump.Core.Reflection;
using Stump.Server.WorldServer.Database.Alliances;
using Stump.Server.WorldServer.Database.Symbols;
using System.Drawing;

namespace Stump.Server.WorldServer.Game.Alliances
{
    public class AllianceEmblem
    {
        // FIELDS
        private Color? m_backgroundColor;
        private Color? m_foregroundColor;

        // PROPERTIES
        public AllianceRecord Record
        {
            get;
            private set;
        }
        public EmblemRecord Template
        {
            get;
            private set;
        }
        public short BackgroundShape
        {
            get
            {
                return this.Record.EmblemBackgroundShape;
            }
            set
            {
                this.Record.EmblemBackgroundShape = value;
                this.IsDirty = true;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                Color? backgroundColor = this.m_backgroundColor;
                Color arg_3F_0;
                if (!backgroundColor.HasValue)
                {
                    Color? color = this.m_backgroundColor = new Color?(Color.FromArgb(this.Record.EmblemBackgroundColor));
                    arg_3F_0 = color.Value;
                }
                else
                {
                    arg_3F_0 = backgroundColor.GetValueOrDefault();
                }
                return arg_3F_0;
            }
            set
            {
                this.m_backgroundColor = new Color?(value);
                this.Record.EmblemBackgroundColor = value.ToArgb();
                this.IsDirty = true;
            }
        }
        public short SymbolShape
        {
            get
            {
                return this.Record.EmblemForegroundShape;
            }
            set
            {
                this.Record.EmblemForegroundShape = value;
                this.Template = Singleton<AllianceManager>.Instance.TryGetEmblem((int)this.SymbolShape);
                this.IsDirty = true;
            }
        }
        public Color SymbolColor
        {
            get
            {
                Color? foregroundColor = this.m_foregroundColor;
                Color arg_3F_0;
                if (!foregroundColor.HasValue)
                {
                    Color? color = this.m_foregroundColor = new Color?(Color.FromArgb(this.Record.EmblemForegroundColor));
                    arg_3F_0 = color.Value;
                }
                else
                {
                    arg_3F_0 = foregroundColor.GetValueOrDefault();
                }
                return arg_3F_0;
            }
            set
            {
                this.m_foregroundColor = new Color?(value);
                this.Record.EmblemForegroundColor = value.ToArgb();
                this.IsDirty = true;
            }
        }
        public bool IsDirty
        {
            get;
            set;
        }

        // CONSTRUCTORS
        public AllianceEmblem(AllianceRecord record)
        {
            this.Record = record;
            if (this.SymbolShape != 0)
            {
                this.Template = Singleton<AllianceManager>.Instance.TryGetEmblem((int)this.SymbolShape);
            }
        }

        // METHODS
        public void ChangeEmblem(Stump.DofusProtocol.Types.GuildEmblem emblem)
        {
            this.BackgroundColor = Color.FromArgb(emblem.backgroundColor);
            this.BackgroundShape = emblem.backgroundShape;
            this.SymbolColor = Color.FromArgb(emblem.symbolColor);
            this.SymbolShape = (short)emblem.symbolShape;
        }
        public bool DoesEmblemMatch(Stump.DofusProtocol.Types.GuildEmblem emblem)
        {
            return this.BackgroundColor.ToArgb() == emblem.backgroundColor && this.BackgroundShape == emblem.backgroundShape && this.SymbolColor.ToArgb() == emblem.symbolColor && this.SymbolShape == emblem.symbolShape;
        }
        public bool DoesEmblemMatch(AllianceEmblem emblem)
        {
            return this.BackgroundColor == emblem.BackgroundColor && this.BackgroundShape == emblem.BackgroundShape && this.SymbolColor == emblem.SymbolColor && this.SymbolShape == emblem.SymbolShape;
        }
        public DofusProtocol.Types.GuildEmblem GetNetworkGuildEmblem()
        {
            return new DofusProtocol.Types.GuildEmblem
            {
                backgroundColor = Record.EmblemBackgroundColor,
                backgroundShape = (sbyte)Record.EmblemBackgroundShape,
                symbolColor = Record.EmblemForegroundColor,
                symbolShape = (ushort)Record.EmblemForegroundShape
            };
        }
    }
}
