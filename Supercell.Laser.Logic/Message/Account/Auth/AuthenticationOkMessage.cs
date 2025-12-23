namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class AuthenticationOkMessage : GameMessage
    {
        public long AccountId;
        public string PassToken;
        public string ServerEnvironment;

        public AuthenticationOkMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Stream.WriteLong(AccountId);
            Stream.WriteLong(AccountId);
            Stream.WriteString(PassToken);
            Stream.WriteString("");  // FacebookID
            Stream.WriteString("");  // GamecenterID
            Stream.WriteInt(57);
            Stream.WriteInt(325);
            Stream.WriteInt(1);
            Stream.WriteString("prod");
            Stream.WriteInt(0);  // SessionCount
            Stream.WriteInt(0);  // PlayTimeSeconds
            Stream.WriteInt(0);  // DaysSinceStartedPlaying
            Stream.WriteString("");  // FacebookAppID
            Stream.WriteString("");  // ServerTime
            Stream.WriteString("");  // AccountCreatedDate
            Stream.WriteInt(0);  // StartupCooldownSeconds
            Stream.WriteString("");  // GoogleServiceID
            Stream.WriteString("EN");
            Stream.WriteString("");  // KunlunID
            Stream.WriteInt(0);  // Tier
            Stream.WriteString("");  // TencentID
            Stream.WriteInt(2);
            {
                Stream.WriteString("https://game-assets.brawlstarsgame.com");
                Stream.WriteString("http://a678dbc1c015a893c9fd-4e8cc3b1ad3a3c940c504815caefa967.r87.cf2.rackcdn.com");
            }
            Stream.WriteInt(3);
            {
                Stream.WriteString("https://event-assets-2.brawlstars.com");
                Stream.WriteString("https://event-assets.brawlstars.com");
                Stream.WriteString("https://24b999e6da07674e22b0-8209975788a0f2469e68e84405ae4fcf.ssl.cf2.rackcdn.com/event-assets");
            }
            Stream.WriteVInt(0);  // SecondsUntilAccountDeletion
            Stream.WriteCompressedString("");  // SupercellIDToken
            Stream.WriteBoolean(false);   // IsSupercellIDLogoutAllDevicesAllowed
            Stream.WriteBoolean(true);  // isSupercellIDEligible
            Stream.WriteString("");  // LineID
            Stream.WriteString("");  // SessionID
            Stream.WriteString("");  // KakaoID
            Stream.WriteString("");  // UpdateURL
            Stream.WriteString("");  // YoozooPayNotifyUrl
            Stream.WriteBoolean(true);  // UnbotifyEnabled
            Stream.WriteBoolean(true);
            Stream.WriteBoolean(true);
            Stream.WriteBoolean(true);
            Stream.WriteBoolean(true);
            Console.WriteLine("[DEBUG] [LoginOkMessage::Encode] Message created!");
        }

        public override int GetMessageType()
        {
            return 20104;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
