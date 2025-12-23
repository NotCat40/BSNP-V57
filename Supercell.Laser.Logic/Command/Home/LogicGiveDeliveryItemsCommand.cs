namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;

    public class LogicGiveDeliveryItemsCommand : Command
    {
        public readonly List<DeliveryUnit> DeliveryUnits;
        public int RewardTrackType { get; set; }
        public int RewardForRank { get; set; }
        public int BrawlPassSeason { get; set; }

        public bool BrawlPassExecute { get; set; }

        public bool StarrDropExecute { get; set; }

        public LogicGiveDeliveryItemsCommand() : base()
        {
            DeliveryUnits = new List<DeliveryUnit>();
        }

        public override void Encode(ByteStream stream)
        {
                stream.WriteVInt(0);

                stream.WriteVInt(DeliveryUnits.Count);
                foreach (DeliveryUnit unit in DeliveryUnits)
                {
                    unit.Encode(stream);
                }

                if (StarrDropExecute)
                {
                    stream.WriteBoolean(true);
                    stream.WriteVInt(200);
                    stream.WriteVInt(200);
                    stream.WriteVInt(5);
                    {
                        stream.WriteVInt(93);
                        stream.WriteVInt(206);
                        stream.WriteVInt(456);
                        stream.WriteVInt(1001);
                        stream.WriteVInt(2264);
                    }
                }
                else
                {
                    stream.WriteBoolean(false);
                }

                stream.WriteVInt(RewardTrackType);
                stream.WriteVInt(RewardForRank);
                stream.WriteVInt(BrawlPassSeason);
                stream.WriteVInt(5);

                stream.WriteByte(2);

                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteBoolean(false);
            
            base.Encode(stream);

        }

        public override int Execute(HomeMode homeMode)
        {
            foreach (DeliveryUnit unit in DeliveryUnits)
            {
                foreach (GatchaDrop drop in unit.GetDrops())
                {
                    drop.DoDrop(homeMode);
                }
            }

            return 0;
        }

        public override int GetCommandType()
        {
            return 203;
        }
    }
}
