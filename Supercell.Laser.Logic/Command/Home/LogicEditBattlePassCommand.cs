namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicEditBattlePassCommand : Command
    {
        public int CharacterId;
        public int VanityId;
        public bool unk1;
        public int Index;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            CharacterId=ByteStreamHelper.ReadDataReference(stream);
            VanityId=ByteStreamHelper.ReadDataReference(stream);
            unk1 = stream.ReadBoolean();
            Index = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            switch (Index)
            {
                case 0:
                    homeMode.Home.DefaultBattleCard.Thumbnail1 = VanityId;
                    break;
                case 1:
                    homeMode.Home.DefaultBattleCard.Thumbnail2 = VanityId;
                    break;
                case 5:
                    EmoteData emoteData = DataTables.Get(DataType.Emote).GetDataByGlobalId<EmoteData>(VanityId); 
                    Console.WriteLine("EMOJE " + homeMode.Home.UnlockedEmotes.Contains(VanityId));
                    if (emoteData.BattleCategory == "DEFAULT" && emoteData.Rarity == "DEFAULT" && emoteData.EmoteType == "DEFAULT" || homeMode.Home.UnlockedEmotes.Contains(VanityId))
                    {

                        if (emoteData.Character != null)
                        {
                            CharacterData characterData = DataTables.Get(DataType.Character).GetData<CharacterData>(emoteData.Character);
                            if (homeMode.Avatar.HasHero(characterData.GetGlobalId())) homeMode.Home.DefaultBattleCard.Emote = VanityId;
                        }
                        else
                        {
                            homeMode.Home.DefaultBattleCard.Emote = VanityId;
                        }
                    }
                    else
                    {
                        return -2;
                    }
                    break;
                case 10:
                    if (!homeMode.Home.UnlockedTituls.Contains(VanityId)) return -1;
                    homeMode.Home.DefaultBattleCard.Title = VanityId;
                    break;    
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 568;
        }
    }
}
