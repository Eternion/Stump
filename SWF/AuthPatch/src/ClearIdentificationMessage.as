//
// ClearIdentificationMessage.as
// CawotteSrv
//
// Created by Luax on 7/18/15.
// Copyright (c) 2015 Luax. All rights reserved.
//

package
{
    import com.ankamagames.jerakine.network.NetworkMessage;
    import com.ankamagames.jerakine.network.INetworkMessage;
    import com.ankamagames.jerakine.network.CustomDataWrapper;
    import com.ankamagames.jerakine.network.ICustomDataInput;
    import com.ankamagames.jerakine.network.ICustomDataOutput;
    import flash.utils.ByteArray;

    public class ClearIdentificationMessage extends NetworkMessage implements INetworkMessage
    {

        public static const protocolId:uint = 10000;

        private var _isInitialized:Boolean = false;

        public var username:String = "";

        public var password:String = "";

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

        public function initClearIdentificationMessage(username:String = "", password:String = "") : ClearIdentificationMessage
        {
            this.username = username;
            this.password = password;

            return this;
        }

        override public function reset() : void
        {
            this.username = "";
            this.password = "";

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
            output.writeUTF(this.username);
            output.writeUTF(this.password);
        }

        public function deserialize(input:ICustomDataInput) : void
        {
            this.username = input.readUTF();
            this.password = input.readUTF();
        }

    }

}
