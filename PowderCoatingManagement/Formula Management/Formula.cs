using PowderCoatingManagement.Contracts;
using System.Text;

namespace PowderCoatingManagement
{
    internal class Formula : Product, ISavable
    {
        public List<FormulaItem> FormulaItemList { get; set; }
        public DateOnly Date { get; set; }

        public override string Id { get; }

        //Constructor when instantiating new formulas
        public Formula(string name, List<FormulaItem> formulaItens, DateOnly date)
        {
            Id = Utilities.GenerateId();
            Name = name;
            FormulaItemList = formulaItens;
            PricePerKg = GetPricePerKg(formulaItens);
            Date = date;
        }

        //Constructor to load from file
        public Formula(string id, string name, List<FormulaItem> formulaItens, DateOnly date)
        {
            Id = id;
            Name = name;
            FormulaItemList = formulaItens;
            PricePerKg = GetPricePerKg(formulaItens); 
            Date = date;
        }

        public string ConvertStringToSave()
        {
            StringBuilder sb = new StringBuilder();
            foreach (FormulaItem formulaItem in FormulaItemList)
            {
                sb.Append($"{formulaItem.RawMaterialItem.Id}|{formulaItem.Amount}");
                sb.Append('\\');
            }

            return $"{Id};{Name};{PricePerKg};{Date};{sb}";
        }

        private float GetPricePerKg (List<FormulaItem> list)
        {
            float pricePerKg = 0;
            foreach (FormulaItem item in list)
            {
                pricePerKg += item.RawMaterialItem.PricePerKg * item.Amount;
            }
            return float.Round(pricePerKg, 2);
        }

    }
}