namespace Nox.Mermaid.Exceptions;

[Serializable]
public class NoxErdParserException: Exception
{
    public NoxErdParserException(string message): base(message)
    {
            
    }
        
    public NoxErdParserException(string message, Exception innerException): base(message, innerException)
    {
        
    }
}