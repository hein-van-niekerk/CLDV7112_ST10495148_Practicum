using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ST10495148_Practicum.Data;
using ST10495148_Practicum.Models;
using ST10495148_Practicum.Services;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly SimulatedEventHubService _eventHub;

    [BindProperty]
    public Product Product { get; set; } = new();

    public CreateModel(ApplicationDbContext context, SimulatedEventHubService eventHub)
    {
        _context = context;
        _eventHub = eventHub;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        // Simulate sending event
        _eventHub.SendMessage($"Product created: {Product.Name}");

        return RedirectToPage("./Index");
    }
}

