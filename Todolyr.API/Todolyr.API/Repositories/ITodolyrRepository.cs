using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todolyr.API.Models;

namespace Todolyr.API.Repositories
{
    public interface ITodolyrRepository
    {
        /// <summary>
        /// Adds an item to the database.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddItem(TodoItem item);

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task DeleteItem(string id, TodoItem item);

        /// <summary>
        /// Retrieves an item with the specified id from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TodoItem> GetItem(string id);

        /// <summary>
        /// Retrieves all the items from the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TodoItem>> GetAllItems();

        /// <summary>
        /// Updates the provided item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateItem(string id, TodoItem item);
    }
}
