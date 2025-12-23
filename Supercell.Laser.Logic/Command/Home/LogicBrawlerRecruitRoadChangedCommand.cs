namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;

    public class LogicBrawlerRecruitRoadChangedCommand : Command
    {
        public HomeMode homeMode;
        public bool UnlockingBrawlers;
        public int NextBrawler;
        public int creditsCost; 
        public int gemsCost;

        public override void Encode(ByteStream stream)
        {
            stream.WriteVInt(0); // DayArrayRange
            stream.WriteVInt(0); // Timer
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0); // Road (if Poco Road - 1, if Brock Road - 2);
            stream.WriteVInt(1);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            List<int> unlockStarRoadList = homeMode.Home.UnlockStarRoad.ToList();
            for (int BrawlerIndex = unlockStarRoadList.Count - 1; BrawlerIndex >= 0; BrawlerIndex--)
            {
                if (homeMode.Avatar.HasHero(unlockStarRoadList[BrawlerIndex]))
                {
                    unlockStarRoadList.RemoveAt(BrawlerIndex);
                }
            }
            homeMode.Home.UnlockStarRoad = unlockStarRoadList.ToArray();

            stream.WriteBoolean(homeMode.Home.UnlockStarRoad.Length != 0);
            if (homeMode.Home.UnlockStarRoad.Length != 0)
            {
                {
                    stream.WriteDataReference(16, homeMode.Home.UnlockStarRoad[0] - 16000000);
                    CharacterData characterData = DataTables.Get(DataType.Character).GetData<CharacterData>(homeMode.Home.UnlockStarRoad[0] - 16000000);
                    CardData cardData = DataTables.Get(DataType.Card).GetData<CardData>(characterData.Name + "_unlock");
                    if (cardData.Rarity == "rare") { creditsCost = 160; gemsCost = 29; }
                    if (cardData.Rarity == "super_rare") { creditsCost = 430; gemsCost = 79; }
                    if (cardData.Rarity == "epic") { creditsCost = 925; gemsCost = 169; }
                    if (cardData.Rarity == "mega_epic") { creditsCost = 1900; gemsCost = 349; }
                    if (cardData.Rarity == "legendary") { creditsCost = 3800; gemsCost = 699; }
                    stream.WriteVInt(creditsCost);
                    stream.WriteVInt(gemsCost);
                    stream.WriteVInt(-1);
                    stream.WriteVInt(homeMode.Avatar.RecruitTokens); // Collected
                    stream.WriteVInt(0); // Index
                    stream.WriteVInt(0);
                }
                
                stream.WriteVInt(homeMode.Home.UnlockStarRoad.Length - 1);
                for (int BrawlerIndex = 1; BrawlerIndex < homeMode.Home.UnlockStarRoad.Length; BrawlerIndex++)
                {
                    stream.WriteDataReference(16, homeMode.Home.UnlockStarRoad[BrawlerIndex] - 16000000);
                    CharacterData characterData = DataTables.Get(DataType.Character).GetData<CharacterData>(homeMode.Home.UnlockStarRoad[BrawlerIndex] - 16000000);
                    CardData cardData = DataTables.Get(DataType.Card).GetData<CardData>(characterData.Name + "_unlock");
                    if (cardData.Rarity == "rare") { creditsCost = 160; gemsCost = 29; }
                    if (cardData.Rarity == "super_rare") { creditsCost = 430; gemsCost = 79; }
                    if (cardData.Rarity == "epic") { creditsCost = 925; gemsCost = 169; }
                    if (cardData.Rarity == "mega_epic") { creditsCost = 1900; gemsCost = 349; }
                    if (cardData.Rarity == "legendary") { creditsCost = 3800; gemsCost = 699; }
                    stream.WriteVInt(creditsCost);
                    stream.WriteVInt(gemsCost);
                    stream.WriteVInt(-1);
                    stream.WriteVInt(0); // Collected
                    stream.WriteVInt(BrawlerIndex); // Index
                    stream.WriteVInt(0);
                }

                stream.WriteVInt(0);
                stream.WriteVInt(0);
            }
            else { stream.WriteVInt(0); stream.WriteVInt(0); stream.WriteVInt(0); }
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 227;
        }
    }
}
