//using System;
//using System.Threading.Tasks;
//using Microsoft.Owin;
//using Owin;

//[assembly: OwinStartup(typeof(MJL_DX20.App_Start.AuthConfig))]

//namespace MJL_DX20.App_Start
//{
//    public class AuthConfig
//    {
//        public static void RegisterAuth()
//        {
//            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
//            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

//            //OAuthWebSecurity.RegisterMicrosoftClient(
//            //    clientId: "",
//            //    clientSecret: "");

//            //OAuthWebSecurity.RegisterTwitterClient(
//            //    consumerKey: "",
//            //    consumerSecret: "");

//            //OAuthWebSecurity.RegisterFacebookClient(
//            //    appId: "",
//            //    appSecret: "");

//            //OAuthWebSecurity.RegisterGoogleClient();
//        }
//    }
//}



namespace MJL_DX20
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
