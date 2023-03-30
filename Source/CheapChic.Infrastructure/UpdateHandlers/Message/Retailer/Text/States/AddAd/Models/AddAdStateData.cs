namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

public class AddAdStateData
{
    public string Action { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public List<Guid> Photos { get; set; } = new();
}