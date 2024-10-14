using System.Text;
using System.Text.Json.Serialization;
namespace KDZ3MOD3;
[Serializable]

public class Monuments
{
    // Fields of the class that represents objects from files.

    [JsonPropertyName("ID")]
    public string? ID { get; set; }

    [JsonPropertyName("SculpName")]
    public string? SculpName { get; set; }

    [JsonPropertyName("Photo")]
    public string? Photo { get; set; }

    [JsonPropertyName("Author")]
    public string? Author { get; set; }

    [JsonPropertyName("ManufactYear")]
    public string? ManufactYear { get; set; }

    [JsonPropertyName("Material")]
    public string? Material { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("LocationPlace")]
    public string? LocationPlace { get; set; }

    [JsonPropertyName("Longitude_WGS84")]
    public string? LongitudeWGS84 { get; set; }

    [JsonPropertyName("Latitude_WGS84")]
    public string? LatitudeWGS84 { get; set; }

    [JsonPropertyName("global_id")]
    public string? GlobalId { get; set; }

    [JsonPropertyName("geodata_center")]
    public string? GeodataCenter { get; set; }

    [JsonPropertyName("geoarea")]
    public string? Geoarea { get; set; }

    /// <summary>
    /// A method representing an object in the CSV format.
    /// </summary>
    /// <returns></returns>
    public string ToScv()
    {
        var fieldsValues = new[] { ID, SculpName, Photo, Author, ManufactYear, Material, Description, LocationPlace, LongitudeWGS84, LatitudeWGS84, GlobalId, GeodataCenter, Geoarea };
        fieldsValues = (from value in fieldsValues select "\"" + value + ";\"").ToArray();
        return String.Join(';', fieldsValues);
    }

    /// <summary>
    /// A constructor with parameters.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sculpname"></param>
    /// <param name="photo"></param>
    /// <param name="author"></param>
    /// <param name="manufactyear"></param>
    /// <param name="material"></param>
    /// <param name="description"></param>
    /// <param name="locationplace"></param>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <param name="globalid"></param>
    /// <param name="geodataCenter"></param>
    /// <param name="geoarea"></param>
    public Monuments(string id, string sculpname, string photo, string author, string manufactyear,
        string material, string description, string locationplace, string longitude, string latitude,
        string globalid, string geodataCenter, string geoarea)
    {
        ID = id;
        SculpName = sculpname;
        Photo = photo;
        Author = author;
        ManufactYear = manufactyear;
        Material = material;   
        Description = description;
        LocationPlace = locationplace;
        LongitudeWGS84 = longitude;
        LatitudeWGS84 = latitude;
        GlobalId = globalid;
        GeodataCenter = geodataCenter;
        Geoarea = geoarea;
    }

    /// <summary>
    /// A constructor without parameters.
    /// </summary>
    public Monuments() { }

}
