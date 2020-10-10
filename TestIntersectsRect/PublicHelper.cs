using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper
{
    static class PublicHelper
    {
        private static GraphicsDevice graphicsDevice;
        public static GraphicsDevice GraphicsDevice { get => graphicsDevice; set => graphicsDevice = value; }
    }
}
