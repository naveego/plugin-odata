using System;

namespace PluginOData.Helper
{
    public class Settings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseUrl { get; set; }

        /// <summary>
        /// Validates the settings input object
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Validate()
        {
            if (String.IsNullOrEmpty(BaseUrl))
            {
                throw new Exception("the BaseUrl property must be set");
            }
            
            if (!string.IsNullOrWhiteSpace(Username))
            {
                if (String.IsNullOrEmpty(Password))
                {
                    throw new Exception("the Password property must be set");
                }
            }
            
            if (!string.IsNullOrWhiteSpace(Password))
            {
                if (String.IsNullOrEmpty(Username))
                {
                    throw new Exception("the Username property must be set");
                }
            }
        }

        public bool HasAuth()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}