using CleanArchitecture.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Security;

namespace CleanArchitecture.WebApi.ExceptionHandlers
{
    /// <summary>
    /// Handle all the exceptions
    /// </summary>
    /// <param name="logger"></param>
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            string errorMessage = "";
            int code = 0;

            var type = exception.GetType();

            HandleException(exception, out errorMessage, out code);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = code;
            //httpContext.Response.AddApplicationError(errorMessage);

            logger.LogError(errorMessage);
            if (exception.InnerException != null)
                logger.LogCritical(exception.InnerException.Message);

            var result = JsonConvert.SerializeObject(new ErrorDetails(code, errorMessage));
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }

        private void HandleException(Exception exception, out string errorMessage, out int code)
        {
            switch (exception)
            {
                //TODO: custom exceptions
                case NotFoundException ex:
                    code = (int) HttpStatusCode.NotFound;
                    errorMessage = ex.Message ?? "Data not found at this time! Please try again later.";
                    break;

                case DbUpdateConcurrencyException:
                    code = (int)HttpStatusCode.Conflict;
                    errorMessage =
                        "A concurrency error occurred. The data was modified by another process.";
                    break;

                case DbUpdateException dbUpdateEx:
                    code = (int)HttpStatusCode.BadRequest;
                    errorMessage = "A database error occurred while updating data.";
                    break;

                case SqlException sqlEx:
                    code = (int)HttpStatusCode.InternalServerError;
                    errorMessage = GetSqlExceptionMessage(sqlEx.Number);
                    break;

                case IndexOutOfRangeException:
                    code = (int)HttpStatusCode.InternalServerError;
                    errorMessage =
                        "An index was out of range. Please check your input and try again.";
                    break;

                case UnauthorizedAccessException:
                    code = (int)HttpStatusCode.Unauthorized;
                    errorMessage = "You are not authorized to perform this action.";
                    break;

                case FileNotFoundException:
                    code = (int)HttpStatusCode.NotFound;
                    errorMessage = "A required file was not found.";
                    break;

                case DirectoryNotFoundException:
                    code = (int)HttpStatusCode.NotFound;
                    errorMessage = "A required directory was not found.";
                    break;

                case PathTooLongException _:
                    code = (int)HttpStatusCode.NotFound;
                    errorMessage = "The specified path or file name is too long.";
                    break;

                case IOException ioEx:
                    code = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "An I/O error occurred.";
                    break;

                case SecurityException securityEx:
                    code = (int)HttpStatusCode.Forbidden;
                    errorMessage = "A security error occurred.";
                    break;

                case ArgumentNullException argNullEx:
                    code = (int)HttpStatusCode.BadRequest;
                    errorMessage = "A required argument was null.";
                    break;

                case ArgumentException argEx:
                    code = (int)HttpStatusCode.BadRequest;
                    errorMessage = "An invalid argument was provided.";
                    break;

                case InvalidOperationException invalidOpEx:
                    code = (int)HttpStatusCode.BadRequest;
                    errorMessage = "The operation is not valid in the current state.";
                    break;

                case NotImplementedException notImplEx:
                    code = (int)HttpStatusCode.NotImplemented;
                    errorMessage = "This functionality is not yet implemented.";
                    break;

                case TimeoutException timeoutEx:
                    code = (int)HttpStatusCode.RequestTimeout;
                    errorMessage = "The operation timed out.";
                    break;

                default:
                    code = (int)HttpStatusCode.InternalServerError;
                    errorMessage = exception.Message;
                    break;
            }
        }

        private string GetSqlExceptionMessage(int errorCode) =>
            errorCode switch
            {
                1205 => "A deadlock occurred. Please retry the operation.",
                2627 => "A unique constraint violation occurred. The record already exists.",
                2601 => "A duplicate key row error occurred. The record already exists.",
                547 => "Oops! It seems like you're missing some information. The record you're referring to isn't available. "
                        + "Please check the details and try again, or let us know if you need assistance.",
                8152 => "String or binary data would be truncated. Please check the data length.",
                4060 => "Cannot open database. Please check the database connection.",
                18456 => "Login failed for the user. Please check your credentials.",
                17 => "SQL Server does not exist or access is denied. Please check the server address and credentials.",
                171 => "Incorrect syntax near the specified location. Please review the SQL query.",
                53 => "We are having trouble connecting to the database server. Please check your network connection, verify the server information. If the problem persists, contact the support team for assistance.",
                _ => "A database error occurred."
            };
    }

    public sealed record ErrorDetails(int statusCode, string message);
}
