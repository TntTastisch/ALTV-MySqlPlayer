using AltV.Net;
using AltV.Net.Elements.Entities;
using mysql.Database;
using mysql.Factories;



namespace mysql
{
    class ServerHandler : Resource
    {
        public override void OnStart()
        {
            Alt.Log("Plugin enabled. Coded by TntTastisch");
            new MyDatabase();
        }

        public override void OnStop()
        {
            Alt.Log("Plugin disabled. Coded by TntTastisch");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new PlayerFactory();
        }
    }
}
