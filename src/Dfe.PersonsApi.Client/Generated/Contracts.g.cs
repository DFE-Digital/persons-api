//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace Dfe.PersonsApi.Client.Contracts
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface IConstituenciesClient
    {
        /// <summary>
        /// Retrieve Member of Parliament by constituency name
        /// </summary>
        /// <param name="constituencyName">The constituency name.</param>
        /// <returns>A Person object representing the Member of Parliament.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<MemberOfParliament> GetMemberOfParliamentByConstituencyAsync(string constituencyName);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Retrieve Member of Parliament by constituency name
        /// </summary>
        /// <param name="constituencyName">The constituency name.</param>
        /// <returns>A Person object representing the Member of Parliament.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<MemberOfParliament> GetMemberOfParliamentByConstituencyAsync(string constituencyName, System.Threading.CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve a collection of Member of Parliament by a collection of constituency names
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of MemberOfParliament objects.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<MemberOfParliament>> GetMembersOfParliamentByConstituenciesAsync(GetMembersOfParliamentByConstituenciesQuery request);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Retrieve a collection of Member of Parliament by a collection of constituency names
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A collection of MemberOfParliament objects.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<MemberOfParliament>> GetMembersOfParliamentByConstituenciesAsync(GetMembersOfParliamentByConstituenciesQuery request, System.Threading.CancellationToken cancellationToken);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface IEstablishmentsClient
    {
        /// <summary>
        /// Retrieve All Members Associated With an Academy by Urn
        /// </summary>
        /// <param name="urn">The URN.</param>
        /// <returns>A Collection of Persons Associated With the Academy.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(int urn);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Retrieve All Members Associated With an Academy by Urn
        /// </summary>
        /// <param name="urn">The URN.</param>
        /// <returns>A Collection of Persons Associated With the Academy.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(int urn, System.Threading.CancellationToken cancellationToken);

        /// <summary>
        /// Get Member of Parliament by School (Urn)
        /// </summary>
        /// <param name="urn">The URN.</param>
        /// <returns>Member of Parliament</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(int urn);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Get Member of Parliament by School (Urn)
        /// </summary>
        /// <param name="urn">The URN.</param>
        /// <returns>Member of Parliament</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(int urn, System.Threading.CancellationToken cancellationToken);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface ITrustsClient
    {
        /// <summary>
        /// Retrieve All Members Associated With a Trust by Either UKPRN or TRN
        /// </summary>
        /// <param name="id">The identifier (UKPRN or TRN).</param>
        /// <returns>A Collection of Persons Associated With the Trust.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<TrustGovernance>> GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(string id);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Retrieve All Members Associated With a Trust by Either UKPRN or TRN
        /// </summary>
        /// <param name="id">The identifier (UKPRN or TRN).</param>
        /// <returns>A Collection of Persons Associated With the Trust.</returns>
        /// <exception cref="PersonsApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<TrustGovernance>> GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(string id, System.Threading.CancellationToken cancellationToken);

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MemberOfParliament : Person
    {
        [Newtonsoft.Json.JsonProperty("constituencyName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ConstituencyName { get; set; }

        public string ToJson()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public static MemberOfParliament FromJson(string data)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<MemberOfParliament>(data, new Newtonsoft.Json.JsonSerializerSettings());

        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Person
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("firstName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [Newtonsoft.Json.JsonProperty("lastName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("displayName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [Newtonsoft.Json.JsonProperty("displayNameWithTitle", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DisplayNameWithTitle { get; set; }

        [Newtonsoft.Json.JsonProperty("phone", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [Newtonsoft.Json.JsonProperty("roles", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<string> Roles { get; set; }

        [Newtonsoft.Json.JsonProperty("updatedAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTime? UpdatedAt { get; set; }

        public string ToJson()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public static Person FromJson(string data)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(data, new Newtonsoft.Json.JsonSerializerSettings());

        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class GetMembersOfParliamentByConstituenciesQuery
    {
        [Newtonsoft.Json.JsonProperty("constituencyNames", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<string> ConstituencyNames { get; set; }

        public string ToJson()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public static GetMembersOfParliamentByConstituenciesQuery FromJson(string data)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<GetMembersOfParliamentByConstituenciesQuery>(data, new Newtonsoft.Json.JsonSerializerSettings());

        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AcademyGovernance : Person
    {
        [Newtonsoft.Json.JsonProperty("ukprn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Ukprn { get; set; }

        [Newtonsoft.Json.JsonProperty("urn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? Urn { get; set; }

        public string ToJson()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public static AcademyGovernance FromJson(string data)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<AcademyGovernance>(data, new Newtonsoft.Json.JsonSerializerSettings());

        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TrustGovernance : Person
    {
        [Newtonsoft.Json.JsonProperty("ukprn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Ukprn { get; set; }

        [Newtonsoft.Json.JsonProperty("trn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Trn { get; set; }

        public string ToJson()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings());

        }
        public static TrustGovernance FromJson(string data)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TrustGovernance>(data, new Newtonsoft.Json.JsonSerializerSettings());

        }

    }



    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PersonsApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public PersonsApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PersonsApiException<TResult> : PersonsApiException
    {
        public TResult Result { get; private set; }

        public PersonsApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625