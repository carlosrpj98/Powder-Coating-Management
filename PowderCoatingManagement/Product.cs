namespace PowderCoatingManagement
{
    internal abstract class Product
    {
        private string name = string.Empty;

        public virtual string Id { get;}
        
        public virtual float PricePerKg { get; set; }

        public virtual string Name
        {
            get => name;
            set
            {
                if (name.Length <= 50)
                {
                    name = value;
                }
                else { Console.WriteLine("The name must have max 50 characters!"); };
            }
        }
    }
}
