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
using System.Collections.Generic;

namespace Todolyr.API.Functions
{
    public class GetAllItems
    {
        private readonly ILogger<GetAllItems> _logger;
        private readonly ITodolyrRepository _todolyrRepository;

        public GetAllItems(
            ILogger<GetAllItems> logger,
            ITodolyrRepository todolyrRepository)
        {
            _logger = logger;
            _todolyrRepository = todolyrRepository;
        }

        [FunctionName(nameof(GetAllItems))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Item")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                var response = await _todolyrRepository.GetAllItems();

                result = new OkObjectResult(response);
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
