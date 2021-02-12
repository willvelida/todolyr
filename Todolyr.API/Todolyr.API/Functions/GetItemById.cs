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
    public class GetItemById
    {
        private readonly ILogger<GetItemById> _logger;
        private readonly ITodolyrRepository _todolyrRepository;

        public GetItemById(
            ILogger<GetItemById> logger,
            ITodolyrRepository todolyrRepository)
        {
            _logger = logger;
            _todolyrRepository = todolyrRepository;
        }

        [FunctionName(nameof(GetItemById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Item/{id}")] HttpRequest req,
            string id)
        {
            IActionResult result;

            try
            {
                TodoItem item = await _todolyrRepository.GetItem(id);

                if (item == null)
                {
                    _logger.LogInformation($"Todo Item with id: {id} was not found!");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    result = new OkObjectResult(item);
                }
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
