using Supercell.Laser.Logic.Home.Structures;
namespace Supercell.Laser.Logic.Message.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Helper;


    public class OwnHomeDataMessage : GameMessage
    {
        public OwnHomeDataMessage() : base()
        {
            ;
        }

        public ClientHome Home;
        public ClientAvatar Avatar;

        public static readonly int[] GoldPacksPrice = new int[]
        {
            20, 50, 140, 280
        };

        public static readonly int[] GoldPacksAmount = new int[]
        {
            150, 400, 1200, 2600
        };

        public override void Encode()
        {

            Home.Encode(Stream);
            Avatar.Encode(Stream);
            //Console.WriteLine("[DEBUG] [OwnHomeDataMessage::Encode] Message Created!");
        }

        public override int GetMessageType()
        {
            return 24101;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }

    }
}
