namespace PowderCoatingManagement
{
    internal class FormulaItem
    {
        private RawMaterial rawMaterialItem;
        private float amount;

        public FormulaItem(RawMaterial rawMaterialItem, float amount)
        {
            RawMaterialItem = rawMaterialItem;
            Amount = amount;
        }

        public float Amount { get => amount; set => amount = value; }
        public RawMaterial RawMaterialItem { get => rawMaterialItem; set => rawMaterialItem = value; }
    }
}
