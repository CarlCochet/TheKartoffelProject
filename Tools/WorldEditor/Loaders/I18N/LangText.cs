﻿using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Loaders.I18N
{
    [TableName("langs")]
    public class LangText : IAutoGeneratedRecord
    {
        [PrimaryKey("Id")]
        public uint Id { get; set; }

        [NullString]
        public string French { get; set; }

        [NullString]
        public string English { get; set; }

        [NullString]
        public string German { get; set; }

        [NullString]
        public string Spanish { get; set; }

        [NullString]
        public string Italian { get; set; }

        [NullString]
        public string Japanish { get; set; }

        [NullString]
        public string Dutsh { get; set; }

        [NullString]
        public string Portugese { get; set; }

        [NullString]
        public string Russish { get; set; }

        public void SetText(Languages language, string text)
        {
            switch (language)
            {
                case Languages.English:
                    English = text;
                    break;
                case Languages.French:
                    French = text;
                    break;
                case Languages.German:
                    German = text;
                    break;
                case Languages.Spanish:
                    Spanish = text;
                    break;
                case Languages.Italian:
                    Italian = text;
                    break;
                case Languages.Japanish:
                    Japanish = text;
                    break;
                case Languages.Dutsh:
                    French = text;
                    break;
                case Languages.Portugese:
                    Portugese = text;
                    break;
                case Languages.Russish:
                    Russish = text;
                    break;
                case Languages.All:
                    Portugese = text;
                    Russish = text;
                    Italian = text;
                    Spanish = text;
                    Japanish = text;
                    Dutsh = text;
                    English = text;
                    German = text;
                    French = text;
                    break;
            }
        }

        public string GetText(Languages language, Languages defaultLanguage)
        {
            switch (language)
            {
                case Languages.English:
                    return English;
                case Languages.French:
                    return French;
                case Languages.German:
                    return German;
                case Languages.Spanish:
                    return Spanish;
                case Languages.Italian:
                    return Italian;
                case Languages.Japanish:
                    return Japanish;
                case Languages.Dutsh:
                    return French;
                case Languages.Portugese:
                    return Portugese;
                case Languages.Russish:
                    return Russish;
                default:
                    return GetText(defaultLanguage);
            }
        }

        public string GetText(Languages language)
        {
            switch (language)
            {
                case Languages.English:
                    return English;
                case Languages.French:
                    return French;
                case Languages.German:
                    return German;
                case Languages.Spanish:
                    return Spanish;
                case Languages.Italian:
                    return Italian;
                case Languages.Japanish:
                    return Japanish;
                case Languages.Dutsh:
                    return French;
                case Languages.Portugese:
                    return Portugese;
                case Languages.Russish:
                    return Russish;
                default:
                    throw new Exception(string.Format("Unknow language {0}", language));
            }
        }

        public LangText Copy()
        {
            return (LangText)base.MemberwiseClone();
        }
    }

    public class LangTextRelator
    {
        public static string FetchQuery = "SELECT * FROM langs";
        public static string FetchById = "SELECT * FROM langs WHERE Id=@0";
    }
}
