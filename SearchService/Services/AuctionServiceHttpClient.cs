using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
    {

        _httpClient = httpClient;
        _config = config;

    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(i => i.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        Console.WriteLine("hello salman" + lastUpdated); 

        return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auction?date=" + lastUpdated);
    }

}
