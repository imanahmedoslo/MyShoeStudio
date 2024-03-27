namespace MyShoeStudio.Data.Models
{
    public class Size
    {
        
            public int Id { get; set; }
            public eSize SizeValue { get; set; }

            // Relationship with ProductSizeInventory
            public virtual ICollection<ProductInventory>? ProductSizeInventories { get; set; }
        
    }
}
