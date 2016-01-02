package
{
    import flash.display.Sprite;
    import flash.events.Event;
    import com.ankamagames.dofus.logic.connection.managers.AuthentificationManager;
    import com.ankamagames.dofus.kernel.net.ConnectionsHandler;
	import com.ankamagames.jerakine.data.XmlConfig;
    import ClearIdentificationMessage;

    public class Main extends Sprite
    {

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

                var cim:ClearIdentificationMessage = new ClearIdentificationMessage();
                cim.initClearIdentificationMessage(autoSelectServer, lang, username, password, serverId);

                ConnectionsHandler.getConnection().send(cim);
            }
            catch (e:*)
            {
                trace("Error: " + e);
            }
        }

    }

}
