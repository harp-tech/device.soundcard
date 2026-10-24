using System.Dynamic;

static class ExpandoHelper
{
    public static ExpandoObject FromDictionary(Dictionary<object, object> members, params string[] optionalMembers)
    {
        var result = new ExpandoObject();
        foreach (var kvp in members)
        {
            result.TryAdd((string)kvp.Key, kvp.Value);
        }
        
        foreach (var member in optionalMembers)
        {
            result.TryAdd(member, null);
        }
        return result;
    }
}