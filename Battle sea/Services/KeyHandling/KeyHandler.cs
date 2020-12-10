using System;
using System.Collections.Generic;
using System.Text;

namespace Battle_sea.Services.KeyHandling
{
    abstract class KeyHandler
    {
        public KeyHandler (KeyProcessor keyProcessor)
        {
            keyProcessor.UpKeyPressed += handleUp;
            keyProcessor.RightKeyPressed += handleRight;
            keyProcessor.DownKeyPressed += handleDown;
            keyProcessor.LeftKeyPressed += handleLeft;
            keyProcessor.EnterKeyPressed += handleEnter;
            keyProcessor.TabKeyPressed += handleTab;
            keyProcessor.RKeyPressed += handleR;
        }

        protected abstract void handleUp();
        protected abstract void handleRight();
        protected abstract void handleDown();
        protected abstract void handleLeft();
        protected abstract void handleEnter();
        protected abstract void handleTab();
        protected abstract void handleR();
    }
}
