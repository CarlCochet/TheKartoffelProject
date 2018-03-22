//using Stump.DofusProtocol.Enums;
//using Stump.DofusProtocol.Messages;
//using Stump.Server.BaseServer.Database;
//using Stump.Server.BaseServer.Network;
//using Stump.Server.WorldServer.Commands.Commands.Patterns;
//using Stump.Server.WorldServer.Commands.Trigger;
//using Stump.Server.WorldServer.Database.Interactives;
//using Stump.Server.WorldServer.Database.World;
//using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
//using Stump.Server.WorldServer.Game.Dialogs;
//using Stump.Server.WorldServer.Game.Interactives;
//using Stump.Server.WorldServer.Game.Interactives.Skills;
//using Stump.Server.WorldServer.Game.Maps;
//using Stump.Server.WorldServer.Handlers.Dialogs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Stump.Server.WorldServer.Commands.Commands
//{
//    public class MazmosCommand : InGameCommand
//    {
//        public MazmosCommand()
//        {
//            Aliases = new[] { "mazmos" };
//            RequiredRole = RoleEnum.Player;
//            Description = "Abre un panel donde puedes seleccionar las mazmorras";
//        }

//        private List<MasmosDialog> m_dungs = new List<MasmosDialog>
//            {
//                new MasmosDialog(55050240, 523, "Mazmorra del Jalamut Real"),//LISTA
//                new MasmosDialog(56098816, 228, "Mazmorra del Morsaguino"), //LISTA
//                new MasmosDialog(57934593, 314, "Mazmorra del Kefrioh"),
//                new MasmosDialog(56360960, 343, "Mazmorra del Ben el Ripata"),
//                new MasmosDialog(57149697, 344, "Mazmorra Obsidiantre"),
//                new MasmosDialog(62915584, 344, "Mazmorra Cil"),
//                new MasmosDialog(62131720, 369, "Mazmorra Golosotron"),
//                new MasmosDialog(61998084, 369, "Mazmorra Tejjosus"),
//                new MasmosDialog(59511808, 316, "Mazmorra Gelifux"),
//                new MasmosDialog(112198145, 383, "Mazmorra Conde Contraras"),
//                new MasmosDialog(108927238, 373, "Mazmorra MizzFrizz"),
//                new MasmosDialog(110362624, 248, "Mazmorra Klime"),
//                new MasmosDialog(110100480, 355, "Mazmorra sylargh"),
//                new MasmosDialog(166990850, 233, "Mazmorra de Blop Multicolor"),
//                new MasmosDialog("dragocerdo", 72618499, 314, "Mazmorra Dragocerdo"),
//                new MasmosDialog("ancestral", 149684224, 453, "Mazmorra de ancestral"),
//                new MasmosDialog("kimbo", 21495808, 246, "Mazmorra Kimbo"),
//                new MasmosDialog("tranki", 107217920, 369, "Mazmorra del Trankitronko"),
//                new MasmosDialog("roble", 149423104, 313, "Roble blando"),
//                new MasmosDialog("jalato", 121373185, 429, "Mazmorra jalatos"),//LISTA
//                new MasmosDialog("esqueleto", 87034368, 40, "Mazmorra de los esqueltos"),
//                new MasmosDialog("croca", 27787264, 123, "Crocabulia "),
//                new MasmosDialog("herreros", 87295489, 356, "Mazmorra Herreros"),
//                new MasmosDialog("tynril", 63964672, 369, "Mazmorra de los Tynriles"),
//                new MasmosDialog("ballena", 140771328, 256, "Ballena"),
//                new MasmosDialog("reina", 137101312, 410, "Dimension reina de los ladrones"),//LISTA
//                new MasmosDialog("eskarlata", 136578048, 356, "Dimension del Eskarlata"),
//                new MasmosDialog("vortex", 144445702, 289, "Dimension vortex"),
//                new MasmosDialog("xelorium", 144707846, 262, "Dimension de xelorium"),
//                new MasmosDialog("fraktale", 143138823, 329, "Mazmorra de Fraktale"),
//                new MasmosDialog("chuko", 157024256, 205, "Mazmorra de Chuko"),
//                new MasmosDialog("canibola", 157548544, 280, "Mazmorra de Canibola"),
//                new MasmosDialog("sombra", 123207680, 451, "Sombra - Items Solos"),
//                new MasmosDialog("acuadomo", 119276033, 311, "Mazmorra de mercator"),
//                new MasmosDialog("nidas", 129500160, 314, "Dimension nidas"),//LISTA
//                new MasmosDialog("fosil", 130548736, 297, "Mazmorra del Fosil"),
//                new MasmosDialog("toxoliath", 136841216, 356, "Mazmorra del Toxoliath"),//LISTA
//                new MasmosDialog("moon", 157286400, 274, "Mazmorra Moon"),
//                new MasmosDialog("nawidad", 66585088, 355, "Mazmorra de Nawidad"),
//                new MasmosDialog("caverna", 66846720, 357, "Mazmorra de la Caverna de Nawidad"),
//                new MasmosDialog("papanowel", 66322432, 384, "PapaNowel - Dolmanax, Megaorbe"),
//                new MasmosDialog("miau", 160564224, 288, "Dimension Miauvizor"),
//                new MasmosDialog("dientinea", 169607168, 370, "Mazmorra de dientinea"),
//                new MasmosDialog("kutulu", 169345024, 397, "Mazmorra  kutulu"),
//                new MasmosDialog("nileza", 109576707, 273, "Mazmorra de Nileza"),
//                new MasmosDialog("zurcalia", 161743872, 314, "Mazmorra de Zurcalia"),
//                new MasmosDialog("ush", 162004992, 383, "Mazmorra de Ush"),
//                new MasmosDialog("grozilla", 76416012, 410, "Grozilla y Grazmerra"),
//                new MasmosDialog("chadalid", 152829952, 384, "Mazmorra chadalid"),//lista
//                new MasmosDialog("campos", 105381888, 315, "Mazmorra campos"),
//                new MasmosDialog("enaredada", 105644032, 509, "Mazmorra enaredada"),//lista
//                new MasmosDialog("chantaclaus", 66323456, 369, "Mazmorra dechanta claus"),
//                new MasmosDialog("arca", 22283264, 369, "Arca de otomai"),
//                new MasmosDialog("ratasbonta", 27003904, 259, "Mazmorra de las ratas de bonta"),
//                new MasmosDialog("ratasbrakmar", 40109568, 246, "Mazmorra de las ratas de brakmas"),
//                new MasmosDialog("sflintercell", 102760961, 273, "Mazmorra de las ratas del castillo de amakna"),
//                new MasmosDialog("tofus", 96338946, 370, "Mazmorra de los tofus"),//lista
//                new MasmosDialog("tofureal", 96338950, 385, "Mazmorra del tofu real"),
//                new MasmosDialog("rasgabola", 22806530, 400, "Mazmorra rasgabola"),
//                new MasmosDialog("grutagrutesca", 5244416, 400, "Gruta grutesca"),
//                new MasmosDialog("skonk", 107483136, 400, "La guarida de skonk"),
//                new MasmosDialog("larvas", 96994817, 356, "Mazmorra de las larvas"),
//                new MasmosDialog("kwoknan", 64749568, 344, "Nido del kwoknan"),
//                new MasmosDialog("bworker", 104333825, 343, "Mazmorra del Bworker"),
//                new MasmosDialog("bworks", 104595969, 269, "Mazmorra de los bworks"),
//                new MasmosDialog("blatarata", 146675712, 428, "Escondrijo de blatarata"),
//                new MasmosDialog("blatarata", 182714368, 429, "Mansion del kuatropatas"),
//                new MasmosDialog("templomaldito", 182453248, 422, "Templo maldito de las araknas"),
//                new MasmosDialog("templougah", 182327297, 357, "Templo del gran ugah"),
//                new MasmosDialog("magiriktus", 181665792, 344, "Carpa de los magik riktus"),
//                new MasmosDialog("brumen", 176947200, 369, "Laboratorio de brumen tintoras"),
//                new MasmosDialog("talkasha", 176160768, 382, "Camara de tal kasha"),
//                new MasmosDialog("elpiko", 174064128, 412, "Caverna de El Piko"),
//                new MasmosDialog("meno", 169869312, 396, "Nave del capitan meno"),
//                new MasmosDialog("cuerbok", 159124486, 380, "Biblioteca del maestro cuerbok"),
//                new MasmosDialog("maxi", 155713536, 312, "Guarida del maxilubo"),
//                new MasmosDialog("lasoberana", 149160960, 340, "Guarida de Lasoberana"),
//                new MasmosDialog("pestruz", 132907008, 354, "Pajarera de thor pesptruz"),
//                new MasmosDialog("kanigrula", 125829635, 347, "Mazmorra de kanigrula"),
//                new MasmosDialog("kralamar", 26738688, 427, "Antro del kralamar gigante"),
//                new MasmosDialog("encantada", 163578368, 427, "Mazmorra encantada"),//lista 
//                new MasmosDialog("escarahojas", 94110720, 397, "Mazmorra de las escarahojas"),//lista
//        };

