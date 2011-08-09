namespace Stump.Server.BaseServer.Database.Data
{
    public interface IRawSerializable<T> where T : new()
    {
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}