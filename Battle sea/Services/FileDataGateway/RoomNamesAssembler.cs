using System;
using System.Collections.Generic;
using System.Text;

namespace Battle_sea.Services.FileDataGateway
{
    class RoomNamesAssembler
    {
        public string createEntity(string externalEntity)
        {
            string result = null;
            if (externalEntity != null)
            {
                result = externalEntity.Substring(8);
                result = result.Substring(0, result.Length - 5);
            }
            return result;
        }

        public IList<string> createEntities(string[] externalEntities)
        {
            IList<string> result = new List<string>();
            if (externalEntities != null)
            {
                foreach (string externalEntity in externalEntities)
                {
                    result.Add(createEntity(externalEntity));
                }
            }
            return result;
        }
    }
}
