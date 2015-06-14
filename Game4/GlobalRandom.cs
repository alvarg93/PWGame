using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class GlobalRandom
    {
        private static GlobalRandom instance;

        public static GlobalRandom Instance
        {
            get
            {
                if (instance == null) instance = new GlobalRandom(); 
                return GlobalRandom.instance; 
            }
            set { GlobalRandom.instance = value; }
        }
        Random generator;

        public void Init()
        {
            generator = new Random();
        }

        public int Next()
        {
            return generator.Next();
        }
        public int Next(int min, int count)
        {
            return generator.Next(min,count);
        }
    }
}
