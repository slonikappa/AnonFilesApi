namespace AnonFilesApi.Exceptions;

public class InvalidDownloadLinkException : Exception
{
    public InvalidDownloadLinkException()
    {
    }

    public InvalidDownloadLinkException(string message)
        : base(message)
    {
    }

    public InvalidDownloadLinkException(string message, Exception inner)
        : base(message, inner)
    {
    }
}