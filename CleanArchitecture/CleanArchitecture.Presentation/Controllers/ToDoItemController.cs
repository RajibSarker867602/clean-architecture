using CleanArchitecture.Application.Department.Command;
using CleanArchitecture.Application.ToDo.Command;
using CleanArchitecture.Application.ToDo.Query;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Presentation.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : BaseAPIController
    {
        /// <summary>
        /// Get all data
        /// </summary>
        /// <returns>List of data</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ToDoItem>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get()
        {
            var toDoItems = await Sender.Send(new GetAllToDoItemsQuery());
            if (toDoItems is null || toDoItems.Value is null || toDoItems.Value.Count == 0) return NoContent();

            return Ok(toDoItems.Value);
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="command">Request params</param>
        /// <returns>Return created object</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<ToDoItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] CreateToDoItemCommand command)
        {
            var result = await Sender.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
    }
}
