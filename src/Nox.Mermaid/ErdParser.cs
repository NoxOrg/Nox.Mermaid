using Nox.Mermaid.Exceptions;
using Nox.Solution;
using Nox.Types;

namespace Nox.Mermaid;

public class ErdParser
{
    private readonly IEnumerable<Entity> _entities;

    public ErdParser(NoxSolution solution)
    {
        if (solution == null) throw new NoxErdParserException("Cannot render mermaid ER diagram. solution in Nox configuration is null");
        if (solution.Domain == null) throw new NoxErdParserException("Cannot render mermaid ER diagram. domain in Nox configuration is null");
        if (solution.Domain.Entities == null) throw new NoxErdParserException("Cannot render mermaid ER diagram. No entities defined in Nox domain configuration");
        _entities = solution.Domain.Entities;
        
    }

    public string Parse()
    {
        var lines = new List<string>();
        lines.Add("erDiagram");
        AddEntities(lines);
        
        return string.Join(System.Environment.NewLine, lines);
    }

    private void AddEntities(List<string> lines)
    {
        foreach (var entity in _entities)
        {
            lines.Add($"    {entity.Name} {{");
            AddKeys(lines, entity);
            AddAttributes(lines, entity.Attributes!);
            lines.Add("    }");
            AddRelationships(lines, entity);
        }
    }
    
    private void AddKeys(List<string> lines, Entity entity)
    {
        if (entity.Keys != null)
        {
            foreach (var key in entity.Keys)
            {
                var comment = "";
                if (!string.IsNullOrWhiteSpace(key.Description))
                {
                    comment = $" \"{key.Description}\"";
                }

                var keyType = "";
                switch (key.Type)
                {
                    case NoxType.Entity:
                        keyType = "FK";
                        break;
                    default:
                        keyType = "PK";
                        break;
                }

                lines.Add($"        {key.Type} {key.Name} {keyType}{comment}");
            }
        }
    }

    private void AddAttributes(List<string> lines, IReadOnlyList<NoxSimpleTypeDefinition> attributes)
    {
        foreach (var attr in attributes)
        {
            lines.Add($"        {attr.Type} {attr.Name}");
        }
    }

    private void AddRelationships(List<string> lines, Entity entity)
    {
        if (entity.Relationships != null)
        {
            foreach (var relationship in entity.Relationships)
            {
                var label = GetRelationshipLabel(relationship.Relationship);
                lines.Add($"    {entity.Name} {label} {relationship.Entity} : \"{relationship.Description}\"");
            }
        }

        if (entity.OwnedRelationships != null)
        {
            foreach (var relationship in entity.OwnedRelationships)
            {
                var label = GetRelationshipLabel(relationship.Relationship);
                lines.Add($"    {entity.Name} {label} {relationship.Entity} : \"{relationship.Description}\"");
            }
        }
    }

    private string GetRelationshipLabel(EntityRelationshipType relationshipType)
    {
        switch (relationshipType)
        {
            case EntityRelationshipType.ExactlyOne:
                return "||--||";
            case EntityRelationshipType.ZeroOrOne:
                return "|o--o|";
            case EntityRelationshipType.OneOrMany:
                return "}|--|{";
            case EntityRelationshipType.ZeroOrMany:
                return "}o--o{";
        }

        return "";
    }
    
}