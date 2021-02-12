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
    public class UpdateItem
    {
        private readonly ILogger<UpdateItem> _logger;
        private readonly ITodolyrRepository _todolyrRepository;

        public UpdateItem(
            ILogger<UpdateItem> logger,
            ITodolyrRepository todolyrRepository)
        {
            _logger = logger;
            _todolyrRepository = todolyrRepository;
        }

        [FunctionName(nameof(UpdateItem))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Item/{id}")] HttpRequest req,
            string id)
        {
            IActionResult result;

            try
            {
                TodoItem itemToUpdate = await _todolyrRepository.GetItem(id);

                if (itemToUpdate == null)
                {
                    _logger.LogWarning($"Item with id: {id} was not found! Update not performed");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    await _todolyrRepository.UpdateItem(id, itemToUpdate);
                    result = new StatusCodeResult(StatusCodes.Status204NoContent);
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
