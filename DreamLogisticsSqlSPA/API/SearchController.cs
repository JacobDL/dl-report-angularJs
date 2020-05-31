using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DreamLogisticsSqlSPA.ControllerLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Repository.Models;
using Repository.Repositories;
using Repository.ViewModels;

namespace DreamLogisticsSqlSPA.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        //Gets the result by the Sql-string in the Query and the information added by the user in the QueryParams
        //Returns it as a "SqlResultViewModel" to display it in the browser
        [HttpPost]
        public IActionResult SearchQuery([FromBody] SearchParameters parameters)
        {
            QueryAndQueryParamsViewModel svm = SearchControllerLogic.GetQueryResult(parameters);

            DataTable sqlResults = QueryRequestRepository.GetSqlRequestToPage(svm);
            SqlResultViewModel resultViewModel = SearchControllerLogic.GetResultViewModel(sqlResults);

            return Ok(resultViewModel);
        }

        //Gets the result by the Sql-string in the Query and the information added by the user in the QueryParams
        //Returns it as a "FileStreamResult" to display it in an Excel document
        [HttpPost("Excel")]
        public IActionResult SearchQueryExcel([FromBody] SearchParameters parameters)
        {
            QueryAndQueryParamsViewModel svm = SearchControllerLogic.GetQueryResult(parameters);
            MemoryStream ms = QueryRequestRepository.GetSqlRequestToExcelFile(svm);

            return new FileStreamResult(ms, new MediaTypeHeaderValue("application/octet-stream"));
        }
    }
}