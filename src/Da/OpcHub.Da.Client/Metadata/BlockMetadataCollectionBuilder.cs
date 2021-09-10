using OpcHub.Da.Client.Attributes;
using OpcHub.Da.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpcHub.Da.Client.Metadata
{
    internal static class BlockMetadataCollectionBuilder
    {
        public static IMetaBlockCollection Build(IEnumerable<Assembly> assembliesToScan, BlockOption blockOption)
        {
            if (!assembliesToScan.Any())
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for opc blocks.");

            assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();

            List<TypeInfo> opcBlockTypes = GetOpcBlockTypes(assembliesToScan);
            if (opcBlockTypes == null || opcBlockTypes.Count == 0)
                throw new InvalidOperationException("Can't find any classes flagged with 'OpcBlockAttribute'.");

            List<BlockMetadata> blocks = new List<BlockMetadata>();
            foreach (var opcBlockType in opcBlockTypes)
            {
                BlockMetadata block = new BlockMetadata();
                block.Configure(opcBlockType, blockOption);

                blocks.Add(block);
            }

            return new BlockMetadataCollection(blocks);
        }

        private static List<TypeInfo> GetOpcBlockTypes(IEnumerable<Assembly> assembliesToScan)
        {
            bool IsOpcBlockAttribute(Attribute[] attributes)
            {
                foreach (Attribute a in attributes)
                {
                    if (a is OpcBlockAttribute) return true;
                }

                return false;
            }

            return assembliesToScan
                .SelectMany(a => a.DefinedTypes)
                .Where(t => IsOpcBlockAttribute(Attribute.GetCustomAttributes(t, false)))
                .ToList();
        }
    }
}
