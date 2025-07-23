using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;

namespace Majorsilence.Reporting.Data
{
    public class JsonTableExtractor
    {
        private readonly Dictionary<string, List<Dictionary<string, object>>> _tables = new();

        public Dictionary<string, IDataReader> Extract(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var rootArray = doc.RootElement;

            if (rootArray.ValueKind != JsonValueKind.Array)
                throw new InvalidOperationException("Root element must be an array.");

            _tables["root"] = new();

            foreach (var item in rootArray.EnumerateArray())
            {
                var row = new Dictionary<string, object>();
                var guid = Guid.NewGuid().ToString();
                row["__guid"] = guid;
                _tables["root"].Add(row);

                Flatten(item, row, "root", guid);
            }

            return _tables.ToDictionary(kv => kv.Key, kv => (IDataReader)new DictionaryDataReader(kv.Value));
        }

        private void Flatten(JsonElement element, Dictionary<string, object> currentRow, string currentTable,
            string parentGuid)
        {
            foreach (var prop in element.EnumerateObject())
            {
                var name = prop.Name;
                var val = prop.Value;

                switch (val.ValueKind)
                {
                    case JsonValueKind.Array:
                        // Handle arrays as before - creates child tables
                        var tableName = $"{currentTable}_{name}";
                        if (!_tables.ContainsKey(tableName))
                            _tables[tableName] = new();

                        foreach (var item in val.EnumerateArray())
                        {
                            var childRow = new Dictionary<string, object> { ["__parent_guid"] = parentGuid };

                            if (item.ValueKind == JsonValueKind.Object)
                                Flatten(item, childRow, tableName, parentGuid);
                            else
                                childRow[name] = GetPrimitiveValue(item);

                            _tables[tableName].Add(childRow);
                        }

                        break;

                    case JsonValueKind.Object:
                        // Two approaches for objects:
                        // 1. Create a nested dictionary and store the entire object
                        var nestedDict = new Dictionary<string, object>();
                        foreach (var nested in val.EnumerateObject())
                        {
                            if (nested.Value.ValueKind == JsonValueKind.Object ||
                                nested.Value.ValueKind == JsonValueKind.Array)
                            {
                                // For deeply nested objects, recursively process them
                                var childGuid = Guid.NewGuid().ToString();
                                var nestedTable = $"{currentTable}_{name}_{nested.Name}";

                                if (nested.Value.ValueKind == JsonValueKind.Object)
                                {
                                    // Handle nested object
                                    if (!_tables.ContainsKey(nestedTable))
                                        _tables[nestedTable] = new();

                                    var childRow = new Dictionary<string, object> { ["__parent_guid"] = parentGuid };
                                    Flatten(nested.Value, childRow, nestedTable, childGuid);
                                    _tables[nestedTable].Add(childRow);
                                }

                                nestedDict[nested.Name] = GetPrimitiveValue(nested.Value);
                            }
                            else
                            {
                                nestedDict[nested.Name] = GetPrimitiveValue(nested.Value);
                            }
                        }

                        currentRow[name] = nestedDict;

                        // 2. Also flatten the object properties with prefixed names (for backward compatibility)
                        foreach (var nested in val.EnumerateObject())
                        {
                            if (nested.Value.ValueKind != JsonValueKind.Object &&
                                nested.Value.ValueKind != JsonValueKind.Array)
                            {
                                currentRow[$"{name}_{nested.Name}"] = GetPrimitiveValue(nested.Value);
                            }
                        }

                        break;

                    default:
                        currentRow[name] = GetPrimitiveValue(val);
                        break;
                }
            }
        }

        private object? GetPrimitiveValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number when element.TryGetInt64(out var l) => l,
                JsonValueKind.Number => element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.ToString() // Fallback to raw JSON for unhandled types
            };
        }
    }
}