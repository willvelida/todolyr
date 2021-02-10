using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolyr.API.Models;

namespace Todolyr.API.Repositories
{
    public class TodolyrRepository : ITodolyrRepository
    {
        private IConfiguration _configuration;
        private CosmosClient _cosmosClient;
        private Container _container;

        public TodolyrRepository(
            IConfiguration configuration,
            CosmosClient cosmosClient)
        {
            _configuration = configuration;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(_configuration["DatabaseName"], _configuration["ContainerName"]);
        }

        public async Task AddItem(TodoItem item)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _container.CreateItemAsync(item, new PartitionKey(item.Owner), itemRequestOptions);
        }

        public async Task DeleteItem(string id, TodoItem item)
        {
            await _container.DeleteItemAsync<TodoItem>(id, new PartitionKey(item.Owner));
        }

        public async Task<TodoItem> GetItem(string id)
        {
            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM Items c WHERE c.id = @id")
                .WithParameter("@id", id);

            List<TodoItem> todoItems = new List<TodoItem>();

            FeedIterator<TodoItem> feedIterator = _container.GetItemQueryIterator<TodoItem>(queryDefinition);

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> items = await feedIterator.ReadNextAsync();
                todoItems.AddRange(items.Resource);
            }

            return todoItems.FirstOrDefault();
        }

        public async Task<IEnumerable<TodoItem>> GetAllItems()
        {
            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM Items c ORDER BY c._ts DESC");

            List<TodoItem> todoItems = new List<TodoItem>();

            FeedIterator<TodoItem> feedIterator = _container.GetItemQueryIterator<TodoItem>(queryDefinition);

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> items = await feedIterator.ReadNextAsync();
                todoItems.AddRange(items.Resource);
            }

            return todoItems;
        }

        public async Task UpdateItem(string id, TodoItem item)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _container.ReplaceItemAsync(
                item,
                id,
                new PartitionKey(item.Owner),
                itemRequestOptions);
        }
    }
}
