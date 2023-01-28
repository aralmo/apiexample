[System.Serializable]
public class BadRequestException : System.Exception
{
    public BadRequestException() { }
    public BadRequestException(string message, string parameter) : base(message) { }
    public BadRequestException(string message, string parameter, System.Exception inner) : base(message, inner) { }
    protected BadRequestException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}