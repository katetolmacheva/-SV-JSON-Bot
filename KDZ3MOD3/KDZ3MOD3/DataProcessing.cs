
namespace KDZ3MOD3
{
    public static class DataProcessing
    {
        /// <summary>
        /// A method that performs sorting using LINQ queries
        /// </summary>
        /// <param name="currentClient"></param>
        /// <param name="order"></param>
        public static void Sort(User currentClient, bool order)
        {
            currentClient.Monuments = order ?
                (from monument in currentClient.Monuments orderby monument.SculpName select monument).ToList() :
                (from monument in currentClient.Monuments
                 orderby monument.ManufactYear descending
                 select monument).ToList();
        }

        /// <summary>
        /// A method that performs filtering using LINQ queries
        /// </summary>
        /// <param name="currentClient"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void Filter(User currentClient, string fieldName, string value)
        {
            currentClient.Monuments = fieldName switch
            {
                "SculpName" => (from monument in currentClient.Monuments
                               where monument.SculpName.Contains(value)
                               select monument).ToList(),
                "LocationPlace" => (from monument in currentClient.Monuments
                                   where monument.LocationPlace.Contains(value)
                                   select monument).ToList(),
                "ManufactYear Material" => (from monuments in currentClient.Monuments
                                       where (monuments.ManufactYear.Contains(value) && monuments.Material.Contains(value))
                                       select monuments).ToList(),
                _ => throw new ArgumentException()
            };
        }
    }
}
