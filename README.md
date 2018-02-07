# Digital First Careers – Find a career website
Website for career information advice and guidance.

Build status

List of dependencies in a table

|Item	|Purpose|
|-------|-------|
|Sitefinity ||	
|SQL Azure||	
|REDIS?	|Needed for local?|
|Azure Search||	
|Bing spell check	||
|Course finder	||
|LMI for all||	





## Running Locally
Introductory text about running local…
Then a section for each dependency, stating what someone would need to do to stand up the dependency, and then config settings which would need to be updated in the repo.

## Sitefinity 

Project - DFC.Digital.Web.Sitefinity





| Config File   | Key                                       | Token                       | Note             |
|-------------| ----------------------------------------- |-----------------------      |:----------------:|
|DataConfig.config |	connectionString |	\_\_connectionString\_\_	| todo             | 
|web.config |	DFC.Digital.CourseSearchAudit.EndpointUrl |	\_\_cosmosDbEndpointUrl\_\_	| todo             | 
|web.config	| DFC.Digital.CourseSearchAudit.PrimaryKey |	\_\_cosmosDbPrimaryKey\_\_	| todo             | 
|web.config |	DFC.Digital.CourseSearchApiKey |	\_\_courseSearchApiKey\_\_	| todo             | 
|web.config |	configuration/system.serviceModel/client/endpoint |	\_\_couseSearchWcfEnpoint\_\_	| todo             | 
|web.config |	DFC.Digital.GovUkNotifyTemplateId |	\_\_govUkNotifyEmailTemplateId\_\_	| todo             | 
|web.config |	DFC.Digital.GovUkNotifyAPIKey |	\_\_govUkNotifyKey\_\_	| todo             | 
|web.config |	DFC.Digital.JobProfileSearchIndex |	\_\_jobProfileSearchIndex\_\_	| todo             | 
|web.config |	DFC.Digital.AsheFeedAccessKey |	\_\_lmiForAllApiAccessKey\_\_	| todo             | 
|web.config |	DFC.Cognitive.BingSpellCheck.ApiKey |	\_\_bingSpellCheckApiKey\_\_ | todo             | 	
|web.config |	DFC.Digital.SearchServiceAdminAPIKey |	\_\_searchServiceAdminApiKey\_\_	| todo             | 
|web.config |	DFC.Digital.SearchServiceName |	\_\_searchServiceName\_\_	| todo             | 
|ApplicationInsights.config |	InstrumentationKey |	\_\_appInsightsKey\_\_	| todo             | 
|AuthenticationConfig.config |	clientId |	\_\_clientId\_\_	| todo             | 
|AuthenticationConfig.config |	clientSecret |	\_\_clientSecret\_\_ | todo             | 
|AuthenticationConfig.config |	encryptionKey |	\_\_sitefinityAuthenticationEncKey\_\_	| todo             | 
|SystemConfig.config |	redis settings xml |	\_\_redisConnectionString\_\_	| todo             | 
|SearchConfig.config |	azureServiceAdminKey |	\_\_searchServiceAdminApiKey\_\_	| todo             | 
|SearchConfig.config |	azureSearchServiceName |	\_\_searchServiceName\_\_	| todo             | 
|SecurityConfig.config |	relyingParties - key |	\_\_securityRelyingParties\_\_	| todo             | 
|SecurityConfig.config |	securityTokenIssuers - key |	\_\_securityTokenIssuers\_\_	| todo             | 
|SecurityConfig.config |	decryptionKey	| \_\_sitefinitySecurityDecryptionKey\_\_	| todo             | 
|SecurityConfig.config |	/securityConfig/applicationRoles/role - [XML] |	\_\_sitefinitySecurityRolesSection\_\_	| todo             | 
|SecurityConfig.config |	validationKey |	\_\_sitefinitySecurityValidationKey\_\_	| todo             | 


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





