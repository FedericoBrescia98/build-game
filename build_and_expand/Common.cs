using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Configuration;
using System.IO;

namespace build_and_expand
{
    public static class C
    {
        public static Point DISPLAYDIM = new Point(960, 960);
        public static Point TILETEXTURESIZE = new Point(32, 32);
        public static int MAXEDGEMAP = 20;
        public static Point MAPDIM = new Point(32, 32); // needs to edit the map file if edited 
    }
}
