using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaJsonHelpers
{
    public static string? MergeJson(string? originalJson, string? updateJson)
    {
        if (string.IsNullOrWhiteSpace(originalJson))
            return updateJson;

        if (string.IsNullOrWhiteSpace(updateJson))
            return originalJson;

        JsonObject originalNode;
        JsonObject updateNode;

        try
        {
            originalNode = JsonNode.Parse(originalJson) as JsonObject ?? new JsonObject();
        }
        catch
        {
            originalNode = new JsonObject();
        }

        try
        {
            updateNode = JsonNode.Parse(updateJson) as JsonObject ?? new JsonObject();
        }
        catch
        {
            return originalJson;
        }

        foreach (var kv in updateNode)
        {
            originalNode[kv.Key] = kv.Value?.DeepClone();
        }

        return originalNode.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = false
        });
    }
}
