using System;
using System.Collections.Generic;
using System.Text;

namespace RequestAPI
{
    public class Authentication
    {
        public string ClientID { get; }
        public string ClientSecret { get; }
        public Authentication(string ClientID, string ClientSecret)
        {
            this.ClientID = ClientID;
            this.ClientSecret = ClientSecret;
        }
        public override string ToString()
        {
            return $"{ClientID}:{ClientSecret}";
        }
    }
}
