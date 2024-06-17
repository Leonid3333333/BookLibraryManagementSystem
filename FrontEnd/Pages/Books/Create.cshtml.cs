using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibraryManagementSystem.Models;

public class CreateModel : PageModel
{
    private readonly HttpClient _httpClient;

    public CreateModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [BindProperty]
    public Book Book { get; set; }

    [BindProperty]
    public List<Author> Authors { get; set; } = new List<Author>();

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        Book.Authors = Authors;
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/books", Book);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Books/Index");
        }

        return Page();
    }
}
