using PowderCoatingManagement.Contracts;

namespace PowderCoatingManagement
{
    internal class RawMaterial : Product, ISavable
    {
        private string brandName = string.Empty;
        
        public RawMaterialType Type { get; set; }
             
        public string BrandName { get => brandName; set => brandName = value; }

        public override string Id { get; }

        //Constructor used when creating a new instance
        public RawMaterial(RawMaterialType type, string name, string brandName, float pricePerKg)
        {
            Id = Utilities.GenerateId(type);
            Type = type;
            Name = name;
            BrandName = brandName;
            PricePerKg = pricePerKg;
        }

        //Constructor to be used when loading from the file
        public RawMaterial(string id, RawMaterialType type, string name, string brandName, float pricePerKg)
        {
            Id = id;
            Type = type;
            Name = name;
            BrandName = brandName;
            PricePerKg = pricePerKg;
        }

        public string ConvertStringToSave()
        {
            return $"{Id};{Type};{Name};{BrandName};{PricePerKg}";
        }
    }
}

