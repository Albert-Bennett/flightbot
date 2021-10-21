namespace FlightBot.Services.DataModels.LUIS
{
    public class LUISResponse
    {
        public string query { get; set; }
        public Prediction prediction { get; set; }
    }

    public class Prediction
    {
        public string topIntent { get; set; }
        public Intents intents { get; set; }
        public Entities entities { get; set; }
    }

    public class Intents
    {
        public BookAFlight Bookaflight { get; set; }
        public FindACountry Findacountry { get; set; }
        public None None { get; set; }
    }

    public class BookAFlight
    {
        public float score { get; set; }
    }

    public class FindACountry
    {
        public float score { get; set; }
    }

    public class None
    {
        public float score { get; set; }
    }

    public class Entities
    {
        public string[] flyto { get; set; }
        public Geographyv21[] geographyV2 { get; set; }
        public string[] flyfrom { get; set; }
        public Datetimev21[] datetimeV2 { get; set; }
        public Instance instance { get; set; }
    }

    public class Instance
    {
        public FlyTo[] flyto { get; set; }
        public Geographyv2[] geographyV2 { get; set; }
        public FlyFrom[] flyfrom { get; set; }
        public Datetimev2[] datetimeV2 { get; set; }
    }

    public class FlyTo
    {
        public string type { get; set; }
        public string text { get; set; }
        public int startIndex { get; set; }
        public int length { get; set; }
        public float score { get; set; }
        public int modelTypeId { get; set; }
        public string modelType { get; set; }
        public string[] recognitionSources { get; set; }
    }

    public class Geographyv2
    {
        public string type { get; set; }
        public string text { get; set; }
        public int startIndex { get; set; }
        public int length { get; set; }
        public int modelTypeId { get; set; }
        public string modelType { get; set; }
        public string[] recognitionSources { get; set; }
    }

    public class FlyFrom
    {
        public string type { get; set; }
        public string text { get; set; }
        public int startIndex { get; set; }
        public int length { get; set; }
        public float score { get; set; }
        public int modelTypeId { get; set; }
        public string modelType { get; set; }
        public string[] recognitionSources { get; set; }
    }

    public class Datetimev2
    {
        public string type { get; set; }
        public string text { get; set; }
        public int startIndex { get; set; }
        public int length { get; set; }
        public int modelTypeId { get; set; }
        public string modelType { get; set; }
        public string[] recognitionSources { get; set; }
    }

    public class Geographyv21
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Datetimev21
    {
        public string type { get; set; }
        public Value[] values { get; set; }
    }

    public class Value
    {
        public string timex { get; set; }
        public Resolution[] resolution { get; set; }
    }

    public class Resolution
    {
        public string start { get; set; }
        public string end { get; set; }
    }
}
