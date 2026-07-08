public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, int id) 
        : base($"{entity} with ID {id} was not found") { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}