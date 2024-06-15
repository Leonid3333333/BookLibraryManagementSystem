using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibraryManagementSystem.Models;

public class DeleteModel : PageModel
{
    private readonly HttpClient _httpClient;

    public DeleteModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [BindProperty]
    public Book Book { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Book = await _httpClient.GetFromJsonAsync<Book>($"https://localhost:5001/api/books/{id}");
        if (Book == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/books/{id}");
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Books/Index");
        }

        return Page();
    }
}
