

// Generated on 02/19/2017 13:42:14
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("ItemCriterionOperator", "com.ankamagames.dofus.datacenter.items.criterion")]
    [Serializable]
    public class ItemCriterionOperator : IDataObject
    {
        public const String SUPERIOR = ">";
        public const String INFERIOR = "<";
        public const String EQUAL = "";
        public const String DIFFERENT = "!";
    }
}