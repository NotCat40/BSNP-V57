namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicOpenRandomCommand : Command
    {
        public int Unk;
        public int RewardType;
        public int RewardAmount;
        public int Unk2;
        public int Unk3;
        public override void Decode(ByteStream stream)
        {
            
        }

        public override int Execute(HomeMode homeMode)
        {
            LogicRefreshRandomRewardsCommand logicRefreshRandomRewardsCommand = new LogicRefreshRandomRewardsCommand();
            logicRefreshRandomRewardsCommand.Execute(homeMode);

            AvailableServerCommandMessage message2 = new AvailableServerCommandMessage();
            message2.Command = logicRefreshRandomRewardsCommand;
            homeMode.GameListener.SendMessage(message2);

            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            DeliveryUnit unit = new DeliveryUnit(100);

            GatchaDrop drop = new GatchaDrop(homeMode.Home.StarrDrop.data.Type);

            drop.DataGlobalId = homeMode.Home.StarrDrop.data.DataGlobalID;
            if(homeMode.Home.StarrDrop.data.Type != 1)drop.SkinGlobalId = homeMode.Home.StarrDrop.data.SkinGlobalID;
            drop.Count = homeMode.Home.StarrDrop.data.Ammount;
            unit.AddDrop(drop);

            command.StarrDropExecute = true;
            command.DeliveryUnits.Add(unit);
            command.Execute(homeMode);

            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            homeMode.GameListener.SendMessage(message);
            return 0;
        }

        public override int GetCommandType()
        {
            return 571;
        }
    }
}
