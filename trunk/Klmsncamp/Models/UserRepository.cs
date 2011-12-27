using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Klmsncamp.Models
{
    public class UserRepository
    {
        public MembershipUser CreateUser(string username, string firstname, string lastname, string password, string email)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                User user = new User();

                user.UserName = username;
                user.FirstName = firstname;
                user.LastName = lastname;
                user.Email = email;
                user.PasswordSalt = CreateSalt();
                user.Password = CreatePasswordHash(password, user.PasswordSalt);
                user.CreatedDate = DateTime.Now;
                user.IsActivated = false;
                user.IsLockedOut = false;
                user.LastLockedOutDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();

                var moderator_role = db.Roles.Find(2);
                moderator_role.Users.Add(user);
                db.SaveChanges();

                return GetUser(username);
            }
        }

        public User GetCustomUser(string username)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser;
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<Role> GetRoles(string username)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.Roles.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetUserNameByEmail(string email)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var result = from u in db.Users where (u.Email == email) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.UserName;
                }
                else
                {
                    return "";
                }
            }
        }

        public MembershipUser GetUser(string username)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    string _username = dbuser.UserName;
                    int _providerUserKey = dbuser.UserId;
                    string _email = dbuser.Email;
                    string _passwordQuestion = "";
                    string _comment = dbuser.Comments;
                    bool _isApproved = dbuser.IsActivated;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreatedDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate.GetValueOrDefault(DateTime.Now);
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = dbuser.LastLockedOutDate.GetValueOrDefault(DateTime.Now);

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate
                                                              );

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool isInRole(int userID, string roleName)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var user_ = db.Users.Find(userID);

                var user_roles = user_.Roles.ToList();
                db.Dispose();
                foreach (Role rl in user_roles)
                {
                    if (rl.Description.ToLower() == roleName.ToLower())
                        return true;
                }
                return false;
            }
        }

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPwd, "sha1");
            return hashedPwd;
        }

        public bool ValidateUser(string username, string password)
        {
            using (KlmsnContext db = new KlmsnContext())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}