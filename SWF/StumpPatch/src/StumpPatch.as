package
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.filesystem.*;
	import flash.net.FileReference;
	import flash.utils.*;
	public class StumpPatch extends Sprite
	{
		public function StumpPatch()
		{
			try 
			{
				var BuildInfos = getDefinitionByName("com.ankamagames.dofus::BuildInfos");
				var AirScanner = getDefinitionByName("com.ankamagames.jerakine.utils.system::AirScanner");
				var ClientInstallTypeEnum = getDefinitionByName("com.ankamagames.dofus.network.enums::ClientInstallTypeEnum");
				var XmlConfig = getDefinitionByName("com.ankamagames.jerakine.data::XmlConfig");
				var AuthentificationManager = getDefinitionByName("com.ankamagames.dofus.logic.connection.managers::AuthentificationManager").getInstance();
				var ConnectionsHandler = getDefinitionByName("com.ankamagames.dofus.kernel.net::ConnectionsHandler");	
				var ClientTechnologyEnum = getDefinitionByName("com.ankamagames.dofus.network.enums::ClientTechnologyEnum");
				
				var password = AuthentificationManager.loginValidationAction.password;
				var username = AuthentificationManager.loginValidationAction.username;
				var serverId = AuthentificationManager.loginValidationAction.serverId;
				var autoSelectServer = AuthentificationManager.loginValidationAction.autoSelectServer;
				
				
				var version = new (getDefinitionByName("com.ankamagames.dofus.network.types.version::VersionExtended") as Class);
				version.initVersionExtended(BuildInfos.BUILD_VERSION.major, 
					BuildInfos.BUILD_VERSION.minor, 
					BuildInfos.BUILD_VERSION.release, 
					AirScanner.isStreamingVersion() ? (70000) : (BuildInfos.BUILD_REVISION), 
					BuildInfos.BUILD_PATCH, 
					BuildInfos.BUILD_VERSION.buildType, 
					AirScanner.isStreamingVersion() ? (ClientInstallTypeEnum.CLIENT_STREAMING) : (ClientInstallTypeEnum.CLIENT_BUNDLE), 
					AirScanner.hasAir() ? (ClientTechnologyEnum.CLIENT_AIR) : (ClientTechnologyEnum.CLIENT_FLASH));
				
				var credentials:ByteArray = new ByteArray();
				credentials.writeUTF(username);
				credentials.writeUTF(password);
				
				var vector:Vector.<int> = new Vector.<int>;
				credentials.position = 0;
				while(credentials.bytesAvailable != 0)
				{
					vector.push(credentials.readByte());
				}
				
				var message = new (getDefinitionByName("com.ankamagames.dofus.network.messages.connection::IdentificationMessage") as Class);
				message.initIdentificationMessage(version, XmlConfig.getInstance().getEntry("config.lang.current"), 
					vector, serverId, autoSelectServer, false, false);	
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