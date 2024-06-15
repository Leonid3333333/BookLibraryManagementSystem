using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibraryManagementSystem.Models;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;

    public IndexModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<Book> Books { get; set; }

    public async Task OnGetAsync()
    {
        Books = await _httpClient.GetFromJsonAsync<List<Book>>("https://localhost:5001/api/books");
    }
}
