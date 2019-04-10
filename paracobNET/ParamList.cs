﻿using System.IO;
using System.Collections.Generic;

namespace paracobNET
{
    public class ParamList : IParam
    {
        public ParamType TypeKey { get; } = ParamType.list;
        
        public List<IParam> Nodes { get; private set; }

        public ParamList() { }
        public ParamList(List<IParam> nodes)
        {
            Nodes = nodes;
        }

        internal void Read(BinaryReader reader)
        {
            uint startPos = (uint)reader.BaseStream.Position - 1;
            int count = reader.ReadInt32();
            Nodes = new List<IParam>(count);
            uint[] offsets = new uint[count];

            //all elements should be the same type but it's not enforced

            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                reader.BaseStream.Seek(startPos + offsets[i], SeekOrigin.Begin);
                Nodes.Add(Util.ReadParam(reader));
            }
        }
        internal void Write(BinaryWriter writer, RefTableEntry parent)
        {
            uint startPos = (uint)writer.BaseStream.Position - 1;

            int count = Nodes.Count;
            writer.Write(count);

            uint[] offsets = new uint[Nodes.Count];
            long ptrStartPos = writer.BaseStream.Position;
            writer.BaseStream.Seek(4 * count, SeekOrigin.Current);
            for (int i = 0; i < count; i++)
            {
                var node = Nodes[i];
                offsets[i] = (uint)(writer.BaseStream.Position - startPos);
                Util.WriteParam(node, writer, parent);
            }
            long endPos = writer.BaseStream.Position;
            writer.BaseStream.Seek(ptrStartPos, SeekOrigin.Begin);
            foreach (var offset in offsets)
                writer.Write(offset);
            writer.BaseStream.Seek(endPos, SeekOrigin.Begin);
        }

        public IParam Clone()
        {
            ParamList clone = new ParamList();
            clone.Nodes = new List<IParam>(Nodes.Count);
            foreach (var node in Nodes)
                clone.Nodes.Add(node.Clone());
            return clone;
        }
    }
}