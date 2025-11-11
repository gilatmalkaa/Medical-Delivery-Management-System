namespace DO;

/// <summary>
/// Exception thrown when attempting to access or operate on a DAL entity that does not exist.
/// Typically used when reading, updating, or deleting a non-existing item.
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalDoesNotExistException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to create a DAL entity that already exists.
/// Typically used when adding a duplicate item to the data source.
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalAlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DalAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when an unsupported operation is attempted in the DAL.
/// Typically used for operations not implemented or not allowed for certain entities.
/// </summary>
[Serializable]
public class DalUnsupportedOperationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalUnsupportedOperationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DalUnsupportedOperationException(string? message) : base(message) { }
}
