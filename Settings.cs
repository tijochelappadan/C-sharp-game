using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWindows.Snake
{
    class Settings
    {
        public static int iWidth { get; set; }
        public static int iHeight { get; set; }

        public static string strDirections;

        public Settings()
        {
            iWidth = 16;
            iHeight = 16;
            strDirections = "left";
        }
    }
}
