using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Services.Application;
using Battle_sea.Model;

namespace Battle_sea.Services.KeyHandling
{
    class GameKeyHandler : KeyHandler
    {
        private GameProcessApplicationService applicationService;
        private Point cursor = new Point(0,0);
        public GameKeyHandler(KeyProcessor keyProcessor, GameProcessApplicationService applicationService) : base( keyProcessor)
        {
            this.applicationService = applicationService;
        }
        protected override void handleDown()
        {
            if (cursor.y <= 8)
            {
                cursor.y++;
            }
            applicationService.changeTargetPosition(cursor);
        }

        protected override void handleEnter()
        {
            applicationService.makeShot(cursor);
        }

        protected override void handleLeft()
        {
            if (cursor.x > 0)
            {
                cursor.x--;
            }
            applicationService.changeTargetPosition(cursor);
        }

        protected override void handleR()
        {
            throw new NotImplementedException();
        }

        protected override void handleRight()
        {
            if (cursor.x <= 8)
            {
                cursor.x++;
            }
            applicationService.changeTargetPosition(cursor);
        }

        protected override void handleTab()
        {
            throw new NotImplementedException();
        }

        protected override void handleUp()
        {
            if (cursor.y > 0)
            {
                cursor.y--;
            }
            applicationService.changeTargetPosition(cursor);
        }
    }
}
