using Microsoft.AspNetCore.Mvc.RazorPages;
using YourApp.Data;

namespace YourApp.Pages
{
    public class TestConnectionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TestConnectionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string ConnectionStatus { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                ConnectionStatus = canConnect ? "✅ Database connection successful!" : "❌ Cannot connect to database";
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"❌ Connection failed: {ex.Message}";
            }
        }
    }
}
