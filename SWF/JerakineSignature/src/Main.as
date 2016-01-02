package 
{  
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import com.ankamagames.jerakine.utils.errors.SignatureError;
   
   public class Main
   {
      private function verifyV1Signature(param1:IDataInput, param2:ByteArray) : Boolean
      {
         var len:uint = 0;
         var input:IDataInput = param1;
         var output:ByteArray = param2;
         var formatVersion:uint = input.readShort();
         var sigData:ByteArray = new ByteArray();
         try
         {
            len = input.readInt();
            input.readBytes(sigData,0,len);
         }
         catch(e:Error)
         {
            throw new SignatureError("Invalid signature format, not enough data.",SignatureError.INVALID_SIGNATURE);
         }
         input.readBytes(output);
         output.position = 0;
         return true;
      }
   }
}