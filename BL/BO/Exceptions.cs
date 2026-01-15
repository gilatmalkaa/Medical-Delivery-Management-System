namespace BO;

/// <summary>
/// Exception thrown when a requested business entity
/// does not exist in the system.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlDoesNotExistException(string? message) : base(message) { }

    /// <summary>
    /// Creates the exception with a message and an inner exception.
    /// </summary>
    public BlDoesNotExistException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when attempting to create
/// an entity that already exists.
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlAlreadyExistsException(string? message) : base(message) { }

    /// <summary>
    /// Creates the exception with a message and an inner exception.
    /// </summary>
    public BlAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a business object
/// contains null values where they are not allowed.
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlNullPropertyException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when a user attempts to perform
/// an operation without sufficient permissions.
/// </summary>
[Serializable]
public class BlPermissionException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlPermissionException(string? message) : base(message) { }

    /// <summary>
    /// Creates the exception with a message and an inner exception.
    /// </summary>
    public BlPermissionException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when invalid input is provided to the BL,
/// such as an illegal value or incorrect format.
/// </summary>
[Serializable]
public class BlInvalidInputException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlInvalidInputException(string? message)
        : base(message) { }

    /// <summary>
    /// Creates the exception with a message and an inner exception.
    /// </summary>
    public BlInvalidInputException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a business rule or constraint
/// is violated during an operation.
/// </summary>
public class BlInvalidOperationException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlInvalidOperationException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when authentication fails
/// due to invalid login credentials.
/// </summary>
public class BlInvalidCredentialsException : Exception
{
    /// <summary>
    /// Creates the exception with a descriptive message.
    /// </summary>
    public BlInvalidCredentialsException(string message)
        : base(message)
    {
    }
}
