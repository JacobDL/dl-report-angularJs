﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace DreamLogisticsSqlSPA.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {

        //Gets all rows from the table [Query] 
        [HttpGet]
        public IActionResult GetQueries()
        {
            List<Query> queries = QueryRepository.GetQueries();
            return Ok(queries);
        }

        //Gets all rows from the table [QueryParam] 
        [HttpGet("QueryParams")]
        public IActionResult GetAllQueryParams()
        {
            List<QueryParam> queryParams = QueryRepository.GetAllQueryParams();
            return Ok(queryParams);
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
            bool needSqlList = false;
            QueryAndQueryParamsViewModel svm = QueryControllerLogic.GetQueryViewModel(queryId, needSqlList);
            return Ok(svm);
        }

        [HttpPost("SqlList")]
        public IActionResult GetSqlList(QueryParam queryParam)
        {
            List<SqlTable> sqlList = SqlListRepository.GetSqlList(queryParam);
            return Ok(sqlList);
        }


        //Inserts the information of both the new Query and QueryParams into the table [Query] and [QueryParam]
        [HttpPost("Create")]
        public IActionResult CreateQuery([FromBody] QueryAndQueryParamsViewModel createModel)
        {
            QueryRepository.InsertQuery(createModel.Query, createModel.QueryParams);
            return Ok();
        }

        //Deletes the chosen Query and attached QueryParams from the database
        [HttpDelete("DeleteQuery/{queryId}")]
        public IActionResult DeleteQuery(int queryId)
        {
            QueryRepository queryRepository = new QueryRepository();
            queryRepository.DeleteQueryById(queryId);
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