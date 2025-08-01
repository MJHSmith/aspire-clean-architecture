namespace SharedKernel;

public enum ErrorType
{
    /// <summary>
    /// Represents an unexpected/unanticipated failure.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// Represents a failure during validation.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// Represents a problem that can occur, but should not.
    /// </summary>
    Problem = 2,

    /// <summary>
    /// Represents not finding a resource.
    /// </summary>
    NotFound = 3,

    /// <summary>
    /// Represents a resource conflict.
    /// </summary>
    Conflict = 4
}
