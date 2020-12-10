using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Battle_sea.Services;
using Battle_sea.Services.KeyHandling;
using Battle_sea.Services.Application;
using Battle_sea.Services.FileDataGateway;
using Battle_sea.Drivers.FieldDrawer;
using Battle_sea.Model;


namespace Battle_sea
{
    class Program
    {
        static ApplicationStorage storage = ApplicationStorage.getInstance();
        static MenuApplicationService menuApplicationService = new MenuApplicationService(storage);
        static GameProcessApplicationService game;
        static KeyProcessor keyProcessor = new KeyProcessor();
        static FieldDrawer fieldDrawer = new FieldDrawer();
        static FileRoomProvider roomProvider = FileRoomProvider.getInstance();
        static KeyHandler keyHandler;
        static void Main(string[] args)
        {
            askPlayerForName();
        }

        static void askPlayerForName()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the player name");
            Console.ForegroundColor = ConsoleColor.Gray;
            //String name = Console.ReadLine();
            //if (name != null && name.Length > 3 && name.Length < 20)
            //{
            //    menuApplicationService.setPlayerName(name);
            //    askPlayerForMode();
            //} else
            //{
            //    askPlayerForName();
            //}

            storage.setPlayerName("Noder");
            askPlayerForMode();
        }

        static void askPlayerForMode()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Please select options:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("1. Create new game");
            Console.WriteLine("2. Connect to existing");
            String option = Console.ReadLine();
            //String option = "1";
            int mode;
            if (int.TryParse(option, out mode))
            {
                if (mode == 1)
                {
                    showCreateNewGameScreen();
                }
                else if (mode == 2)
                {
                    showConnectToGameScreen();
                }
                else
                {
                    Console.WriteLine("Ups... Please select one of the options");
                    Console.ReadKey();
                    askPlayerForMode();
                }
            }
        }

        static void showCreateNewGameScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select the room name");
            Console.ForegroundColor = ConsoleColor.Gray;
            //String roomName = Console.ReadLine();
            String roomName = "TestRoom";
            bool isRoomInited = menuApplicationService.initNewRoom(roomName);

            // Temporary
            //storage.setCurrentRoomName(roomName);
            //showWaitingForOtherPlayerScreen(true);

            if (isRoomInited)
            {
                subscribeToFieldArrange(true);
                renderFieldMenu(roomProvider.getRoomByName(roomName).firstPlayer.myField);
            }
        }

        static void showConnectToGameScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select the game");
            Console.ForegroundColor = ConsoleColor.Gray;
            IDictionary<int, string> roomNames = roomProvider.getAllRoomNamesAsDictionary();
            foreach (KeyValuePair<int, string> pair in roomNames)
            {
                Console.WriteLine(pair.Key + ". " + pair.Value);
            }
            String roomOptions = Console.ReadLine();
            bool isConnected = menuApplicationService.connectToRoom(roomNames[int.Parse(roomOptions)]);
            if (isConnected)
            {
                if (menuApplicationService.isGameStarted(roomNames[int.Parse(roomOptions)])) {
                    subscribeToFieldGame(false);
                } else
                {
                    subscribeToFieldArrange(false);
                    //if (menuApplicationService.isGameStarted(roomNames[int.Parse(roomOptions)]))
                    //    renderFieldMenu(roomProvider.getRoomByName(roomNames[int.Parse(roomOptions)]).secondPlayer.myField);
                }
               
            }
        }

        static void showWaitingForOtherPlayerScreen(bool isFirstPlayer)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Waiting for the other player...");
            storage.isWaitingForOtherPlayer = true;
            while (storage.isWaitingForOtherPlayer)
            {
                Field field = menuApplicationService.getWaitingInfo(isFirstPlayer);
                fieldDrawer.drawShips(field, false);
                Thread.Sleep(500);
            }
            subscribeToFieldGame(isFirstPlayer);
        }

        private static void subscribeToFieldArrange(bool isFirstPlayer = false)
        {
            FieldArrangeApplicationService fieldArrange = new FieldArrangeApplicationService(isFirstPlayer);
            Action allShipsPlacedHandler = null;
            allShipsPlacedHandler = () =>
            {
                fieldArrange.fieldReadyToRender -= renderFieldMenu;
                fieldArrange.allShipsPlaced -= allShipsPlacedHandler;
                keyHandler = null;
                fieldArrange = null;
                showWaitingForOtherPlayerScreen(isFirstPlayer);
            };

            fieldArrange.fieldReadyToRender += renderFieldMenu;
            fieldArrange.allShipsPlaced += allShipsPlacedHandler;

            keyHandler = new MenuKeyHandler(keyProcessor, fieldArrange);

            if (fieldArrange != null)
            fieldArrange.changeShipPosition(new Point(0, 0));
        }

        static void renderFieldMenu(Field field)
        {
            Console.Clear();
            fieldDrawer.draw(field);
            keyProcessor.waitForKey();
        }

        private static void subscribeToFieldGame(bool isFirstPlayer)
        {
            GameProcessApplicationService gameProcess = new GameProcessApplicationService();

            gameProcess.renderMyTurn += renderMyTurn;
            gameProcess.renderEnemyTurn += renderNotMyTurn;

            keyHandler = new GameKeyHandler(keyProcessor, gameProcess);

            game = gameProcess;

            gameProcess.initGameProcess(isFirstPlayer);
        }

        static void renderMyTurn(Field myField, Field enemyField)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("... YourTurn ...");
            Console.ForegroundColor = ConsoleColor.Gray;
            fieldDrawer.draw(myField, enemyField);
            keyProcessor.waitForKey();
        }

        static void renderNotMyTurn(Field myField, Field enemyField)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("... Waiting for other player ...");
            Console.ForegroundColor = ConsoleColor.Gray;
            fieldDrawer.draw(myField, enemyField);
            game.waitForPlayerShot();
        }
    }
}
