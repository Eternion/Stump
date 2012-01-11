namespace Stump.Server.BaseServer.IPC.Objects
{
    public enum RegisterResultEnum
    {
        OK,
        ContextNotFound,
        ChannelNotFound,
        IpNotAllowed,
        PropertiesMismatch,
        AlreadyRegistered,
        IpcConnectionFailed,
        AuthServerUnreachable,
        UnknownError,
    }
}