# Digital First Careers – Find a career product

- CI - <img src="https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/e67ac9ef-271e-4b31-9775-a964f03525d7/806/badge" />
- master - <img src="https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/e67ac9ef-271e-4b31-9775-a964f03525d7/561/badge" />

The “Find a career” product provides the digital front end for citizens to self-serve information and advice on careers.  The product enables users to explore options for career goals and make a confident and informed choice of one that suits them.

The product uses the Sitefinity CMS solution at it's core, and uses the add-on model to extend the core Sitefinity functionality to meet the needs of citizens.  Extended functionality is written in C#, using the .NET framework 4.6.2 and ASP.NET MVC 5.  The product is dependant upon a number of Azure platform services, which are pre-requisites for running the solution.  The following section discusses these dependencies in detail, and the configuration changes that are required to use such services.

_**Note**. The “Find a career” product showcases available apprenticeship vacancies for a given job profile (such as nurse).  Apprenticeship vacancies are selected and imported into “Find a career” via a separate component, which can be found in the following repository https://github.com/SkillsFundingAgency/dfc-integration-faa._

You can see the public beta of this service at: _URL coming soon._

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Sitefinity |Content management system|	
|SQL Azure|SQL database used by Sitefinity CMS|	
|REDIS	|Cache - Not needed for single instance, used by Sitefinity in multi-tenant environment.|
|Azure Search|Search engine for searching Job Profiles  |	
|Bing spell check	|API used for spell correction of search terms |
|Course finder	|API used to display available courses related to a Job Profile |
|LMI for all|API used to obtain ASHE published salary informaiton for a Job Profile |	

## Running Locally
To run this product locally, you will need to configure the list of dependencies, once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS. 

## Local Config Files
Once you have cloned the public repo you need to remove the .template part from the configuration file names listed below.


| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.Digital/Service.Cognitive.BingSpellCheck.UnitTests | app.config.template | app.config |
| DFC.Digital/DFC.Digital.AcceptanceTest | App.config.template | App.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity/App_Data/Sitefinity/Configuration | AuthenticationConfig.config.template | AuthenticationConfig.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity/App_Data/Sitefinity/Configuration | DataConfig.config.template | DataConfig.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity/App_Data/Sitefinity/Configuration | SearchConfig.config.template | SearchConfig.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity/App_Data/Sitefinity/Configuration | SecurityConfig.config.template | SecurityConfig.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity | web.config.template | web.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity | web.Debug.config.template | web.Debug.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity | web.Release Pro.config.template | web.Release Pro.config |
| DFC.Digital/DFC.Digital.Web.Sitefinity | web.Release.config.template | web.Release Pro.config |

These files contain keys and other configuration that is particular to your local instance of the application.
You will need to set these as detailed in the sections below.configuration configuration 

## Sitefinity 

Project - DFC.Digital.Web.Sitefinity

Sitefinity CMS forms the core of this product. It uses database to store all its content, you can use SQL lite or full sql for database, use a connection string with a user who have enough access to create databases for sitefinity create its required database and configure all its components. You also need to obtain a licence to run sitefinity, for more information on installation and troubleshooting please follow instructions from the vendor 

‘Installation: Get started with Sitefinity CMS’
https://docs.sitefinity.com/installation

The following configurations needs to be replaced with your values. You may obtain the same from a blank sitefintiy installation.

| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------|
|DataConfig.config |	connectionString |	\_\_connectionString\_\_	| e.g. "data source=.;UID=sitefinityUser;PWD=sitefinityPWD;initial catalog=sitefinityDB"           | 
|ApplicationInsights.config |	InstrumentationKey |	\_\_appInsightsKey\_\_	| It's a GUID in following format "1abc2345-abc3-41g4-96a9-1ab2cd34ef56"             | 
|SecurityConfig.config |	relyingParties - key |	\_\_securityRelyingParties\_\_	| "12ABC3DCC1C686346DC40B816ADC126C011E99210BD8A8AE735CD9A54EDF5188"             | 
|SecurityConfig.config |	securityTokenIssuers - key |	\_\_securityTokenIssuers\_\_	| "12ABC3DCC1C686346DC40B816ADC126C011E99210BD8A8AE735CD9A54EDF5188"             | 
|SecurityConfig.config |	decryptionKey	| \_\_sitefinitySecurityDecryptionKey\_\_	| "0A1B12AB6CB969A878C3F06B438B6491D79A6CAD22A3982F9497A1AE21CE9740"             | 
|SecurityConfig.config |	/securityConfig/applicationRoles/role - [XML] |	\_\_sitefinitySecurityRolesSection\_\_	| \<applicationRoles\> section can be replaced              | 
|SecurityConfig.config |	validationKey |	\_\_sitefinitySecurityValidationKey\_\_	| "90A36AB52A6E2D6558AA4AB1CFCBFE15AF8EC6A77015DC53E8CCC865E1D75EC0725E8FB8D31EED4F50FE3D5B050FC7E90508E036A9E5C5B84C17B22787285FC1"             | 

After restarting of the application our content types should be recreated by the installed AddOns.
Please, be aware that PreSearchFilters AddOn should be installed first as they are used as related data in the main content type JobProfile. 

This will prepare the database and get it ready for populating with relevant content.

## SQL/Azure Database

Technically on installing Sitefinity you can choose any of the following Microsoft SQL Server Express, Microsoft SQL Server, Oracle, MySQL

'Configure and start a Sitefinity project'
https://docs.sitefinity.com/configure-and-start-a-project

In reality we would recomend using MS SQL Server for local development and in our case we have used Azure SQL to deploy the database in Azure envirenment.


## REDIS
Redis caching is not used on local development machines. But it is used when our solution is deployed in Azure Load Balanced environment. 

## Azure Search

The default search features in this solution use Azure Search as its search engine. 
If you prefer you may set up and change the solution to user other search service providers.


In order to use any of the default search features of the application you will need to set up an Azure search service and 
change the related configuration keys below.

The search service is configured in the solution in two places.

* The main web application search configuration, this file can be found in the 
DFC.Digital\DFC.Digital.Web.Sitefinity folder of the solution.

* It is also used by the acceptance tests in the soultion and these can be configured in the app.config file at 
DFC.Digital\DFC.Digital.AcceptanceTest folder.

| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------:|
|DFC.Digital.Web.Sitefinity\web.config |	DFC.Digital.JobProfileSearchIndex |	\_\_jobProfileSearchIndex\_\_ | jobprofilesIndex-debug | 
|DFC.Digital.Web.Sitefinity\web.config |	DFC.Digital.SearchServiceName |	\_\_searchServiceName\_\_	| jobprofile-search| 
|DFC.Digital.Web.Sitefinity\web.config |	DFC.Digital.SearchServiceAdminAPIKey |	\_\_searchServiceAdminApiKey\_\_	| 12C371B3C2368D0A5E15C533138C4297 | 
|DFC.Digital.AcceptanceTest\App.config |	DFC.Digital.JobProfileSearchIndex |	\_\_jobProfileSearchIndex\_\_ | jobprofilesIndex-debug | 
|DFC.Digital.AcceptanceTest\App.config |	DFC.Digital.SearchServiceName |	\_\_searchServiceName\_\_	| jobprofile-search| 
|DFC.Digital.AcceptanceTest\App.config |	DFC.Digital.SearchServiceAdminAPIKey |	\_\_searchServiceAdminApiKey\_\_	| 12C371B3C2368D0A5E15C533138C4297 | 


DFC.Digital.Web.Sitefinity\web.config

