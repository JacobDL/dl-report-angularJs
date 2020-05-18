using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DreamLogisticsSqlSPA.ControllerLogic;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Repositories;
using Repository.Repositories.Query_and_QueryParam;
using Repository.ViewModels;

namespace DreamLogisticsSqlSPA.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        public bool IsAdmin { get; set; } = true;

        //Gets all rows from the table [Query] 
        [HttpGet]
        public IActionResult GetQueries()
        {
            List<Query> queries = QueryRepository.GetQueries();
            return Ok(queries);
        }


        //Gets [Query] row with its QueryParams and TypeId=4 lists
        [HttpGet("List/{queryId}")]
        public IActionResult GetQueryModelWithList(int queryId)
        {
            bool needSqlList = true;
            QueryAndQueryParamsViewModel svm = QueryControllerLogic.GetQueryViewModel(queryId, needSqlList);
            return Ok(svm);
        }

        //Gets [Query] row with its QueryParams but without any TypeId=4 lists
        [HttpGet("NoList/{queryId}")]
        public IActionResult GetQueryModelWithoutList(int queryId)
        {
            if (IsAdmin)
            {
                bool needSqlList = false;
                QueryAndQueryParamsViewModel svm = QueryControllerLogic.GetQueryViewModel(queryId, needSqlList);
                return Ok(svm);
            }
            return Ok();
        }

        //Inserts the information of both the new Query and QueryParams into the table [Query] and [QueryParam]
        [HttpPost("Create")]
        public IActionResult CreateQuery([FromBody] CreateViewModel formData)
        {
            if (!IsAdmin)
            {

                Query query = new Query(formData.Sql, formData.GroupName);
                List<QueryParam> queryParams = QueryControllerLogic.ConvertQueryParams(formData);

                QueryRepository.InsertQuery(query, queryParams);
            }
            return Redirect("/#!/");
        }

        //Deletes the chosen Query and attached QueryParams from the database
        [HttpDelete("DeleteQuery/{queryId}")]
        public IActionResult DeleteQuery(int queryId)
        {
            if (IsAdmin)
            {
                QueryRepository queryRepository = new QueryRepository();
                queryRepository.DeleteQueryById(queryId);

            }
            return Ok();
        }

        //Deletes chosen QueryParams, Inserts newly added QueryParams and updates all other changes in the tables [Query] and [QueryParam]
        [HttpPut("UpdateQuery")]
        public IActionResult UpdateQuery(QueryAndQueryParamsViewModel updateModel)
        {
            QueryControllerLogic.UpdateQuery(updateModel);
            return Ok();
        }

    }
}