using System.Text;

namespace ReferenceManager.Services;

public record BibtexEntry(string Type, string Key, Dictionary<string, string> Fields);

public static class BibtexParser
{
    public static List<BibtexEntry> Parse(string content)
    {
        var entries = new List<BibtexEntry>();
        int i = 0;
        while (i < content.Length)
        {
            i = content.IndexOf('@', i);
            if (i < 0) break;
            i++;

            var typeStart = i;
            while (i < content.Length && char.IsLetter(content[i])) i++;
            var type = content[typeStart..i].ToLowerInvariant();

            while (i < content.Length && char.IsWhiteSpace(content[i])) i++;
            if (i >= content.Length || content[i] != '{') continue;
            i++;

            while (i < content.Length && char.IsWhiteSpace(content[i])) i++;
            var keyStart = i;
            while (i < content.Length && content[i] != ',' && content[i] != '}' && !char.IsWhiteSpace(content[i])) i++;
            var key = content[keyStart..i].Trim();

            while (i < content.Length && content[i] != ',' && content[i] != '}') i++;
            if (i >= content.Length || content[i] != ',') continue;
            i++;

            var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            while (i < content.Length)
            {
                while (i < content.Length && (char.IsWhiteSpace(content[i]) || content[i] == ',')) i++;
                if (i >= content.Length || content[i] == '}') break;

                var fieldStart = i;
                while (i < content.Length && content[i] != '=' && content[i] != '}' && content[i] != '@') i++;
                if (i >= content.Length || content[i] != '=') break;
                var fieldName = content[fieldStart..i].Trim().ToLowerInvariant();
                i++;

                while (i < content.Length && char.IsWhiteSpace(content[i])) i++;
                if (i >= content.Length) break;

                string value;
                if (content[i] == '{')
                {
                    value = ReadBraced(content, ref i);
                }
                else if (content[i] == '"')
                {
                    i++;
                    var valStart = i;
                    while (i < content.Length && content[i] != '"') i++;
                    value = content[valStart..i];
                    if (i < content.Length) i++;
                }
                else
                {
                    var valStart = i;
                    while (i < content.Length && content[i] != ',' && content[i] != '}') i++;
                    value = content[valStart..i].Trim();
                }

                if (!string.IsNullOrEmpty(fieldName))
                    fields[fieldName] = value;
            }

            if (i < content.Length && content[i] == '}') i++;
            entries.Add(new BibtexEntry(type, key, fields));
        }
        return entries;
    }

    private static string ReadBraced(string content, ref int i)
    {
        i++;
        var sb = new StringBuilder();
        int depth = 1;
        while (i < content.Length && depth > 0)
        {
            var c = content[i++];
            if (c == '{') { depth++; sb.Append(c); }
            else if (c == '}') { depth--; if (depth > 0) sb.Append(c); }
            else sb.Append(c);
        }
        return sb.ToString();
    }
}
