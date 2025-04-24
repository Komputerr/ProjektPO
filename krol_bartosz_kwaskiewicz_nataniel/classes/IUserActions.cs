using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public interface IUserActions
    {
        void AddFacebookPost(string post, User user);
        void AddInstagramPost(string post, User user);
        void AddRedditPost(string post, User user);
        void EditPost(User user);
        void DeletePostAllAdmin(User user);
        void DeletePostAllOwn(User user);
        void DeletePostOneAdmin(User user);
        void DeletePostOneOwn(User user);
    }
}
