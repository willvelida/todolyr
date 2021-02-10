using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Todolyr.API.Repositories;
using Todolyr.API.Models;

namespace Todolyr.API.Functions
{
    public class CreateItem
    {
        private readonly ILogger<CreateItem> _logger;
        private readonly ITodolyrRepository _todolyrRepository;

        public CreateItem(
            ILogger<CreateItem> logger,
            TodolyrRepository todolyrRepository)
        {
            _logger = logger;
            _todolyrRepository = todolyrRepository;
        }

        [FunctionName(nameof(CreateItem))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "Item")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                string inputBody = await new StreamReader(req.Body).ReadToEndAsync();

                TodoItem todoItem = JsonConvert.DeserializeObject<TodoItem>(inputBody);

                await _todolyrRepository.AddItem(todoItem);

                result = new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
