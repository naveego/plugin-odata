using System;
using PluginOData.Helper;
using Xunit;

namespace PluginODataTest.Helper
{
    public class SettingsTest
    {
        [Fact]
        public void ValidateValidNoAuthTest()
        {
            // setup
            var settings = new Settings
            {
                BaseUrl = "odatafeed",
                Username = "",
                Password = ""
            };

            // act
            settings.Validate();

            // assert
        }
        
        [Fact]
        public void ValidateValidAuthTest()
        {
            // setup
            var settings = new Settings
            {
                BaseUrl = "odatafeed",
                Username = "user",
                Password = "pass"
            };

            // act
            settings.Validate();

            // assert
        }

        [Fact]
        public void ValidateNoBaseUrlTest()
        {
            // setup
            var settings = new Settings
            {
                BaseUrl = null,
                Username = "",
                Password = ""
            };

            // act
            Exception e = Assert.Throws<Exception>(() => settings.Validate());

            // assert
            Assert.Contains("The BaseUrl property must be set", e.Message);
        }
        
        [Fact]
        public void ValidateNoUserTest()
        {
            // setup
            var settings = new Settings
            {
                BaseUrl = "odatafeed",
                Username = null,
                Password = "password"
            };

            // act
            Exception e = Assert.Throws<Exception>(() => settings.Validate());

            // assert
            Assert.Contains("The Username property must be set", e.Message);
        }
        
        [Fact]
        public void ValidateNoPasswordTest()
        {
            // setup
            var settings = new Settings
            {
                BaseUrl = "odatafeed",
                Username = "user",
                Password = null
            };

            // act
            Exception e = Assert.Throws<Exception>(() => settings.Validate());

            // assert
            Assert.Contains("The Password property must be set", e.Message);
        }
    }
}