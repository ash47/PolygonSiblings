using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Common;

namespace Project
{
    class LuaGlobals
    {
        public Vec2 Vec2(float x, float y)
        {
            return new Vec2(x, y);
        }
    }
}
