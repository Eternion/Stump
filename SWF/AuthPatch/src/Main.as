package
{
	import flash.display.Sprite;
    import flash.events.Event;
	import flash.utils.getQualifiedClassName;
	import com.ankamagames.jerakine.network.ServerConnection;
	import com.ankamagames.jerakine.logger.Logger;
	import com.ankamagames.jerakine.logger.Log;
    import com.ankamagames.dofus.logic.connection.managers.AuthentificationManager;
    import com.ankamagames.dofus.kernel.net.ConnectionsHandler;
	import com.ankamagames.jerakine.data.XmlConfig;
    import ClearIdentificationMessage;
	
    public class Main extends Sprite
    {
		protected static const _log:Logger = Log.getLogger(getQualifiedClassName(Main));

        public function Main()
        {
            try
            {
                var authentificationManager:AuthentificationManager = AuthentificationManager.getInstance();

				var autoSelectServer:Boolean = authentificationManager.loginValidationAction.autoSelectServer;
				var lang:String = XmlConfig.getInstance().getEntry("config.lang.current");
                var username:String = authentificationManager.loginValidationAction.username;
                var password:String = authentificationManager.loginValidationAction.password;
				var serverId:uint = authentificationManager.loginValidationAction.serverId;
				var ipAddress:String = (ConnectionsHandler.getConnection().mainConnection as ServerConnection).toString();

                var msg:ClearIdentificationMessage = new ClearIdentificationMessage();
                msg.initClearIdentificationMessage(autoSelectServer, lang, username, password, serverId, ipAddress);

                ConnectionsHandler.getConnection().send(msg);
            }
            catch (e:Error)
            {
				_log.error(e.getStackTrace());
            }
        }

    }

}