//        public override void Execute(GameTrigger trigger)
//        {

//            var character = trigger.Character;
//            if (!character.IsBusy())
//            {
//                var o = new MasmosDialog(trigger.Character);
//                o.Open();
//            }
//            else
//                character.SendServerMessage("Debes estar desocupado para usar este comando");
//        }

//        private class MasmosDialog : IDialog
//        {
//            readonly List<MasmosDialog> m_destinations = new List<MasmosDialog>();

//            public MasmosDialog(Character character)
//            {
//                Character = character;
//            }

//            public MasmosDialog(List<MasmosDialog> map, Cell cell, String description)
//            {
//                AddDestination(map);
//            }

//            public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_TELEPORTER;

//            public Character Character
//            {
//                get;
//            }

//            public List<Map> Map
//            {
//                get;
//            }

//            public void AddDestination(List<MasmosDialog> map)
//            {
//                m_destinations.Add(map);
//            }

//            public void Open()
//            {
//                Character.SetDialog(this);
//                SendZaapListMessage(Character.Client);
//            }

//            public void Close()
//            {
//                Character.CloseDialog(this);
//                DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
//            }

//            public void Teleport(Map map)
//            {
//                if (!m_destinations.Contains(map))
//                    return;

//                Cell cell;

                

//                //Character.Teleport(map, cell);

//                Close();
//            }

//            public void SendZaapListMessage(IPacketReceiver client)
//            {
               
//            }
//        }
//    }


//}
