using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Battle_sea.Model;

namespace Battle_sea.Services.FileDataGateway
{
    class RoomAssembler
    {

        public Room readRoom(StreamReader streamReader, out bool isLocked)
        {
            isLocked = bool.Parse(streamReader.ReadLine());

            Room result = new Room();
            result.name = streamReader.ReadLine();
            result.isFirstPlayerTime = bool.Parse(streamReader.ReadLine());

            result.firstPlayer = new Player();
            result.firstPlayer.name = streamReader.ReadLine();
            result.firstPlayer.myField = getField(streamReader);
            result.firstPlayer.enemyField = getField(streamReader);
            result.firstPlayer.myField.fourCellShips = int.Parse(streamReader.ReadLine());
            result.firstPlayer.myField.threeCellShips = int.Parse(streamReader.ReadLine());
            result.firstPlayer.myField.twoCellShips = int.Parse(streamReader.ReadLine());
            result.firstPlayer.myField.oneCellShips = int.Parse(streamReader.ReadLine());

            result.secondPlayer = new Player();
            result.secondPlayer.name = streamReader.ReadLine();
            result.secondPlayer.myField = getField(streamReader);
            result.secondPlayer.enemyField = getField(streamReader);
            result.secondPlayer.myField.fourCellShips = int.Parse(streamReader.ReadLine());
            result.secondPlayer.myField.threeCellShips = int.Parse(streamReader.ReadLine());
            result.secondPlayer.myField.twoCellShips = int.Parse(streamReader.ReadLine());
            result.secondPlayer.myField.oneCellShips = int.Parse(streamReader.ReadLine());

            return result;
        }

        public void writeRoom(StreamWriter streamWriter, Room room, bool shouldLock = false)
        {
            streamWriter.WriteLine(shouldLock);

            streamWriter.WriteLine(room.name);
            streamWriter.WriteLine(room.isFirstPlayerTime);

            streamWriter.WriteLine(room.firstPlayer.name);
            streamWriter.WriteLine(room.firstPlayer.myField.ToString());
            streamWriter.WriteLine(room.firstPlayer.enemyField.ToString());
            streamWriter.WriteLine(room.firstPlayer.myField.fourCellShips);
            streamWriter.WriteLine(room.firstPlayer.myField.threeCellShips);
            streamWriter.WriteLine(room.firstPlayer.myField.twoCellShips);
            streamWriter.WriteLine(room.firstPlayer.myField.oneCellShips);

            streamWriter.WriteLine(room.secondPlayer.name);
            streamWriter.WriteLine(room.secondPlayer.myField.ToString());
            streamWriter.WriteLine(room.secondPlayer.enemyField.ToString());
            streamWriter.WriteLine(room.secondPlayer.myField.fourCellShips);
            streamWriter.WriteLine(room.secondPlayer.myField.threeCellShips);
            streamWriter.WriteLine(room.secondPlayer.myField.twoCellShips);
            streamWriter.WriteLine(room.secondPlayer.myField.oneCellShips);
        }

        private Field getField(StreamReader sr)
        {
            string[] fieldRows = new string[10];
            for (int i = 0; i < 10; i++)
            {
                fieldRows[i] = sr.ReadLine();
            }
            Field result = new Field(fieldRows);
            return result;
        }
    }
}
