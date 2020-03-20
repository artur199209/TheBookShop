using System.Runtime.Serialization;

namespace TheBookShop.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(){}

        public DataPoint(string label, double y)
        {
            Label = label;
            Y = y;
        }
        
        [DataMember(Name = "label")]
        public string Label;
        
        [DataMember(Name = "y")]
        public double? Y;
    }
}