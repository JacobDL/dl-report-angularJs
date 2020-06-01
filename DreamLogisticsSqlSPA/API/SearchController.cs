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
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Repository.Models;
using Repository.Repositories;
using Repository.Repositories.Query_and_QueryParam;
using Repository.ViewModels;

namespace DreamLogisticsSqlSPA.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IQueryRepository _queryRepository;
        private readonly IQueryRequestRepository _queryRequestRepository;
        public SearchController(IOptions<AppSettings> appSettings, IQueryRepository queryRepository, IQueryRequestRepository queryRequestRepository)
        {
            _queryRepository = queryRepository;
            _appSettings = appSettings.Value;
            _queryRequestRepository = queryRequestRepository;
        }
        //Gets the result by the Sql-string in the Query and the information added by the user in the QueryParams
        //Returns it as a "SqlResultViewModel" to display it in the browser
        [HttpPost]
        public IActionResult SearchQuery([FromBody] SearchParameters parameters)
        {
            SearchControllerLogic scl = new SearchControllerLogic(_queryRepository);
            QueryAndQueryParamsViewModel svm = scl.GetQueryResult(parameters);

            DataTable sqlResults = _queryRequestRepository.GetSqlRequestToPage(svm);
            SqlResultViewModel resultViewModel = SearchControllerLogic.GetResultViewModel(sqlResults);

            return Ok(resultViewModel);
        }

        //Gets the result by the Sql-string in the Query and the information added by the user in the QueryParams
        //Returns it as a "FileStreamResult" to display it in an Excel document
        [HttpPost("Excel")]
        public IActionResult SearchQueryExcel([FromBody] SearchParameters parameters)
        {
            SearchControllerLogic scl = new SearchControllerLogic(_queryRepository);
            QueryAndQueryParamsViewModel svm = scl.GetQueryResult(parameters);
            MemoryStream ms = _queryRequestRepository.GetSqlRequestToExcelFile(svm);

            return new FileStreamResult(ms, new MediaTypeHeaderValue("application/octet-stream"));
        }
    }
}