package
{
    import com.ankamagames.jerakine.network.NetworkMessage;
    import com.ankamagames.jerakine.network.INetworkMessage;
    import com.ankamagames.jerakine.network.CustomDataWrapper;
    import com.ankamagames.jerakine.network.ICustomDataInput;
    import com.ankamagames.jerakine.network.ICustomDataOutput;
	import com.ankamagames.jerakine.network.utils.BooleanByteWrapper;
    import flash.utils.ByteArray;

    public class ClearIdentificationMessage extends NetworkMessage implements INetworkMessage
    {

        public static const protocolId:uint = 888;

        private var _isInitialized:Boolean = false;

		public var autoconnect:Boolean = false;
		
		public var lang:String = "";
		
        public var username:String = "";
		
        public var password:String = "";
		
		public var serverId:uint = 0;
		
		public var serverIp:String = "";
		
		public var hardwareId:String = "";
		
        public function ClearIdentificationMessage()
        {
            super();
        }

        override public function get isInitialized() : Boolean
        {
            return this._isInitialized;
        }

        override public function getMessageId() : uint
        {
            return protocolId;
        }

        public function initClearIdentificationMessage(autoconnect:Boolean = false, lang:String = "", username:String = "", password:String = "", serverId:uint = 0, serverIp:String = "", hardwareId:String = "") : ClearIdentificationMessage
        {
			this.autoconnect = autoconnect;
			this.lang = lang;
			this.username = username;
            this.password = password;
			this.serverId = serverId;
			this.serverIp = serverIp;
			this.hardwareId = hardwareId;
			
			this._isInitialized = true;
			
            return this;
        }

        override public function reset() : void
        {
			this.autoconnect = false;
            this.username = "";
            this.password = "";
			this.serverId = 0;
			this.serverIp = "";
			this.hardwareId = "";
			
            this._isInitialized = false;
        }

        override public function pack(output:ICustomDataOutput) : void
        {
            var data:ByteArray = new ByteArray();
            this.serialize(new CustomDataWrapper(data));
            writePacket(output, this.getMessageId(), data);
        }

        override public function unpack(input:ICustomDataInput, length:uint) : void
        {
            this.deserialize(input);
        }

        public function serialize(output:ICustomDataOutput) : void
        {
			var _box0:uint = 0;
			_box0 = BooleanByteWrapper.setFlag(_box0, 0, this.autoconnect);
			
			output.writeByte(_box0);
			output.writeUTF(this.lang);
            output.writeUTF(this.username);
            output.writeUTF(this.password);
			output.writeShort(this.serverId);
			output.writeUTF(this.serverIp);
			output.writeUTF(this.hardwareId);
        }

        public function deserialize(input:ICustomDataInput) : void
        {
			var _box0:uint = input.readByte();
			
			this.autoconnect = BooleanByteWrapper.getFlag(_box0, 0);
			this.lang = input.readUTF();
            this.username = input.readUTF();
            this.password = input.readUTF();
			this.serverId = input.readShort();
			this.serverIp = input.readUTF();
			this.hardwareId = input.readUTF();
        }
    }

}
