using System;
using System.Collections.Generic;
using System.Text;

namespace DrumTrainer.Core
{
    public interface IUIContext
    {
        void BeginInvoke(Action action);

        float GetCurrentElapsedTimeSinceStart();
    }
}
