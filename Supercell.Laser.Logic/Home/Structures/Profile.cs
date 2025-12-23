namespace Supercell.Laser.Logic.Home.Structures
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class Profile
    {
        public PlayerDisplayData DisplayData;
        public long AccountId;
        public Hero[] Heroes;
        public List<LogicVector2> Stats;
        public int Hero;
        public int HeroSkin;
        public int T1;
        public int T2;
        public int E;
        public int Ti;
        public Profile()
        {
            DisplayData = new PlayerDisplayData();
            Stats = new List<LogicVector2>();
        }

        public void AddStat(int key, int value)
        {
            Stats.Add(new LogicVector2(key, value));
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVLong(AccountId);
            ByteStreamHelper.WriteDataReference(stream,Hero);

            stream.WriteVInt(Heroes.Length);
            foreach (Hero hero in Heroes)
            {
                hero.Encode(stream);
            }

            stream.WriteVInt(Stats.Count);
            foreach (LogicVector2 stat in Stats)
            {
                stat.Encode(stream);
            }

            DisplayData.Encode(stream);

            stream.WriteBoolean(false);

            stream.WriteString("str");
            stream.WriteVInt(100);
            stream.WriteVInt(200);
            stream.WriteVInt(0);//v53
            ByteStreamHelper.WriteDataReference(stream, HeroSkin);
            ByteStreamHelper.WriteDataReference(stream, T1);
            ByteStreamHelper.WriteDataReference(stream, T2);
            ByteStreamHelper.WriteDataReference(stream, E);
            ByteStreamHelper.WriteDataReference(stream, Ti);
        }

        public static Profile Create(ClientHome home, ClientAvatar avatar)
        {
            Profile profile = new Profile();

            profile.AccountId = avatar.AccountId;
            
            PlayerDisplayData playerDisplayData = new PlayerDisplayData();
            playerDisplayData.ThumbnailId = home.ThumbnailId;
            playerDisplayData.NameColorId = home.NameColorId;
            playerDisplayData.Name = avatar.Name;
            profile.DisplayData = playerDisplayData;

            profile.Heroes = avatar.Heroes.ToArray();

            profile.AddStat(1, avatar.TrioWins);
            profile.AddStat(2, 100); // Experience
            profile.AddStat(3, avatar.Trophies);
            profile.AddStat(4, avatar.HighestTrophies);
            profile.AddStat(5, profile.Heroes.Length);
            profile.AddStat(7, home.ThumbnailId);
            profile.AddStat(8, avatar.SoloWins);
            profile.AddStat(20, 1); // fame
            profile.AddStat(18, 19);
            profile.AddStat(17, 19);
            /*
             * 11 - DUO WINs
             * 9 highest level roborumble
             * 12 - lvl boss fight
             * 13 - power league points
             * 14 - power league ?
             * 15 - most challenge win
             * 16 level city ramage
             * 18 - solo rank
             * 17 - team rank
             * 19 - club rank
             * 20 - fame
            */

            profile.Hero = home.FavouriteCharacter;
            profile.HeroSkin = avatar.GetHero(home.FavouriteCharacter).SelectedSkinId + 29000000;
            profile.T1 = home.DefaultBattleCard.Thumbnail1;
            profile.T2 = home.DefaultBattleCard.Thumbnail2;
            profile.E = home.DefaultBattleCard.Emote;
            profile.Ti = home.DefaultBattleCard.Title;
            return profile;
        }
    }
}
