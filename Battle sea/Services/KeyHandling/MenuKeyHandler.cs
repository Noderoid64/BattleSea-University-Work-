using System;
using System.Collections.Generic;
using System.Text;

using Battle_sea.Model;
using Battle_sea.Services.Application;

namespace Battle_sea.Services.KeyHandling
{
    class MenuKeyHandler : KeyHandler
    {
        private FieldArrangeApplicationService fieldArrangeApplicationService;
        private Point cursor = new Point(0,0);
        public MenuKeyHandler (KeyProcessor keyProcessor, FieldArrangeApplicationService fieldArrangeApplicationService) : base (keyProcessor) {
            this.fieldArrangeApplicationService = fieldArrangeApplicationService;
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }
        protected override void handleDown()
        {
            if (cursor.y <= 8)
            {
                cursor.y++;
            }
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }

        protected override void handleLeft()
        {
            if (cursor.x > 0)
            {
                cursor.x--;
            }
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }

        protected override void handleRight()
        {
            if (cursor.x <= 8)
            {
                cursor.x++;
            }
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }

        protected override void handleUp()
        {
            if (cursor.y > 0)
            {
                cursor.y--;
            }
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }

        protected override void handleEnter()
        {
            fieldArrangeApplicationService.placeShip(cursor);
        }

        protected override void handleTab()
        {
            fieldArrangeApplicationService.changeShipType();
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }

        protected override void handleR()
        {
            fieldArrangeApplicationService.changeShipOrientation();
            fieldArrangeApplicationService.changeShipPosition(cursor);
        }
    }
}
