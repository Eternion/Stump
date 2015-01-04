namespace Stump.DofusProtocol.Messages.Custom
{
    public class ErrorMessage : Message
    {
        public ErrorMessage(byte[] data, string error)
        {
            this.data = data;
            this.error = error;
        }
        public byte[] data;
        public string error;

        public override uint MessageId
        {
            get { return 0; }
        }

        public override void Serialize(Core.IO.IDataWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override void Deserialize(Core.IO.IDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}