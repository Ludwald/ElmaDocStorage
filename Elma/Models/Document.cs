using Elma.Models.utils;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elma.Models
{
    public class Document
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual string DataType { get; set; }
        public virtual string GetLimitedName {
            get
            {
                return Name.GetLimitedString(30);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Document document &&
                   Id == document.Id &&
                   EqualityComparer<User>.Default.Equals(User, document.User) &&
                   Name == document.Name &&
                   DateTime == document.DateTime &&
                   DataType == document.DataType;
        }

        public override int GetHashCode()
        {
            var hashCode = 419885285;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(User);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + DateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataType);
            return hashCode;
        }

        public class DocumentMap : ClassMap<Document>
        {
            public DocumentMap()
            {
                Id(x => x.Id);
                References(x => x.User);
                Map(x => x.Name);
                Map(x => x.DateTime);
                Map(x => x.DataType);
            }
        }

    }
}