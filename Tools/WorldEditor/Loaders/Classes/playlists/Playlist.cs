

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Playlist", "com.ankamagames.dofus.datacenter.playlists")]
    [Serializable]
    public class Playlist : IDataObject, IIndexedData
    {
        public const int AMBIENT_TYPE_ROLEPLAY = 1;
        public const int AMBIENT_TYPE_AMBIENT = 2;
        public const int AMBIENT_TYPE_FIGHT = 3;
        public const int AMBIENT_TYPE_BOSS = 4;
        public const String MODULE = "Playlists";
        public int id;
        public int silenceDuration;
        public int iteration;
        public int type;
        public List<PlaylistSound> sounds;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int SilenceDuration
        {
            get { return this.silenceDuration; }
            set { this.silenceDuration = value; }
        }
        [D2OIgnore]
        public int Iteration
        {
            get { return this.iteration; }
            set { this.iteration = value; }
        }
        [D2OIgnore]
        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        [D2OIgnore]
        public List<PlaylistSound> Sounds
        {
            get { return this.sounds; }
            set { this.sounds = value; }
        }
    }
}