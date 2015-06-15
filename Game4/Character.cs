using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class Character : Entity
    {
        int attackModifier = 0;

        public int AttackModifier
        {
            get { return attackModifier; }
            set { attackModifier = value; }
        }

    }
}
