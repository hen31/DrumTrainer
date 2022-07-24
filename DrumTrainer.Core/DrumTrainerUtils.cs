using System;
using System.Collections.Generic;
using System.Text;

namespace DrumTrainer.Core
{
    public static class DrumTrainerUtils
    {
        public static string GetStartPath()
        {
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            return string.Concat(System.IO.Path.GetDirectoryName(fullPath).Remove(0, 6), @"\");
        }
    }
}
