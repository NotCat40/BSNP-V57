namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetDoNotDisturb : Command
    {
        public int DoNotDistrub;

        public override void Encode(ByteStream stream)
        {

            stream.WriteVInt(DoNotDistrub);
            stream.WriteVInt(0);
            base.Encode(stream);

        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 213;
        }
    }
}
