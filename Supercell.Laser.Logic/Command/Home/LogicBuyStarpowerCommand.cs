namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class LogicBuyStarpowerCommand : Command
    {
        private int ID;
        private int CharacterId1;
        private const double GEMS_PER_COIN = 0.1;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            ID = stream.ReadVInt(); 
            stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 557;
        }
    }
}
