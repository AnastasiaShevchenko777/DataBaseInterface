using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTest
{
    class WaterObject
    {
        public string nwo { get; set; }
        public int id { get; set; }

        public string GetName()
        {
            return $"{ nwo }";
        }
        public int GetID() 
        {
            return id;
        }
    }
}
