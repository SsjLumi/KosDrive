//using dotAPNS;
//using Newtonsoft.Json;

//namespace KosDrive.Services
//{
//    public class ApplePushNotificationService
//    {
//        private readonly string _keyId = "MY_KEY_ID";
//        private readonly string _teamId = "MY_TEAM_ID";
//        private readonly string _bundleId = "com.my.app";
//        private readonly string _authKeyPath = "path/to/AuthKey.p8";

//        public async Task<OperationResult<string>> SendPushAsync(string deviceToken, string title, string body)
//        {
//            try
//            {
//                var authKey = File.ReadAllText(_authKeyPath);
//                var apns = new ApnsJwtOptions
//                {
//                    BundleID = _bundleId,
//                    KeyId = _keyId,
//                    TeamId = _teamId,
//                    AuthKey = authKey,
//                };

//                var push = new ApnsHttp2Handler(apns);

//                var payload = new
//                {
//                    aps = new
//                    {
//                        alert = new
//                        {
//                            title = title,
//                            body = body
//                        },
//                        sound = "default"s
//                    }
//                };

//                var jsonPayload = JsonConvert.SerializeObject(payload);

//                var result = await push.SendAsync(deviceToken, jsonPayload);

//                if (result.IsSuccessful)
//                    return OperationResult<string>.Success("Notification sent successfully");
//                else
//                    return OperationResult<string>.Failure("Notification failed: " + result.Reason);
//            }catch(Exception ex)
//            {
//                return OperationResult<string>.Failure("Exception: " + ex.Message);
//            }
//        }
//    }
//}