You can set up a free trial search service by following the instructions at this URL
[Azure Search Set Up](https://docs.microsoft.com/en-us/azure/search/search-create-service-portal)

Once you have completed this set up you will have a search service name, this will be part of the 
service URL endpoint that you will configured when setting up the service.

* The value for the DFC.Digital.SearchServiceName key which by default is  __searchServiceName__  need to be replaced 
with your own search service name.


* A search service API key is also required  the value for DFC.Digital.SearchServiceAdminAPIKey which by default is set to 
__searchServiceAdminApiKey__. 
This will the the PRIMARY ADMIN KEY for the Azure Search service that you have set up and can be 
found under keys in the settings section of your search service portal.

* The final bit of configuration required is to set up the search index name. 
Replace  __jobProfileSearchIndex__  with your own index name for job profile in Sitefinity.
In Sitefinity create two identical search indexes one that exactly matches your chosen index name and a second one that is your index name 
appended with -debug. 
This second index get used when you are running the solution under debug mode.

Both Indexes need to be on the JobProfile content type and you will need to add the  following additional fields for indexing 
AlternativeTitle, Overview, SalaryRange, UrlName, JobProfileCategories, JobProfileSpecialism, HiddenAlternativeTitle, TrainingRoutes, 
Enablers, EntryQualifications, Interests, PreferredTaskTypes, JobAreas.

Once configuration is complete, trigger a reindex operation from Sitefinity to build the initial indexes in Azure.


## Bing spell check

To aid users with spell checking when using the search option, the system has a "Did you mean" feature to suggest corrections.
This feature makes use of the Bing Spell Check API, which is part of Azure cognitive services.
You can set up a free trial account to use this service at this URL 
[Azure Cognitive Services.](https://azure.microsoft.com/en-us/try/cognitive-services/?api=spellcheck-api)

Once you have set up your Spell Check Service you will be able to replace the Bing Spell API configuration key in the system at the following locations.

| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------:|
|DFC.Digital.Web.Sitefinity\web.config | DFC.Cognitive.BingSpellCheck.ApiKey |	\_\_bingSpellCheckApiKey\_\_ | 6d5102d3104a4308821cbeeb1e57093 | 
|DFC.Digital.Service.Cognitive.BingSpellCheck.UnitTests\app.config | DFC.Cognitive.BingSpellCheck.ApiKey |	\_\_bingSpellCheckApiKey\_\_ | 6d5102d3104a4308821cbeeb1e57093 | 

## Course finder

When users are viewing Job profile details the service will try and present them with upto two suggestions for related current courses that may exist.
These courses are retrived from the Course Directory Provider Portal provided by the Skills Funding Agency.

#### Course Search Service
Details on using the API and requesting an account can be found at  [Course Search Service](https://opendata.coursedirectoryproviderportal.org.uk/CourseSearchService.svc) and the 
document [Course Directory API.](https://coursedirectoryproviderportal.org.uk/Content/Files/Help/Course%20Directory%20API.pdf)

#### NoSQL Database for Course Search Service Audit

The course search service logs requests, responses and failures to a NoSQL database.
You can set up a trial NoSQL database at this URL [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/?v=17.45a)
 

Once you have set up the service you will be able the replace the tokens for the configuration keys in the locations below.

| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------:|
|DFC.Digital.Web.Sitefinity\web.config | DFC.Digital.CourseSearchApiKey |	\_\_courseSearchApiKey\_\_ | 6d5102d3104a4308821cbeeb1e57093 | 
|DFC.Digital.Web.Sitefinity\web.config | DFC.Digital.CourseSearchAudit.EndpointUrl |	\_\_cosmosDbEndpointUrl\_\_ | https://coursesearch.Aadit.documents.azure.com:443 | 
|DFC.Digital.Web.Sitefinity\web.config | DFC.Digital.CourseSearchAudit.PrimaryKey |	\_\_cosmosDbPrimaryKey\_\_ | fGzzzD3Wd34lkdjfkdf dsjfksjdfkdsfjksdfj354lk45l4543jk2354325lk232DF313j2321jklldgfdgld9FWweeYkhw3Zffz==| 


## LMI For All API

LMI For All API - 
http://api.lmiforall.org.uk/

The solution  uses the Get /ashe/estimatePayMD API to get an estimation of the median and decile distribution of weekly pay for a job.


| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------|
|DFC.Digital.Web.Sitefinity\web.config |	DFC.Digital.AsheFeedAccessKey |	\_\_lmiForAllApiAccessKey\_\_	| secret phrase             | 
            | 

You can obtain an APIAccessKey (the secret phrase)  by contacting 'LMI for All' http://www.lmiforall.org.uk/ (email <LMIforAll.dfe@education.gov.uk>)

In our solution we have SalaryCalcluator which calculates yearly salary for starter and experienced levels.
The CMS system can be configured to ignore the data provided by the LMI salary feed and display data which is stored locally in Sitefinity database.
