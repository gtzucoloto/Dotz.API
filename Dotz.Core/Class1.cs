using System;

namespace Dotz.Core
{
    public class Category
    {
        public string Name { get; set; }
        public Category SuperCategory { get; set; }
    }
}
