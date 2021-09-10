using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpcHub.Da.Client.Metadata;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Client.Services
{
    internal class OpcDaContext : IOpcDaContext
    {
        private readonly IMetaBlockCollection _metaBlockCollection;
        private readonly IReadService _readService;
        private readonly IWriteService _writeService;
        private readonly ILogger<OpcDaContext> _logger;

        public OpcDaContext(
            IMetaBlockCollection metaBlockCollection,
            IReadService readService,
            IWriteService writeService,
            ILogger<OpcDaContext> logger)
        {
            _metaBlockCollection = metaBlockCollection;
            _readService = readService;
            _writeService = writeService;
            _logger = logger;
        }

        public bool ShortPooling { get; set; }

        public async Task<ReadCommandResult> Read(string tag)
        {
            if (tag == null || tag.Trim().Length == 0)
                throw new ArgumentNullException(nameof(tag));

            return await Read(new List<string> {tag});
        }

        public async Task<ReadCommandResult> Read(List<string> tags)
        {
            if (tags == null || tags.Count == 0)
                throw new ArgumentNullException(nameof(tags));

            return await _readService.Read(new ReadCommandRequest {ShortPooling = ShortPooling, Tags = tags});
        }

        public async Task<T> Read<T>(string blockName = null)
        {
            (string BlockName, List<string> Tags) tagResult = _metaBlockCollection.GetReadTags<T>(blockName);

            ReadCommandResult result = await Read(tagResult.Tags);
            return _metaBlockCollection.ToEntity<T>(tagResult.BlockName, result);
        }

        public async Task<List<(string BlockName, T BlockData)>> Read<T>(List<string> blockNames)
        {
            #region Checks

            if (blockNames == null || blockNames.Count == 0)
                throw new ArgumentNullException(nameof(blockNames));

            if (blockNames.Any(blockName => blockName == null || blockName.Trim().Length == 0))
                throw new ArgumentException("Null or empty block name is given in blockNames when Read<T>.");

            blockNames = blockNames.Select(blockName => blockName.Trim()).ToList();
            if (blockNames.Distinct().Count() != blockNames.Count)
                throw new ArgumentException("Duplicate block names were given in blockNames when Read<T>.");

            #endregion

            List<string> tags = new List<string>();
            foreach (string blockName in blockNames)
            {
                (string BlockName, List<string> Tags) tagResult = _metaBlockCollection.GetReadTags<T>(blockName);
                tags.AddRange(tagResult.Tags);
            }

            ReadCommandResult result = await Read(tags);
            List<(string BlockName, T BlockData)> blocks = new List<(string BlockName, T BlockData)>();
            foreach (string blockName in blockNames)
            {
                blocks.Add((blockName, _metaBlockCollection.ToEntity<T>(blockName, result)));
            }

            return blocks;
        }

        public async Task<WriteCommandResult> Write(WriteItemValue itemValue)
        {
            return await Write(new List<WriteItemValue> {itemValue});
        }

        public async Task<WriteCommandResult> Write(List<WriteItemValue> itemValues)
        {
            return await _writeService.Write(new WriteCommandRequest {ShortPolling = ShortPooling, ItemValues = itemValues});
        }

        public async Task<WriteCommandResult> Write<T>(T block)
        {
            return await Write(_metaBlockCollection.GetWriteItems(block));
        }

        public async Task<WriteCommandResult> Write<T>(T block, string blockName)
        {
            return await Write(_metaBlockCollection.GetWriteItems(block, blockName));
        }
    }
}