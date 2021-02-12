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
    public class DeleteItem
    {
        private readonly ILogger<DeleteItem> _logger;
        private readonly ITodolyrRepository _todolyrRepository;

        public DeleteItem(
            ILogger<DeleteItem> logger,
            ITodolyrRepository todolyrRepository)
        {
            _logger = logger;
            _todolyrRepository = todolyrRepository;
        }

        [FunctionName(nameof(DeleteItem))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Item/{id}")] HttpRequest req,
            string id)
        {
            IActionResult result;

            try
            {
                TodoItem itemToDelete = await _todolyrRepository.GetItem(id);

                if (itemToDelete == null)
                {
                    _logger.LogWarning($"Todo Item with id: {id} not found! Cancelling Delete Operation");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    await _todolyrRepository.DeleteItem(id, itemToDelete);
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
