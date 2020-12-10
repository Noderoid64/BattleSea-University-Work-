using System;
using System.Collections.Generic;
using System.Text;

namespace Battle_sea.Services.KeyHandling
{
    class KeyProcessor
    {
        public event Action UpKeyPressed;
        public event Action RightKeyPressed;
        public event Action DownKeyPressed;
        public event Action LeftKeyPressed;
        public event Action EnterKeyPressed;
        public event Action TabKeyPressed;
        public event Action RKeyPressed;
        public void waitForKey()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            switch(key.Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        this.UpKeyPressed?.Invoke();
                    } break ;
                case ConsoleKey.RightArrow:
                    {
                        this.RightKeyPressed?.Invoke();
                    }
                    break;
                case ConsoleKey.DownArrow:
                    {
                        this.DownKeyPressed?.Invoke();
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    {
                        this.LeftKeyPressed?.Invoke();
                    }
                    break;
                case ConsoleKey.Enter:
                    {
                        EnterKeyPressed?.Invoke();
                    }
                    break;
                case ConsoleKey.Tab:
                    {
                        TabKeyPressed?.Invoke();
                    }
                    break;
                case ConsoleKey.R:
                    {
                        RKeyPressed?.Invoke();
                    }
                    break;
                default:
                    {
                        waitForKey();
                    } break;
            }
        }
    }
}
