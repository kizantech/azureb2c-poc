namespace AzureB2C.AuthApi.Utils
{
    public class InviteCodeGenerator
    {
        public static string GenerateInviteCode(int length = 10)
        {
            const string charspace = "BCDFGHJKMPQRTVWXY2346789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(charspace, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
