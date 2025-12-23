namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicClaimStarRoadCommand : Command
    {
        public int BrawlerId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            BrawlerId = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            int globalId = GlobalId.CreateGlobalId(16, BrawlerId);
            int creditsCost = 0;
            if (homeMode.Avatar.HasHero(globalId)) { return -1; }
            else
            {
                CharacterData characterData = DataTables.Get(16).GetDataByGlobalId<CharacterData>(globalId);
                if (characterData == null) return -1;
                CardData cardData = DataTables.Get(23).GetData<CardData>(characterData.Name + "_unlock");
                if (cardData == null) return -1;
                if (cardData.Rarity == "rare") { creditsCost = 160; }
                if (cardData.Rarity == "super_rare") { creditsCost = 430; }
                if (cardData.Rarity == "epic") { creditsCost = 925; }
                if (cardData.Rarity == "mega_epic") { creditsCost = 1900; }
                if (cardData.Rarity == "legendary") { creditsCost = 3800; }
                if (!homeMode.Avatar.UseRecruitTokens(creditsCost)) { return -2; }
                homeMode.Avatar.UnlockHero(characterData.GetGlobalId(), cardData.GetGlobalId());
                LogicBrawlerRecruitRoadChangedCommand command = new LogicBrawlerRecruitRoadChangedCommand();
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                command.homeMode = homeMode;
                message.Command = command;
                homeMode.GameListener.SendMessage(message);
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 562;
        }
    }
}
