using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Net.Mail;
using Klmsncamp.Models;
using Klmsncamp.DAL;

public class MyRoleProvider : RoleProvider
{

    UserRepository _repository = new UserRepository();

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override string ApplicationName
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public override void CreateRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        throw new NotImplementedException();
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        throw new NotImplementedException();
    }

    public override string[] GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public override string[] GetRolesForUser(string username)
    {
        UserRepository _repository = new UserRepository();
        IList<Role> roles_ =  _repository.GetRoles(username);

        if (roles_.Count() > 0)
        {

            string[] roles = new string[roles_.Count];

            int idx = 0;
            foreach (Role rl in roles_)
            {
                roles[idx] = rl.Description;
                idx++;
            }
            return roles;
        }
        else
        {
            //eğer rol atanmamışsa "users" grubuna al
            string[] roles = new string[1];
            roles[0] = "users";
            return roles;
        }
    }

    public override string[] GetUsersInRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override bool IsUserInRole(string username, string roleName)
    {
        UserRepository _repository = new UserRepository();
        MembershipUser user = _repository.GetUser(username);

        if (user != null)
            return _repository.isInRole(int.Parse((user.ProviderUserKey).ToString()),roleName);
        else
            return false;

    }

    public bool HasPerm(string username, string customperm)
    {
        UserRepository _repository = new UserRepository();
        MembershipUser user = _repository.GetUser(username);
        if (user != null)
        {
            return _repository.HasPerm(int.Parse((user.ProviderUserKey).ToString()), customperm);
        }
        else
        {
            return false;
        }
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override bool RoleExists(string roleName)
    {
        throw new NotImplementedException();
    }
}

    