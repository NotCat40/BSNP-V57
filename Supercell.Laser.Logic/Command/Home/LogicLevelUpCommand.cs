namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class LogicLevelUpCommand : Command
    {
        private int CharacterId;
        const double GEMS_PER_COIN = 55.0 / 290;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt(); // class
            CharacterId = stream.ReadVInt(); // я забыл
        }

        public override int Execute(HomeMode homeMode)
        {
            Hero hero = homeMode.Avatar.GetHero(GlobalId.CreateGlobalId(16, CharacterId));
            hero.PowerLevel++;
            return 0;
        }

        public override int GetCommandType()
        {
            return 520;
        }
    }
}
