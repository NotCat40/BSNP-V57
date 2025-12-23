namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Logic.Util;
    using System.Runtime.InteropServices;

    public class LogicClaimMasteriesCommand : Command
    {
        private int Tick1;
        private int Brawler;
        private int MasteryIndex;

        public override void Decode(ByteStream stream)
        {
            Tick1 = stream.ReadVInt();
            stream.ReadVInt();
            stream.ReadVInt();
            stream.ReadVInt();
            stream.ReadVInt();
            Brawler = stream.ReadVInt();
            MasteryIndex = stream.ReadVInt()-1;
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 569;
        }
    }
}
