using Dfe.PersonsApi.Application.Constituencies.Queries.GetMemberOfParliamentByConstituency;
using Dfe.PersonsApi.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Dfe.PersonsApi.Application.Constituencies.Queries.GetMemberOfParliamentByConstituencies;
using Dfe.PersonsApi.Application.Constituencies.Queries.SearchMembersOfParliament;
using Dfe.PersonsApi.Application.Common.Exceptions;
using System.Net;

namespace PersonsApi.Controllers
{
    [ApiController]
    [Authorize(Policy = "API.Read")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ConstituenciesController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// Retrieve Member of Parliament by constituency name
        /// </summary>
        /// <param name="constituencyName">The constituency name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpGet("{constituencyName}/mp")]
        [SwaggerResponse(200, "A Person object representing the Member of Parliament.", typeof(MemberOfParliament))]
        [SwaggerResponse(404, "Constituency not found.")]
        [SwaggerResponse(400, "Constituency cannot be null or empty.")]
        public async Task<IActionResult> GetMemberOfParliamentByConstituencyAsync([FromRoute] string constituencyName, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetMemberOfParliamentByConstituencyQuery(constituencyName), cancellationToken);

            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
        }

        /// <summary>
        /// Retrieve a collection of Member of Parliament by a collection of constituency names
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpPost("mps")]
        [SwaggerResponse(200, "A collection of MemberOfParliament objects.", typeof(IEnumerable<MemberOfParliament>))]
        [SwaggerResponse(400, "Constituency names cannot be null or empty.")]
        public async Task<IActionResult> GetMembersOfParliamentByConstituenciesAsync([FromBody] GetMembersOfParliamentByConstituenciesQuery request, CancellationToken cancellationToken)
        {
            var result = await sender.Send(request, cancellationToken);

            return Ok(result.Value);
        }

        /// <summary>
        /// Search for Members of Parliament by either their name or their constituency name
        /// </summary>
        /// <param name="searchTerm">A single term matched against both the Member of Parliament name and the constituency name, for example "John Moore" or "London".</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpGet("mps/search")]
        [SwaggerResponse(200, "A collection of MemberOfParliament objects matching the search term.", typeof(IEnumerable<MemberOfParliament>))]
        [SwaggerResponse(400, "Search term cannot be null or empty.")]
        public async Task<IActionResult> SearchMembersOfParliamentAsync([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new SearchMembersOfParliamentQuery(searchTerm), cancellationToken);

            return Ok(result.Value);
        }
    }
}
