using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Entity
{
    public class Catalog
    {
        public string Owner { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }

        public Catalog(string owner, string objectName, string objectType)
        {
            Owner = owner;
            ObjectName = objectName;
            ObjectType = objectType;
        }
    }
}
