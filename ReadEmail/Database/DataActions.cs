using System;
using System.Linq;

namespace ReadEmail.Database
{
    class DataActions
    {
        PRAEntities ObjectEntities = new PRAEntities();

        public string getProcessingFolderLocation()
        {
            try
            {
               // int intId = ObjectEntities.DataLoaderConfigs.Max(LogID => LogID.Id);
                var result = (from saveObjects in ObjectEntities.DataLoaderConfigs
                              select saveObjects.location).FirstOrDefault();
                //(ObjectEntities.DataLoaderConfigs.Select(maxId => maxId.Id).Max())

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
    }
}
