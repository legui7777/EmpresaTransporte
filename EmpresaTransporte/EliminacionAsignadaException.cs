using System;
using System.Runtime.Serialization;

public class EliminacionAsignadaException : Exception, ISerializable
{
	public EliminacionAsignadaException()
	{
	}

	public EliminacionAsignadaException(string message) : base(message)
	{
	}

	public EliminacionAsignadaException(string message, Exception innerException) : base(message, innerException)
	{
	}

	protected EliminacionAsignadaException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
