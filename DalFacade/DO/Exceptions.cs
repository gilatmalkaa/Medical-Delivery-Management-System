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

/// <summary>
/// Exception thrown when an XML data file cannot be loaded or created.
/// Typically used when initializing or loading XML files in the DAL.
///
/// For example:
/// - Missing XML file
/// - Corrupted XML file
/// - Failure to create a new XML file
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalXMLFileLoadCreateException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DalXMLFileLoadCreateException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DalXMLFileLoadCreateException"/> class
    /// with a specified error message and an inner exception.
    /// Useful when wrapping underlying XML or IO errors.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="inner">The underlying exception.</param>
    public DalXMLFileLoadCreateException(string? message, Exception inner)
        : base(message, inner) { }
}
