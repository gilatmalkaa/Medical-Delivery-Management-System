namespace BO;

/// <summary>
/// Thrown when requested BL entity does not exist.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Thrown when trying to create entity that already exists.
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Thrown when BL object contains null values where forbidden.
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
