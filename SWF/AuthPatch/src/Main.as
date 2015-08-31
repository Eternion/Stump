//
// Main.as
// CawotteSrv
//
// Created by Luax on 7/18/15.
// Copyright (c) 2015 Luax. All rights reserved.
//

package
{
    import flash.display.Sprite;
    import flash.events.Event;
    import com.ankamagames.dofus.logic.connection.managers.AuthentificationManager;
    import com.ankamagames.dofus.kernel.net.ConnectionsHandler;
    import ClearIdentificationMessage;

    public class Main extends Sprite
    {

        public function Main()
        {
            try
            {
                var authentificationManager:AuthentificationManager = AuthentificationManager.getInstance();

                var username:String = authentificationManager.loginValidationAction.username;
                var password:String = authentificationManager.loginValidationAction.password;

                var cim:ClearIdentificationMessage = new ClearIdentificationMessage();
                cim.initClearIdentificationMessage(username, password);

                ConnectionsHandler.getConnection().send(cim);
            }
            catch (e:*)
            {
                trace("Error: " + e);
            }
        }

    }

}
