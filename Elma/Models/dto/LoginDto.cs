using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elma.Models
{
    public class LoginDto
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual bool RememberMe { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LoginDto dto &&
                   UserName == dto.UserName &&
                   Password == dto.Password &&
                   RememberMe == dto.RememberMe;
        }

        public override int GetHashCode()
        {
            var hashCode = 435790889;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = hashCode * -1521134295 + RememberMe.GetHashCode();
            return hashCode;
        }
    }
}