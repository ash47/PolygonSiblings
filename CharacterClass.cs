using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace Project
{
    public class CharacterClass
    {
        // The name of this character
        public String name;

        // The method to setup the character
        public LuaFunction init;

        // The update function for this character
        public LuaFunction update;
    }
}
