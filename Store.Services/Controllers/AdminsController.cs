using Store.Data;
using Store.Models;
using Store.Services.Attributes;
using Store.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace Store.Services.Controllers
{
    public class AdminsController : BaseApiController
    {
        private const int MinUsernameLength = 6;

        private const int MaxUsernameLength = 30;

        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";

        private const string ValidNicknameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";

        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";

        private const int SessionKeyLength = 50;

        private const int Sha1Length = 40;

        private static readonly Random Rand = new Random();

        public AdminsController()
        {
        }

        [ActionName("register")]
        public HttpResponseMessage PostRegisterAdmin(UserModel model, 
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new StoreContext();

                this.ValidateUsername(model.Username);
                this.ValidateAuthCode(model.AuthCode);
                this.ValidateNickname(model.DisplayName);

                var admin = context.Admins.FirstOrDefault(a => a.SessionKey == sessionKey);
                if (admin == null)
                {
                    throw new InvalidOperationException("Invalid session key");
                }

                var usernameToLower = model.Username.ToLower();
                var displayNameToLower = model.DisplayName.ToLower();
                var existingAdmin = context.Admins.FirstOrDefault(
                    a => a.Username == usernameToLower
                    || a.DisplayName.ToLower() == displayNameToLower);

                if (existingAdmin != null)
                {
                    throw new InvalidOperationException("Username already exists");
                }

                Admin newAdmin = new Admin
                {
                    Username = model.Username,
                    DisplayName = model.DisplayName,
                    AuthCode = model.AuthCode
                };

                context.Admins.Add(newAdmin);
                context.SaveChanges();

                var loggedModel = new LoggedUserModel
                {
                    DisplayName = newAdmin.DisplayName,
                    SessionKey = newAdmin.SessionKey
                };

                var responseMsg = this.Request.CreateResponse(HttpStatusCode.Created, loggedModel);
                return responseMsg;
            });

            return response;
        }

        [ActionName("login")]
        public HttpResponseMessage PostLoginAdmin(UserLoginModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  var context = new StoreContext();
                  using (context)
                  {
                      this.ValidateUsername(model.Username);
                      this.ValidateAuthCode(model.AuthCode);
                      var usernameToLower = model.Username.ToLower();
                      var admin = context.Admins.FirstOrDefault(
                          adm => adm.Username == usernameToLower
                          && adm.AuthCode == model.AuthCode);

                      if (admin == null)
                      {
                          throw new InvalidOperationException("Invalid username or password");
                      }

                      if (admin.SessionKey == null)
                      {
                          admin.SessionKey = this.GenerateSessionKey(admin.AdminId);
                          context.SaveChanges();
                      }

                      var loggedModel = new LoggedUserModel()
                      {
                          DisplayName = admin.DisplayName,
                          SessionKey = admin.SessionKey
                      };

                      var response = this.Request.CreateResponse(HttpStatusCode.Created, loggedModel);
                      return response;
                  }
              });

            return responseMsg;
        }

        [ActionName("logout")]
        public HttpResponseMessage PutLogoutAdmin(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ValidateSessionKey(sessionKey);
                  var context = new StoreContext();
                  using (context)
                  {
                      var admin = context.Admins.FirstOrDefault(a => a.SessionKey == sessionKey);
                      if (admin == null)
                      {
                          throw new ArgumentOutOfRangeException("Invalid session key!");
                      }

                      admin.SessionKey = null;
                      context.SaveChanges();

                      var response =
                          this.Request.CreateResponse(HttpStatusCode.OK);
                      return response;
                  }
              });

            return responseMsg;
        }

        private string GenerateSessionKey(int userId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = Rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }

            return skeyBuilder.ToString();
        }

        private void ValidateSessionKey(string sessionKey)
        {
            if (sessionKey == null || sessionKey.Length != SessionKeyLength)
            {
                throw new ArgumentOutOfRangeException("Invalid session key!");
            }
        }

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null || authCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException("Password should be encrypted");
            }
        }

        private void ValidateNickname(string nickname)
        {
            if (nickname == null)
            {
                throw new ArgumentNullException("Nickname cannot be null");
            }
            else if (nickname.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Nickname must be at least {0} characters long",
                    MinUsernameLength));
            }
            else if (nickname.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Nickname must be less than {0} characters long",
                    MaxUsernameLength));
            }
            else if (nickname.Any(ch => !ValidNicknameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Nickname must contain only Latin letters, digits, ., _,  , -");
            }
        }

        private void ValidateUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            else if (username.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Username must be at least {0} characters long",
                    MinUsernameLength));
            }
            else if (username.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Username must be less than {0} characters long",
                    MaxUsernameLength));
            }
            else if (username.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Username must contain only Latin letters, digits .,_");
            }
        }
    }
}
