using AltV.Net;
using AltV.Net.Elements.Entities;
using mysql.Database;

namespace mysql.MyEntitys
{
    class MyPlayer : Player
    {

        public bool IsLoggedIn { get; set; }
        public int Db_Id { get; set; }
        public string DisplayName { get; set; }
        public uint Cash { get; set; }
        public uint Bank { get; set; }
        public uint BlackMoney { get; set; }


        public MyPlayer(IServer server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            IsLoggedIn = false;
        }

        public void Login()
        {
            IsLoggedIn = true;
        }

        public void SendNotification(string msg)
        {
            Emit("mysql:notify", msg);
        }

        public void CreatePlayer(string username, string email, string password)
        {
            DisplayName = username;
            Db_Id = PlayerDatabase.CreatePlayer(DisplayName, email, password);
            Login();
        }

        public void LoadPLayer(string username)
        {
            DisplayName = username;
            PlayerDatabase.LoadPlayer(this);
            Login();
        }

        public void Save()
        {
            PlayerDatabase.UpdatePlayer(this);
        }
    }
}
