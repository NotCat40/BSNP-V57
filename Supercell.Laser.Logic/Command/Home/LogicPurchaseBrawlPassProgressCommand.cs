namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseBrawlPassProgressCommand : Command
    {
        public int Unknown { get; set; }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            Unknown = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (homeMode.Avatar.UseDiamonds(30))
            {
                for (int x = 966; x < 1055 + 1; x++)
                {
                    MilestoneData milestoneData = DataTables.Get(DataType.Milestone).GetDataByGlobalId<MilestoneData>(GlobalId.CreateGlobalId((int)DataType.Milestone, x));
                    if (milestoneData.ProgressStart <= homeMode.Home.BrawlPassTokens && (milestoneData.ProgressStart + milestoneData.Progress) > homeMode.Home.BrawlPassTokens)
                    {
                        homeMode.Home.BrawlPassTokens = milestoneData.ProgressStart + milestoneData.Progress;
                        return 0;
                    }
                }
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 536;
        }
    }
}
