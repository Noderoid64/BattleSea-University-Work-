using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using Battle_sea.Model;

namespace Battle_sea.Services.FileDataGateway
{
    class FileRoomProvider
    {
        private const String DEFAULT_DIR = "Rooms";
        private const String EXT = ".room";

        private RoomAssembler roomAdapter = new RoomAssembler();
        private RoomNamesAssembler roomNamesAssembler = new RoomNamesAssembler();

        public bool canWrite = true;
        public bool myLock = false;

        #region Singleton

        private static FileRoomProvider instance;

        public static FileRoomProvider getInstance()
        {
            if (instance == null)
            {
                instance = new FileRoomProvider();
            }
            return instance;
        }

        private FileRoomProvider() {
            clearAllRooms();
        }

        #endregion

        
        public IList<string> getAllRoomNames()
        {
            string[] rawFileNames = Directory.GetFiles("./" + DEFAULT_DIR, "*" + EXT);
            return roomNamesAssembler.createEntities(rawFileNames);
        }

        public IDictionary<int, string> getAllRoomNamesAsDictionary()
        {
            IDictionary<int, string> result = new Dictionary<int, string>();
            IList<string> roomNames = getAllRoomNames();
            for (int i = 0; i < roomNames.Count; i++)
            {
                result.Add(i, roomNames[i]);
            }
            return result;
        }

        public void saveRoom(Room room)
        {
            if (canWrite || myLock)
            {
                StreamWriter roomFile = new StreamWriter("./" + DEFAULT_DIR + "/" + room.name + EXT);

                roomAdapter.writeRoom(roomFile, room);

                roomFile.Close();
            }
        }

        public Room getRoomByName(String roomName, bool shouldLock = false, int attempts = 0)
        {
            try
            {
                FileStream fs = new FileStream("./" + DEFAULT_DIR + "/" + roomName + EXT, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                StreamReader sr = new StreamReader(fs);

                Room result = roomAdapter.readRoom(sr, out canWrite);

                if (shouldLock)
                {
                    StreamWriter sw = new StreamWriter(fs);
                    roomAdapter.writeRoom(sw, result, shouldLock);
                    myLock = true;
                }
                fs.Close();

                return result;
            }
            catch (IOException)
            {
                Thread.Sleep(100);
                return getRoomByName(roomName, shouldLock, ++attempts);
            }
        }

        private void clearAllRooms()
        {
            string[] directories = Directory.GetDirectories("./");
            if (!directories.Any(directory => directory.Equals(DEFAULT_DIR)))
            {
                Directory.CreateDirectory("./" + DEFAULT_DIR);
            } else
            {
                DirectoryInfo di = new DirectoryInfo("./" + DEFAULT_DIR);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
    }
}
