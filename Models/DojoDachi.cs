using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dojodachi.Models
{
    [Serializable]
    public class DojoDachi
    {
        public int Fullness;
        public int Happines;
        public int Meals;
        public int Energy;

        public bool Alive = true;
        public bool Win = false;

        public List<string> History = new List<string>{};
        public DojoDachi()
        {
            Fullness = 20;
            Happines = 20;
            Energy = 50;
            Meals = 3;
            History.Add("Nice to meet you!!!");
        }
    }
}