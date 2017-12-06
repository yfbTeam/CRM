using DDHelper.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DDHelper
{
    public static class Config
    {
        //企业CorpID
        private static string _ECorpId;

        //企业CorpSecret
        private static string _ECorpSecret;

        //具体应用的appId
        private static string _EAgentID;


        //当前网站的weburl
        private static string _webUrl;


        //isv参数
        private static string _SUITE_KEY;



        private static string _SUITE_KEY_SECRET;


        private static string _ENCODING_AES_KEY;


        //private static string _Token;

        private static TokenModel _TokenModel;

        private static string _ServerUri;

        private static string _ServerUri_PC;

        public static string ECorpId
        {
            get { return Config._ECorpId; }
            set { Config._ECorpId = value; }
        }

        public static string ECorpSecret
        {
            get { return Config._ECorpSecret; }
            set { Config._ECorpSecret = value; }
        }

       /// <summary>
       /// 微应用ID
       /// </summary>
        public static string EAgentID
        {
            get { return Config._EAgentID; }
            set { Config._EAgentID = value; }
        }

       /// <summary>
       /// 网页地址
       /// </summary>
        public static string WebUrl
        {
            get { return Config._webUrl; }
            set { Config._webUrl = value; }
        }

        public static string SUITE_KEY
        {
            get { return Config._SUITE_KEY; }
            set { Config._SUITE_KEY = value; }
        }

        public static string SUITE_KEY_SECRET
        {
            get { return Config._SUITE_KEY_SECRET; }
            set { Config._SUITE_KEY_SECRET = value; }
        }

        //public static string Token
        //{
        //    get { return Config._Token; }
        //    set { Config._Token = value; }
        //}

        public static TokenModel TokenModel
        {
            get { return Config._TokenModel; }
            set { Config._TokenModel = value; }
        }


        public static string ENCODING_AES_KEY
        {
            get { return Config._ENCODING_AES_KEY; }
            set { Config._ENCODING_AES_KEY = value; }
        }

        public static string ServerUri
        {
            get { return Config._ServerUri; }
            set { Config._ServerUri = value; }
        }

        public static string ServerUri_PC
        {
            get { return Config._ServerUri_PC; }
            set { Config._ServerUri_PC = value; }
        }
        //static Config()
        //{
        //    _ECorpId = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpId];
        //    _ECorpSecret = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpSecret];
        //    _EAgentID = ConfigurationManager.AppSettings[ConfigurationKeys.EAgentID];
        //    _webUrl = ConfigurationManager.AppSettings[ConfigurationKeys.WebUrl];

        //    _SUITE_KEY = ConfigurationManager.AppSettings[ConfigurationKeys.SUITE_KEY];
        //    _SUITE_KEY_SECRET = ConfigurationManager.AppSettings[ConfigurationKeys.SUITE_KEY_SECRET];
        //    _Token = ConfigurationManager.AppSettings[ConfigurationKeys.Token];
        //    _ENCODING_AES_KEY = ConfigurationManager.AppSettings[ConfigurationKeys.ENCODING_AES_KEY];

        //    _ServerUri = ConfigurationManager.AppSettings[ConfigurationKeys.ServerUri];
        //}


        static Config()
        {
            _ECorpId = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpId];
            _ECorpSecret = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpSecret];
            _EAgentID = ConfigurationManager.AppSettings[ConfigurationKeys.EAgentID];
            _webUrl = ConfigurationManager.AppSettings[ConfigurationKeys.WebUrl];
            _ServerUri = ConfigurationManager.AppSettings[ConfigurationKeys.ServerUri];
            _ServerUri_PC = ConfigurationManager.AppSettings[ConfigurationKeys.ServerUri_PC];

            _SUITE_KEY = "suitex3iapsh4ezpbj4wg";
            _SUITE_KEY_SECRET = "dDaUdBGQX6RLaVbKZi4qQ8AV__4JrpPH_8UXTWHIysK7nl6FrS6lH3RP_En0VwAm";
            //_Token = "hwxxkj";
            _ENCODING_AES_KEY = "t6bidxwi58u2ln5ssosyh9wam109b77kl35h4gl8ppe";            
        }



        private static class ConfigurationKeys
        {
            public const string ECorpId = "E.CorpId";
            public const string ECorpSecret = "E.CorpSecret";
            public const string EAgentID = "E.AgentID";
            public const string WebUrl = "webUrl";


            public const string SUITE_KEY = "SUITE_KEY";
            public const string SUITE_KEY_SECRET = "SUITE_KEY_SECRET";
            public const string Token = "Token";
            public const string ENCODING_AES_KEY = "ENCODING_AES_KEY";
            public const string ServerUri = "ServerUri";

            public const string ServerUri_PC = "ServerUri_PC";
        }
    }
}
