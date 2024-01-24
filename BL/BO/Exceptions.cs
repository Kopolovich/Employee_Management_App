namespace BO;

/// <summary>
/// exception for non existing item
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string? message, Exception innerException) 
                                                    : base(message, innerException) { }
}

/// <summary>
/// exception for already existing item
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string? message, Exception innerException)
                                                    : base(message, innerException) { }
}

/// <summary>
/// exception for impossilbe deletion
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }
    public BlDeletionImpossibleException(string? message, Exception innerException)
                                                   : base(message, innerException) { }
}

/// <summary>
/// exception for failing to create xml file
/// </summary>
[Serializable]
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string? message, Exception innerException)
                                                  : base(message, innerException) { }
}

/// <summary>
/// exception for trying to use property with null value
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

/// <summary>
/// exception for trying to use property with null value
/// </summary>
[Serializable]
public class BlInvalidValueException : Exception
{
    public BlInvalidValueException(string? message) : base(message) { }
}
