using System;
using System.Runtime.Serialization;

namespace DataAccess.Exceptions;
[Serializable]
public class EntityNotFoundException<T> : Exception
{
    private readonly long id;

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(long id)
    {
        this.id = id;
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public long Id => id;
}