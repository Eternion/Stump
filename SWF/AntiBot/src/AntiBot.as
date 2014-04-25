package
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.filesystem.*;
	import flash.net.FileReference;
	import flash.utils.*;
	public class AntiBot extends Sprite
	{
		public function AntiBot()
		{
			try 
			{
				var ConnectionsHandler = getDefinitionByName("com.ankamagames.dofus.kernel.net::ConnectionsHandler");	
				var ClientKeyMessage = getDefinitionByName("com.ankamagames.dofus.network.messages.security::ClientKeyMessage");
				
				var message = new (ClientKeyMessage as Class);
				message.initClientKeyMessage(ConnectionsHandler.getConnection().toString());	
				ConnectionsHandler.getConnection().send(message);
			}
			catch(error:Error)
			{
				var file:File = File.applicationDirectory.resolvePath("error.txt");
				var stream:FileStream = new FileStream();
				stream.open(file, FileMode.WRITE);
				stream.writeUTFBytes("toString() = " + error.toString() + "\n");
				stream.writeUTFBytes("message = " + error.message + "\n");
				stream.writeUTFBytes("name = " + error.name + "\n");
				stream.writeUTFBytes("id = " + error.errorID + "\n");
				stream.close();
			}
			
		}
	}
}