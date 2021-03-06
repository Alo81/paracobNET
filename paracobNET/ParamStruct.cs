﻿using System;
using System.Collections.Generic;

namespace paracobNET
{
    [Serializable]
    public class ParamStruct : IParam
    {
        public ParamType TypeKey { get; } = ParamType.@struct;
        public Hash40Pairs<IParam> Nodes { get; set; }

        public ParamStruct()
        {
            Nodes = new Hash40Pairs<IParam>();
        }
        public ParamStruct(int capacity)
        {
            Nodes = new Hash40Pairs<IParam>(capacity);
        }
        public ParamStruct(Hash40Pairs<IParam> nodes)
        {
            Nodes = nodes;
        }

        public IParam Clone()
        {
            ParamStruct clone = new ParamStruct(Nodes.Count);
            foreach (var node in Nodes)
                clone.Nodes.Add(node.Key, node.Value.Clone());
            return clone;
        }
    }
}
