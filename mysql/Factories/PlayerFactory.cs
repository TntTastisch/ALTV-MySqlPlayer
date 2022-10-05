using AltV.Net;
using AltV.Net.Elements.Entities;
using mysql.MyEntitys;

namespace mysql.Factories
{
    class PlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IServer server, IntPtr entityPointer, ushort id)
        {
            return new MyPlayer(server, entityPointer, id);
        }
    }
}
