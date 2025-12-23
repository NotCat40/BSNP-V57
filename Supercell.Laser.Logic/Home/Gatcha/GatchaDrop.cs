namespace Supercell.Laser.Logic.Home.Gatcha
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class GatchaDrop
    {
        public int Count;
        public int DataGlobalId;
        public int SkinGlobalId;
        public int CardGlobalId;
        public int VanityGlobalId;
        public int Type;

        public bool IsExecuted;

        public GatchaDrop(int type)
        {
            Type = type;
        }

        public void DoDrop(HomeMode homeMode)
        {
            ClientAvatar avatar = homeMode.Avatar;

            switch (Type)
            {
                case 1:
                    CharacterData characterData = DataTables.Get(16).GetDataByGlobalId<CharacterData>(DataGlobalId);
                    if (characterData == null) return;
                    CardData cardData = DataTables.Get(23).GetData<CardData>(characterData.Name + "_unlock");
                    if (cardData == null) return;
                    avatar.UnlockHero(characterData.GetGlobalId(), cardData.GetGlobalId());
                    Hero hero = avatar.GetHero(DataGlobalId);
                    hero.PowerLevel = Count - 1;
                    break;
                case 2:
                    break;
                case 4:
                    // Item
                    Console.WriteLine($"{CardGlobalId}");
                    break;
                case 6:
                    break;
                case 7:
                    avatar.AddHeroLvlUpMaterial(Count);
                    break;
                case 8:
                    avatar.AddDiamonds(Count);
                    break;
                case 9:
                    // Skin
                    homeMode.Home.UnlockedSkins.Add(SkinGlobalId);
                    break;
                case 10:
                    break;
                case 11:
                    Console.WriteLine($"{VanityGlobalId}");
                    string idStr = VanityGlobalId.ToString();
                    if (idStr.StartsWith("28"))
                    {
                        if (!homeMode.Home.UnlockedThumbnails.Contains(VanityGlobalId))
                        {
                            homeMode.Home.UnlockedThumbnails.Add(VanityGlobalId);
                            Console.WriteLine("UnlockedThumbnails: " + string.Join(", ", homeMode.Home.UnlockedThumbnails));
                        }
                    }
                    else if (idStr.StartsWith("52"))
                    {
                        if (!homeMode.Home.UnlockedEmotes.Contains(VanityGlobalId))
                        {
                            homeMode.Home.UnlockedEmotes.Add(VanityGlobalId);
                            Console.WriteLine("UnlockedEmotes: " + string.Join(", ", homeMode.Home.UnlockedEmotes));
                        }
                    }
                    else if (idStr.StartsWith("68"))
                    {
                        if (!homeMode.Home.UnlockedSprays.Contains(VanityGlobalId))
                        {
                            homeMode.Home.UnlockedSprays.Add(VanityGlobalId);
                            Console.WriteLine("UnlockedSprays: " + string.Join(", ", homeMode.Home.UnlockedSprays));
                        }
                    }
                    else if (idStr.StartsWith("76"))
                    {
                        if (!homeMode.Home.UnlockedTituls.Contains(VanityGlobalId))
                        {
                            homeMode.Home.UnlockedTituls.Add(VanityGlobalId);
                            Console.WriteLine("UnlockedTituls: " + string.Join(", ", homeMode.Home.UnlockedTituls));
                        }
                    }
                    else if (idStr.StartsWith("85"))
                    {
                        if (!homeMode.Home.UnlockedFrames.Contains(VanityGlobalId))
                        {
                            homeMode.Home.UnlockedFrames.Add(VanityGlobalId);
                            Console.WriteLine("UnlockedFrames: " + string.Join(", ", homeMode.Home.UnlockedFrames));
                        }
                    }
                    break;

                case 12:
                    avatar.AddLegendaryTrophies(Count);
                    break;
                case 22:
                    avatar.AddRecruitTokens(Count);
                    break;
                case 24:
                    avatar.AddPowerPoints(Count);
                    break;
                case 25:
                    avatar.AddBling(Count);
                    break;
                case 31:
                    homeMode.Home.HasPremiumPassPlus = true;
                    homeMode.Home.HasPremiumPass = true;
                    homeMode.Home.BrawlPassTokens += 8000;
                    break;
                case 34:
                    avatar.AddCollabPoints(Count);
                    break;
            }
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(Count);
            ByteStreamHelper.WriteDataReference(stream, DataGlobalId);
            stream.WriteVInt(Type);
            ByteStreamHelper.WriteDataReference(stream, SkinGlobalId);
            ByteStreamHelper.WriteDataReference(stream, VanityGlobalId);
            ByteStreamHelper.WriteDataReference(stream, CardGlobalId);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
        }
    }
}
