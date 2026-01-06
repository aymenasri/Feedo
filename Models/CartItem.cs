namespace Feedo.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        
        public decimal Subtotal => Price * Quantity;
    }
    
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        
        public int TotalItems => Items.Sum(i => i.Quantity);
        
        public void AddItem(int productId, string productName, decimal price, int quantity, string? imageUrl)
        {
            // Check by ProductId AND ProductName to allow distinct items with different extras
            var existingItem = Items.FirstOrDefault(i => i.ProductId == productId && i.ProductName == productName);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = imageUrl
                });
            }
        }
        
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
        
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }
        
        public void Clear()
        {
            Items.Clear();
        }
    }
}
