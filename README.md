# Digital First Careers � Find a career website
Website for career information advice and guidance.

Build status

List of dependencies in a table

|Item	|Purpose|
|-------|-------|
|Sitefinity ||	
|SQL Azure||	
|REDIS	|Needed for local?|
|Azure Search||	
|Bing spell check	||
|Course finder	||
|LMI for all||	





## Running Locally
Introductory text about running local�
Then a section for each dependency, stating what someone would need to do to stand up the dependency, and then config settings which would need to be updated in the repo.

## Sitefinity 

Project - DFC.Digital.Web.Sitefinity

In core of our solution is the Sitefinity CMS. In order our solution to run as prerequisite the fully fledged licenced/trial version of Sitefinity have to be installed, configured and running.


�Installation: Get started with Sitefinity CMS�
https://docs.sitefinity.com/installation


Once you have Sitefinity running you can take next step of plugging Sitefinity into our solution. 

First of all you have to set the connection string to the one you are using.

And migrate the values generated by your Sitefinity installation into tokenised settings of our solution.



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

## Course finder

## LMI For All API

LMI For All API - 
http://api.lmiforall.org.uk/

and more specifically we are using 
get /ashe/estimatePayMD 
which Gets an estimation of the median and decile distribution of weekly pay for a job.


| Config File   | Key                                       | Token                       | Example value             |
|-------------| ----------------------------------------- |-----------------------      |:----------------|
|web.config |	DFC.Digital.AsheFeedAccessKey |	\_\_lmiForAllApiAccessKey\_\_	| secret phrase             | 
|web.config |	DFC.Digital.AsheEstimateMdApiGateway |	not tokanised	| http://api.lmiforall.org.uk/api/v1/ashe/estimatePayMD?soc={0}&amp;accessKey={1}            | 

You can obtain an APIAccessKey (the secret phrase)  by contacting 'LMI for All' http://www.lmiforall.org.uk/ (email <LMIforAll.dfe@education.gov.uk>)

In our solution we have SalaryCalcluator which calculates yearly salary for starter and experienced levels.
We also could overide and ignore the data provided by the LMI salary feed and display data which is stored locally in Sitefinity database.