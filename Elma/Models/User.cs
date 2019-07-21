using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elma.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Login == user.Login &&
                   Password == user.Password;
        }

        public override int GetHashCode()
        {
            var hashCode = 2079674330;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Login);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }

        public class UserMap : ClassMap<User>
        {
            public UserMap()
            {
                Id(x => x.Id);
                Map(x => x.Login);
                Map(x => x.Password);
            }
        }


    }
}